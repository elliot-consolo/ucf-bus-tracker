using Dapper;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

using BusData.Repositories;

[ApiController]
[Route("api/[controller]")]
public class CongestionController : ControllerBase
{  
    private readonly ShuttleRepository _repository;
    private readonly string _connectionString;
    public CongestionController(IConfiguration configuration, ShuttleRepository repository)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        _repository = repository;
    }

    //GET api/congestion/recent
    [HttpGet("recent")]
    public async Task<IActionResult> GetRecentSnapshots([FromQuery] int limit=50)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        string sql = @"
            SELECT 
                snapshot_id AS SnapshotId,
                vehicle_name AS VehicleName,
                vehicle_id AS VehicleID,
                route_id AS RouteID,
                latitude,
                longitude,
                speed,
                recorded_at AS RecordedAt
            FROM bus_snapshots 
            ORDER BY recorded_at DESC 
            LIMIT @Limit;";
        
        var snapshot = await connection.QueryAsync(sql, new {Limit = limit});
        return Ok(snapshot);

    }

    [HttpGet("routes/{routeId:int}")]
    public async Task<IActionResult> GetRouteData(int routeId, [FromQuery] int limit = 30)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        string sql = @"
            SELECT 
                snapshot_id AS SnapshotId,
                vehicle_name AS VehicleName,
                vehicle_id AS VehicleID,
                route_id AS RouteID,
                latitude,
                longitude,
                speed,
                recorded_at AS RecordedAt
            FROM bus_snapshots
            WHERE route_id = @RouteId
            ORDER BY recorded_at DESC 
            LIMIT @Limit;";
        var snapshot = await connection.QueryAsync(sql, new {RouteId = routeId, Limit = limit});
        return Ok(snapshot);
    }

    [HttpGet("routes/{routeId:int}/vehicles/{vehicleId:int}")]
    public async Task<IActionResult> GetRouteData(int routeId, int vehicleId, [FromQuery] int limit = 30)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        string sql = @"
            SELECT 
                snapshot_id AS SnapshotId,
                vehicle_name AS VehicleName,
                vehicle_id AS VehicleID,
                route_id AS RouteID,
                latitude,
                longitude,
                speed,
                recorded_at AS RecordedAt
            FROM bus_snapshots
            WHERE route_id = @RouteId AND vehicle_id = @VehicleId
            ORDER BY recorded_at DESC 
            LIMIT @Limit;";
        var snapshot = await connection.QueryAsync(sql, new {RouteId = routeId, VehicleId = vehicleId, Limit = limit});
        return Ok(snapshot);
    }

    [HttpGet("analytics")]
    public async Task<IActionResult> GetAnalytics([FromQuery] string routeId, [FromQuery] int startStopId, [FromQuery] int endStopId)
    {
        using var connection = new NpgsqlConnection(_connectionString);

        if (string.IsNullOrEmpty(routeId) || startStopId <= 0 || endStopId <= 0)
        {
            return BadRequest("Invalid route or stop value(s)");
        }

        var analytics = await _repository.GetAnalyticsAsync(routeId, startStopId, endStopId);
        return Ok(analytics);
    }

}