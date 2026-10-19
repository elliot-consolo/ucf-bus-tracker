using Dapper;
using Npgsql;

namespace BusData.Service;

public class SnapshotFilterService : BackgroundService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<SnapshotFilterService> _logger;

    public SnapshotFilterService(
        IConfiguration configuration,
        ILogger<SnapshotFilterService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // 5 to 15 minutes gives the DB breathing room and allows autovacuum to keep up
        using var timer = new PeriodicTimer(TimeSpan.FromMinutes(10));

        while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken))
        {
            try
            {
                await FilterSnapshotsAsync(stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                // Graceful service shutdown
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while filtering snapshots.");
            }
        }
    }

    private async Task FilterSnapshotsAsync(CancellationToken stoppingToken)
    {
        string connectionString = _configuration.GetConnectionString("DefaultConnection")!;
        await using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync(stoppingToken);

        // Only scan records created recently rather than the entire historical table
        string sql = @"
            DELETE FROM bus_snapshots snaps
            WHERE snaps.recorded_at >= NOW() - INTERVAL '15 minutes'
              AND NOT EXISTS (
                  SELECT 1 
                  FROM stops
                  WHERE (111139 * |/ (
                      (snaps.latitude - stops.latitude)^2 + 
                      ((snaps.longitude - stops.longitude) * cos(radians(snaps.latitude)))^2
                  )) <= stops.radius_meters
                  AND snaps.route_id = stops.route_id
              );";

        var command = new CommandDefinition(sql, cancellationToken: stoppingToken, commandTimeout: 120);
        int rowsDeleted = await connection.ExecuteAsync(command);

        if (rowsDeleted > 0)
        {
            _logger.LogInformation("Deleted {Count} unnecessary bus snapshots from database.", rowsDeleted);
        }
    }
}