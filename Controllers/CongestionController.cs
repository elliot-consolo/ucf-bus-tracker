using Dapper;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

[ApiController]
[Route("api/[controller]")]
public class CongestionController : ControllerBase
{
    private readonly string _connectionString;
    public CongestionController(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
    }

    //GET api/congestion/recent
    [HttpGet("recent")]
    public async Task<IActionResult> GetRecentSnapshots([FromQuery] int limit=50)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        string sql = @"
            SELECT 
                snapshot_id AS SnapshotId,
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
}