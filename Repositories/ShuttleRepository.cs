namespace BusData.Repositories;

using Dapper;
using Npgsql;
using BusData.Models;

public class ShuttleRepository
{
    private readonly string _connectionString;
    public ShuttleRepository (IConfiguration configuration){
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    //get routes for dropdown
    public async Task<IEnumerable<RouteDto>> GetRoutesAsync()
    {
        using var connection = new NpgsqlConnection(_connectionString);
        string sql = @"
            SELECT 
                route_id AS RouteId,
                route_name AS RouteName
            FROM routes
            ORDER BY route_id;";
        return await connection.QueryAsync<RouteDto>(sql);
    }

    public async Task<IEnumerable<StopDto>> GetStopsByRouteAsync(string routeId)
    {
        using var connection = new NpgsqlConnection(_connectionString);

        string sql = @"
            SELECT
                stop_id AS StopId,
                route_id AS RouteId,
                stop_name AS StopName,
                sequence_order AS SequenceOrder,
                latitude,
                longitude,
                radius_meters AS RadiusMeters
            FROM stops
            WHERE route_id = @RouteId
            ORDER BY sequence_order;";
        return await connection.QueryAsync<StopDto>(sql, new {RouteId = routeId});
    }

    public async Task<IEnumerable<TransitAnalyticsDto>> GetAnalyticsAsync(string routeId, int startStopId, int endStopId)
    {
        using var connection = new NpgsqlConnection(_connectionString);

        var parameters = new
        {
            RouteId = routeId,
            StartStop = startStopId,
            EndStop = endStopId
        };

        string sql = @"
            WITH params AS (
                SELECT
                    @StartStop AS start_stop,
                    @EndStop AS end_stop,
                    @RouteId AS route_id
            ),

            valid_stops AS (
                SELECT 
                    stop_id,
                    s.route_id,
                    stop_name,
                    sequence_order,
                    latitude,
                    longitude,
                    radius_meters,
                    CASE
                        WHEN start_stop = sequence_order THEN 'start'
                        WHEN end_stop = sequence_order THEN 'end'
                    END AS arrival_zone
                FROM stops s
                CROSS JOIN params p
                WHERE (p.start_stop = sequence_order
                OR p.end_stop = sequence_order)
                AND s.route_id = p.route_id
                ORDER BY arrival_zone DESC
            ),

            valid_snapshots AS (
                SELECT
                    snapshot_id,
                    vehicle_id,
                    bs.route_id,
                    latitude,
                    longitude,
                    speed,
                    recorded_at,
                    vehicle_name
                FROM bus_snapshots bs
                CROSS JOIN params p
                WHERE bs.route_id = p.route_id
            ),

            geofenced_snapshots AS (
                SELECT
                    vehicle_id,
                    snaps.latitude,
                    stops.latitude,
                    snaps.longitude,
                    stops.longitude,
                    recorded_at,
                    stops.stop_id,
                    arrival_zone,
                    LAG(arrival_zone) OVER (
                        PARTITION BY vehicle_id
                        ORDER BY recorded_at
                    ) AS prev_arrival_zone
                FROM valid_snapshots snaps
                CROSS JOIN valid_stops stops
                WHERE (111139 * |/ ((snaps.latitude - stops.latitude)^2 + (snaps.longitude - stops.longitude)^2)) <= radius_meters
                ORDER BY recorded_at
            ),

            zone_entry AS (
                SELECT 
                    vehicle_id,
                    recorded_at,
                    LAG(recorded_at) OVER (
                        PARTITION BY vehicle_id 
                        ORDER BY recorded_at
                    ) AS starttime,
                    arrival_zone,
                    prev_arrival_zone
                FROM geofenced_snapshots
                WHERE (arrival_zone != prev_arrival_zone) OR prev_arrival_zone IS NULL
            ),

            valid_trips AS (
                SELECT 
                    DATE_TRUNC('minute',starttime) AS trip_minute,
                    EXTRACT(EPOCH FROM (recorded_at - starttime))/60.0 AS trip_time
                FROM zone_entry
                WHERE starttime IS NOT NULL AND arrival_zone = 'end'
                AND (EXTRACT(EPOCH FROM (recorded_at - starttime))/60.0) < 100
            ),

            minutes AS (
                SELECT minute_bucket
                FROM generate_series(
                    TIMESTAMP '2026-09-07 07:00:00',
                    TIMESTAMP '2026-09-07 19:00:00',
                    INTERVAL '1 minute'
                ) AS minute_bucket
            )

            SELECT 
                TO_CHAR(m.minute_bucket, 'HH12:MI AM') AS TimeLabel,
                EXTRACT(HOUR FROM m.minute_bucket) AS Hour,
                ROUND((AVG(vt.trip_time)::numeric),1) AS AvgDurationMinutes
            FROM minutes m
            LEFT JOIN valid_trips vt
                ON vt.trip_minute::time = m.minute_bucket::time
            GROUP BY m.minute_bucket
            ORDER BY m.minute_bucket;";
        
        return await connection.QueryAsync<TransitAnalyticsDto>(sql, parameters);
        
    }
}
