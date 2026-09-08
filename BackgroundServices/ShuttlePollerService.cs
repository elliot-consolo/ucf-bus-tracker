using Dapper;
using Npgsql;

namespace BusData.Service;

using BusData.Models;

public class ShuttlePollerService : BackgroundService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    private readonly ILogger<ShuttlePollerService> _logger;

    public ShuttlePollerService(
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration,
        ILogger<ShuttlePollerService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
        _logger = logger;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var timer = new PeriodicTimer(TimeSpan.FromSeconds(15));

        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            try
            {
                await PollAndPersistShuttlesAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching shuttle telemetry.");
            }
        }
    }

    private async Task PollAndPersistShuttlesAsync(CancellationToken stoppingToken)
    {
        var client = _httpClientFactory.CreateClient();

        string apiURL = "https://ucf.transloc.com/Services/JSONPRelay.svc/GetMapVehiclePoints?apiKey=8882812681&isPublicMap=true";

        var vehicles = await client.GetFromJsonAsync<List<VehicleDto>>(apiURL, stoppingToken);

        if (vehicles == null || !vehicles.Any())
        {
            _logger.LogWarning("No Vehicle Data returned from endpoint");
            return;
        }

        //postgre connection to db
        string connectionString = _configuration.GetConnectionString("DefaultConnection")!;
        using var connection = new NpgsqlConnection(connectionString);

        string sql = @"
            INSERT INTO bus_snapshots (vehicle_id, route_id, latitude, longitude, speed, recorded_at, vehicle_name)
            VALUES (@VehicleID, @RouteID, @Latitude, @Longitude, @Speed, NOW(), @VehicleName);";
        // Dapper executes batch insert for all items in list
        int rowsInserted = await connection.ExecuteAsync(sql, vehicles);
        _logger.LogInformation("Logged {Count} shuttle snapshots to BusDb.", rowsInserted);
    }
}