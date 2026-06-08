using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NexusRoute.Application.DTOs;
using NexusRoute.Application.Interfaces;
using NexusRoute.Application.Services;
using NexusRoute.Domain.Enums;

namespace NexusRoute.Simulator.Services;

public class TelemetrySimulatorService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<TelemetrySimulatorService> _logger;
    private readonly SimulatorConfiguration _config;
    private readonly Random _random = new();

    public TelemetrySimulatorService(
        IServiceProvider serviceProvider,
        ILogger<TelemetrySimulatorService> logger,
        IOptions<SimulatorConfiguration> config)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _config = config?.Value ?? new SimulatorConfiguration();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Telemetry Simulator Service starting...");
        
        await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await GenerateTelemetryAsync(stoppingToken);
                await Task.Delay(TimeSpan.FromSeconds(_config.TelemetryIntervalSeconds), stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating simulated telemetry");
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }
    }

    private async Task GenerateTelemetryAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var assetRepository = scope.ServiceProvider.GetRequiredService<IAssetRepository>();
        var telemetryService = scope.ServiceProvider.GetRequiredService<TelemetryService>();

        var assets = await assetRepository.GetByTypeAsync(AssetType.HaulTruck, cancellationToken);
        
        foreach (var asset in assets.Take(5))
        {
            try
            {
                var telemetry = GenerateRealisticTelemetry(asset.Id, asset.CurrentLatitude, asset.CurrentLongitude);
                await telemetryService.IngestTelemetryAsync(telemetry, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to generate telemetry for asset {AssetCode}", asset.AssetCode);
            }
        }
    }

    private TelemetryDto GenerateRealisticTelemetry(Guid assetId, double? currentLat, double? currentLon)
    {
        var baseTemp = 75.0 + _random.NextDouble() * 20;
        var basePressure = 100.0 + _random.NextDouble() * 20;
        var basePayload = 150.0 + _random.NextDouble() * 40;
        var baseFuel = 30.0 + _random.NextDouble() * 60;

        var shouldTriggerAlert = _random.NextDouble() < 0.05;
        if (shouldTriggerAlert)
        {
            var alertType = _random.Next(4);
            switch (alertType)
            {
                case 0:
                    baseTemp = 96.0 + _random.NextDouble() * 8;
                    break;
                case 1:
                    basePressure = 82.0 + _random.NextDouble() * 8;
                    break;
                case 2:
                    basePayload = 201.0 + _random.NextDouble() * 15;
                    break;
                case 3:
                    baseFuel = 5.0 + _random.NextDouble() * 10;
                    break;
            }
        }

        var lat = currentLat ?? -2.1234;
        var lon = currentLon ?? 28.5678;
        
        lat += (_random.NextDouble() - 0.5) * 0.001;
        lon += (_random.NextDouble() - 0.5) * 0.001;

        return new TelemetryDto
        {
            AssetId = assetId,
            EngineTemperatureCelsius = baseTemp,
            PayloadWeightTonnes = basePayload,
            TirePressurePsi = basePressure,
            FuelLevelPercentage = baseFuel,
            Latitude = lat,
            Longitude = lon,
            Altitude = 1200.0 + _random.NextDouble() * 100,
            Timestamp = DateTime.UtcNow
        };
    }
}

public class SimulatorConfiguration
{
    public int TelemetryIntervalSeconds { get; set; } = 5;
    public int ConvoyCheckIntervalSeconds { get; set; } = 30;
}
