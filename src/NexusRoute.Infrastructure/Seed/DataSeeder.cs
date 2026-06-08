using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NexusRoute.Domain.Entities;
using NexusRoute.Domain.Enums;
using NexusRoute.Infrastructure.Data;

namespace NexusRoute.Infrastructure.Seed;

public class DataSeeder
{
    private readonly NexusRouteDbContext _context;
    private readonly ILogger<DataSeeder> _logger;

    public DataSeeder(NexusRouteDbContext context, ILogger<DataSeeder> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task SeedAsync()
    {
        _logger.LogInformation("Starting data seeding for Kivu Ridge Gold Operations (FICTIONAL)");

        if (await _context.Assets.AnyAsync())
        {
            _logger.LogInformation("Database already seeded, skipping");
            return;
        }

        await SeedAssetsAsync();
        await SeedRoutesAndCheckpointsAsync();
        await SeedProductionQuotasAsync();

        _logger.LogInformation("Data seeding completed successfully");
    }

    private async Task SeedAssetsAsync()
    {
        var assets = new List<Asset>
        {
            new() { Id = Guid.NewGuid(), AssetCode = "HT-001", Name = "Haul Truck Alpha", Type = AssetType.HaulTruck, Status = AssetStatus.Idle, CurrentLocation = "North Pit", CurrentLatitude = -2.1234, CurrentLongitude = 28.5678, IsActive = true },
            new() { Id = Guid.NewGuid(), AssetCode = "HT-002", Name = "Haul Truck Bravo", Type = AssetType.HaulTruck, Status = AssetStatus.Idle, CurrentLocation = "North Pit", CurrentLatitude = -2.1245, CurrentLongitude = 28.5689, IsActive = true },
            new() { Id = Guid.NewGuid(), AssetCode = "HT-003", Name = "Haul Truck Charlie", Type = AssetType.HaulTruck, Status = AssetStatus.Idle, CurrentLocation = "South Pit", CurrentLatitude = -2.1356, CurrentLongitude = 28.5801, IsActive = true },
            new() { Id = Guid.NewGuid(), AssetCode = "HT-004", Name = "Haul Truck Delta", Type = AssetType.HaulTruck, Status = AssetStatus.Idle, CurrentLocation = "South Pit", CurrentLatitude = -2.1367, CurrentLongitude = 28.5812, IsActive = true },
            new() { Id = Guid.NewGuid(), AssetCode = "HT-005", Name = "Haul Truck Echo", Type = AssetType.HaulTruck, Status = AssetStatus.Idle, CurrentLocation = "Primary Crusher", CurrentLatitude = -2.1278, CurrentLongitude = 28.5734, IsActive = true },
            
            new() { Id = Guid.NewGuid(), AssetCode = "EX-001", Name = "Excavator North-1", Type = AssetType.Excavator, Status = AssetStatus.Idle, CurrentLocation = "North Pit Face A", CurrentLatitude = -2.1240, CurrentLongitude = 28.5680, IsActive = true },
            new() { Id = Guid.NewGuid(), AssetCode = "EX-002", Name = "Excavator South-1", Type = AssetType.Excavator, Status = AssetStatus.Idle, CurrentLocation = "South Pit Face B", CurrentLatitude = -2.1360, CurrentLongitude = 28.5805, IsActive = true },
            
            new() { Id = Guid.NewGuid(), AssetCode = "DR-001", Name = "Drill Rig Alpha", Type = AssetType.DrillRig, Status = AssetStatus.Idle, CurrentLocation = "North Pit Bench 3", CurrentLatitude = -2.1250, CurrentLongitude = 28.5690, IsActive = true },
            new() { Id = Guid.NewGuid(), AssetCode = "DR-002", Name = "Drill Rig Bravo", Type = AssetType.DrillRig, Status = AssetStatus.Idle, CurrentLocation = "South Pit Bench 2", CurrentLatitude = -2.1370, CurrentLongitude = 28.5815, IsActive = true },
            
            new() { Id = Guid.NewGuid(), AssetCode = "FT-001", Name = "Fuel Tanker 1", Type = AssetType.FuelTanker, Status = AssetStatus.Idle, CurrentLocation = "Fuel Depot", CurrentLatitude = -2.1290, CurrentLongitude = 28.5750, IsActive = true },
            
            new() { Id = Guid.NewGuid(), AssetCode = "AC-001", Name = "Armored Convoy Lead", Type = AssetType.ArmoredConvoy, Status = AssetStatus.Idle, CurrentLocation = "Secure Compound", CurrentLatitude = -2.1300, CurrentLongitude = 28.5760, IsActive = true },
            new() { Id = Guid.NewGuid(), AssetCode = "AC-002", Name = "Armored Convoy Escort", Type = AssetType.ArmoredConvoy, Status = AssetStatus.Idle, CurrentLocation = "Secure Compound", CurrentLatitude = -2.1302, CurrentLongitude = 28.5762, IsActive = true }
        };

        await _context.Assets.AddRangeAsync(assets);
        await _context.SaveChangesAsync();
        
        _logger.LogInformation("Seeded {Count} assets for Kivu Ridge operations", assets.Count);
    }

    private async Task SeedRoutesAndCheckpointsAsync()
    {
        var routes = new List<Route>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Kivu Ridge to Goma Refinery",
                Description = "Primary doré transport route through mountain corridor",
                StartLocation = "Kivu Ridge Secure Compound",
                EndLocation = "Goma Refinery Checkpoint",
                StartLatitude = -2.1300,
                StartLongitude = 28.5760,
                EndLatitude = -1.6740,
                EndLongitude = 29.2280,
                EstimatedDistanceKm = 85.0,
                EstimatedDuration = TimeSpan.FromHours(3.5),
                GeofenceRadiusKm = 3.0,
                IsActive = true
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "North Pit to Primary Crusher",
                Description = "Main haulage route from North Pit",
                StartLocation = "North Pit Loading Zone",
                EndLocation = "Primary Crusher",
                StartLatitude = -2.1234,
                StartLongitude = 28.5678,
                EndLatitude = -2.1278,
                EndLongitude = 28.5734,
                EstimatedDistanceKm = 5.2,
                EstimatedDuration = TimeSpan.FromMinutes(12),
                GeofenceRadiusKm = 1.0,
                IsActive = true
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "South Pit to Primary Crusher",
                Description = "Main haulage route from South Pit",
                StartLocation = "South Pit Loading Zone",
                EndLocation = "Primary Crusher",
                StartLatitude = -2.1356,
                StartLongitude = 28.5801,
                EndLatitude = -2.1278,
                EndLongitude = 28.5734,
                EstimatedDistanceKm = 8.7,
                EstimatedDuration = TimeSpan.FromMinutes(18),
                GeofenceRadiusKm = 1.0,
                IsActive = true
            }
        };

