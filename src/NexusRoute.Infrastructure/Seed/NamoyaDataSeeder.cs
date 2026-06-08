using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NexusRoute.Domain.Entities;
using NexusRoute.Domain.Enums;
using NexusRoute.Infrastructure.Data;

namespace NexusRoute.Infrastructure.Seed;

public class NamoyaDataSeeder
{
    private readonly NexusRouteDbContext _context;
    private readonly ILogger<NamoyaDataSeeder> _logger;

    public NamoyaDataSeeder(NexusRouteDbContext context, ILogger<NamoyaDataSeeder> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task SeedAsync()
    {
        _logger.LogInformation("Starting Namoya Mine data seeding (FICTIONAL - Based on Real Equipment)");

        if (await _context.Assets.AnyAsync())
        {
            _logger.LogInformation("Database already seeded, skipping");
            return;
        }

        await SeedNamoyaFleetAsync();
        await SeedRoutesAsync();
        await SeedProductionQuotasAsync();

        _logger.LogInformation("Namoya data seeding completed successfully");
    }

    private async Task SeedNamoyaFleetAsync()
    {
        var assets = new List<Asset>();
        
        // Excavators - CAT 6015B (2 units)
        assets.Add(new() { Id = Guid.NewGuid(), AssetCode = "EXC-6015B-01", Name = "CAT 6015B Excavator #1", Type = AssetType.Excavator, Status = AssetStatus.Loading, CurrentLocation = "Mt. Mwendamboko Pit", CurrentLatitude = -3.0234, CurrentLongitude = 28.1678, IsActive = true });
        assets.Add(new() { Id = Guid.NewGuid(), AssetCode = "EXC-6015B-02", Name = "CAT 6015B Excavator #2", Type = AssetType.Excavator, Status = AssetStatus.Loading, CurrentLocation = "Muviringu Pit", CurrentLatitude = -3.0345, CurrentLongitude = 28.1789, IsActive = true });
        
        // Excavators - CAT 374FL (2 units)
        assets.Add(new() { Id = Guid.NewGuid(), AssetCode = "EXC-374FL-01", Name = "CAT 374FL Excavator #1", Type = AssetType.Excavator, Status = AssetStatus.Loading, CurrentLocation = "Kakula Pit", CurrentLatitude = -3.0240, CurrentLongitude = 28.1680, IsActive = true });
        assets.Add(new() { Id = Guid.NewGuid(), AssetCode = "EXC-374FL-02", Name = "CAT 374FL Excavator #2", Type = AssetType.Excavator, Status = AssetStatus.Idle, CurrentLocation = "Maintenance Bay", CurrentLatitude = -3.0290, CurrentLongitude = 28.1750, IsActive = true });
        
        // Excavator - CAT 336D (1 unit)
        assets.Add(new() { Id = Guid.NewGuid(), AssetCode = "EXC-336D-01", Name = "CAT 336D Excavator", Type = AssetType.Excavator, Status = AssetStatus.Loading, CurrentLocation = "Namoya Summit Pit", CurrentLatitude = -3.0278, CurrentLongitude = 28.1734, IsActive = true });
        
        // Drill Rigs - DP1500i (2 units)
        assets.Add(new() { Id = Guid.NewGuid(), AssetCode = "DR-DP1500i-01", Name = "DP1500i Drill Rig #1", Type = AssetType.DrillRig, Status = AssetStatus.Idle, CurrentLocation = "Mt. Mwendamboko Pit Bench 3", CurrentLatitude = -3.0250, CurrentLongitude = 28.1690, IsActive = true });
        assets.Add(new() { Id = Guid.NewGuid(), AssetCode = "DR-DP1500i-02", Name = "DP1500i Drill Rig #2", Type = AssetType.DrillRig, Status = AssetStatus.Idle, CurrentLocation = "Muviringu Pit Bench 2", CurrentLatitude = -3.0370, CurrentLongitude = 28.1815, IsActive = true });
        
        // Dump Trucks - CAT 777D (12 units)
        for (int i = 1; i <= 12; i++)
        {
            var status = i <= 10 ? AssetStatus.Hauling : AssetStatus.Idle;
            var locations = new[] { "Mt. Mwendamboko Pit", "Muviringu Pit", "Kakula Pit", "Namoya Summit Pit" };
            var location = locations[i % 4];
            var lat = i % 2 == 0 ? -3.0234 + (i * 0.001) : -3.0345 + (i * 0.001);
            var lon = i % 2 == 0 ? 28.1678 + (i * 0.001) : 28.1789 + (i * 0.001);
            
            assets.Add(new() { 
                Id = Guid.NewGuid(), 
                AssetCode = $"DT-777D-{i:D2}", 
                Name = $"CAT 777D Dump Truck #{i}", 
                Type = AssetType.HaulTruck, 
                Status = status, 
                CurrentLocation = location, 
                CurrentLatitude = lat, 
                CurrentLongitude = lon, 
                IsActive = true 
            });
        }
        
        // Dump Trucks - VOLVO A40F (7 units)
        for (int i = 1; i <= 7; i++)
        {
            var status = i <= 6 ? AssetStatus.Hauling : AssetStatus.Maintenance;
            var location = i % 2 == 0 ? "Waste Dump" : "Ore Stockpile";
            var lat = -3.0300 + (i * 0.002);
            var lon = 28.1700 + (i * 0.002);
            
            assets.Add(new() { 
                Id = Guid.NewGuid(), 
                AssetCode = $"DT-A40F-{i:D2}", 
                Name = $"VOLVO A40F Dump Truck #{i}", 
                Type = AssetType.HaulTruck, 
                Status = status, 
                CurrentLocation = location, 
                CurrentLatitude = lat, 
                CurrentLongitude = lon, 
                IsActive = true 
            });
        }
        
        // Graders - CAT 14M (3 units)
        for (int i = 1; i <= 3; i++)
        {
            assets.Add(new() { 
                Id = Guid.NewGuid(), 
                AssetCode = $"GR-14M-{i:D2}", 
                Name = $"CAT 14M Grader #{i}", 
                Type = AssetType.Grader, 
                Status = AssetStatus.Idle, 
                CurrentLocation = "Haul Road", 
                CurrentLatitude = -3.0260 + (i * 0.003), 
                CurrentLongitude = 28.1720 + (i * 0.003), 
                IsActive = true 
            });
        }
        
        // Dozers - 12M (3 units)
        for (int i = 1; i <= 3; i++)
        {
            assets.Add(new() { 
                Id = Guid.NewGuid(), 
                AssetCode = $"DZ-12M-{i:D2}", 
                Name = $"Dozer 12M #{i}", 
                Type = AssetType.Dozer, 
                Status = AssetStatus.Idle, 
                CurrentLocation = "Pit Ramp", 
                CurrentLatitude = -3.0270 + (i * 0.002), 
                CurrentLongitude = 28.1730 + (i * 0.002), 
                IsActive = true 
            });
        }
        
        // Water Trucks - Bell B20D (4 units)
        for (int i = 1; i <= 4; i++)
        {
            assets.Add(new() { 
                Id = Guid.NewGuid(), 
                AssetCode = $"WT-B20D-{i:D2}", 
                Name = $"Bell B20D Water Truck #{i}", 
                Type = AssetType.WaterTruck, 
                Status = AssetStatus.Idle, 
                CurrentLocation = "Haul Road", 
                CurrentLatitude = -3.0280 + (i * 0.001), 
                CurrentLongitude = 28.1740 + (i * 0.001), 
                IsActive = true 
            });
        }
        
        // Wheel Loaders (2 units)
        assets.Add(new() { Id = Guid.NewGuid(), AssetCode = "WL-01", Name = "Wheel Loader #1", Type = AssetType.Excavator, Status = AssetStatus.Loading, CurrentLocation = "ROM Pad", CurrentLatitude = -3.0285, CurrentLongitude = 28.1745, IsActive = true });
        assets.Add(new() { Id = Guid.NewGuid(), AssetCode = "WL-02", Name = "Wheel Loader #2", Type = AssetType.Excavator, Status = AssetStatus.Idle, CurrentLocation = "Stockpile", CurrentLatitude = -3.0287, CurrentLongitude = 28.1747, IsActive = true });

        await _context.Assets.AddRangeAsync(assets);
        await _context.SaveChangesAsync();
        
        _logger.LogInformation("Seeded {Count} Namoya fleet assets", assets.Count);
    }

    private async Task SeedRoutesAsync()
    {
        var routes = new List<Route>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "North Pit to Primary Crusher",
                Description = "Main ore haulage route from North Pit",
                StartLocation = "North Pit Loading Zone",
                EndLocation = "Primary Crusher",
                StartLatitude = -3.0234,
                StartLongitude = 28.1678,
                EndLatitude = -3.0278,
                EndLongitude = 28.1734,
                EstimatedDistanceKm = 4.8,
                EstimatedDuration = TimeSpan.FromMinutes(10),
                GeofenceRadiusKm = 1.0,
                IsActive = true
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "South Pit to Primary Crusher",
                Description = "Secondary ore haulage route from South Pit",
                StartLocation = "South Pit Loading Zone",
                EndLocation = "Primary Crusher",
                StartLatitude = -3.0345,
                StartLongitude = 28.1789,
                EndLatitude = -3.0278,
                EndLongitude = 28.1734,
                EstimatedDistanceKm = 6.2,
                EstimatedDuration = TimeSpan.FromMinutes(14),
                GeofenceRadiusKm = 1.0,
                IsActive = true
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Waste Dump Route",
                Description = "Waste rock haulage to dump",
                StartLocation = "Pit Waste Loading",
                EndLocation = "Waste Dump",
                StartLatitude = -3.0250,
                StartLongitude = 28.1690,
                EndLatitude = -3.0400,
                EndLongitude = 28.1850,
                EstimatedDistanceKm = 8.5,
                EstimatedDuration = TimeSpan.FromMinutes(18),
                GeofenceRadiusKm = 1.5,
                IsActive = true
            }
        };