        await _context.Routes.AddRangeAsync(routes);
        await _context.SaveChangesAsync();

        var convoyRoute = routes[0];
        var checkpoints = new List<Checkpoint>
        {
            new()
            {
                Id = Guid.NewGuid(),
                RouteId = convoyRoute.Id,
                Name = "Kivu Ridge Gate",
                SequenceNumber = 1,
                Latitude = -2.1300,
                Longitude = 28.5760,
                GeofenceRadiusKm = 0.5,
                ExpectedTimeFromStart = TimeSpan.Zero,
                AllowedDeviationTime = TimeSpan.FromMinutes(10),
                IsMandatory = true,
                IsActive = true
            },
            new()
            {
                Id = Guid.NewGuid(),
                RouteId = convoyRoute.Id,
                Name = "Mountain Pass Checkpoint",
                SequenceNumber = 2,
                Latitude = -1.9500,
                Longitude = 28.8500,
                GeofenceRadiusKm = 0.5,
                ExpectedTimeFromStart = TimeSpan.FromHours(1.5),
                AllowedDeviationTime = TimeSpan.FromMinutes(15),
                IsMandatory = true,
                IsActive = true
            },
            new()
            {
                Id = Guid.NewGuid(),
                RouteId = convoyRoute.Id,
                Name = "Valley Security Post",
                SequenceNumber = 3,
                Latitude = -1.8000,
                Longitude = 29.0500,
                GeofenceRadiusKm = 0.5,
                ExpectedTimeFromStart = TimeSpan.FromHours(2.5),
                AllowedDeviationTime = TimeSpan.FromMinutes(15),
                IsMandatory = true,
                IsActive = true
            },
            new()
            {
                Id = Guid.NewGuid(),
                RouteId = convoyRoute.Id,
                Name = "Goma Refinery Gate",
                SequenceNumber = 4,
                Latitude = -1.6740,
                Longitude = 29.2280,
                GeofenceRadiusKm = 0.5,
                ExpectedTimeFromStart = TimeSpan.FromHours(3.5),
                AllowedDeviationTime = TimeSpan.FromMinutes(20),
                IsMandatory = true,
                IsActive = true
            }
        };

        await _context.Checkpoints.AddRangeAsync(checkpoints);
        await _context.SaveChangesAsync();
        
        _logger.LogInformation("Seeded {RouteCount} routes and {CheckpointCount} checkpoints", routes.Count, checkpoints.Count);
    }

    private async Task SeedProductionQuotasAsync()
    {
        var startOfWeek = DateTime.UtcNow.Date.AddDays(-(int)DateTime.UtcNow.DayOfWeek);
        
        var quotas = new List<ProductionQuota>
        {
            new()
            {
                Id = Guid.NewGuid(),
                PeriodName = "Current Week",
                StartDate = startOfWeek,
                EndDate = startOfWeek.AddDays(7),
                TargetTonnage = 12000,
                TargetGrade = 3.5,
                MaterialType = MaterialType.Ore,
                ActualTonnage = 0,
                ActualGrade = 0
            }
        };

        await _context.ProductionQuotas.AddRangeAsync(quotas);
        await _context.SaveChangesAsync();
        
        _logger.LogInformation("Seeded {Count} production quotas", quotas.Count);
    }
}