        await _context.Routes.AddRangeAsync(routes);
        await _context.SaveChangesAsync();
        
        _logger.LogInformation("Seeded {Count} routes", routes.Count);
    }

    private async Task SeedProductionQuotasAsync()
    {
        // Based on Namoya targets: 2.6 Mt/a ore, 122k-198k oz Au/year
        // Weekly: ~50,000 tonnes, Daily: ~7,100 tonnes
        var quotas = new List<ProductionQuota>
        {
            new()
            {
                Id = Guid.NewGuid(),
                PeriodName = "Daily",
                TargetTonnage = 7100.0,
                ActualTonnage = 5850.0, // 82% of target
                TargetGrade = 1.92, // g/t Au
                ActualGrade = 1.88,
                MaterialType = MaterialType.Ore,
                StartDate = DateTime.UtcNow.Date,
                EndDate = DateTime.UtcNow.Date.AddDays(1)
            },
            new()
            {
                Id = Guid.NewGuid(),
                PeriodName = "Weekly",
                TargetTonnage = 50000.0,
                ActualTonnage = 38500.0, // 77% of target
                TargetGrade = 1.92,
                ActualGrade = 1.90,
                MaterialType = MaterialType.Ore,
                StartDate = DateTime.UtcNow.Date.AddDays(-(int)DateTime.UtcNow.DayOfWeek),
                EndDate = DateTime.UtcNow.Date.AddDays(7 - (int)DateTime.UtcNow.DayOfWeek)
            },
            new()
            {
                Id = Guid.NewGuid(),
                PeriodName = "Monthly",
                TargetTonnage = 216000.0,
                TargetGrade = 1.92,
                MaterialType = MaterialType.Ore,
                StartDate = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1),
                EndDate = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1).AddMonths(1)
            },
            new()
            {
                Id = Guid.NewGuid(),
                PeriodName = "Annual",
                TargetTonnage = 2600000.0, // 2.6 Mt/a
                TargetGrade = 1.92,
                MaterialType = MaterialType.Ore,
                StartDate = new DateTime(DateTime.UtcNow.Year, 1, 1),
                EndDate = new DateTime(DateTime.UtcNow.Year, 12, 31)
            }
        };

        await _context.ProductionQuotas.AddRangeAsync(quotas);
        await _context.SaveChangesAsync();
        
        _logger.LogInformation("Seeded {Count} production quotas based on Namoya targets", quotas.Count);
    }
}
