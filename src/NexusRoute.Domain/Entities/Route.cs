using NexusRoute.Domain.ValueObjects;

namespace NexusRoute.Domain.Entities;

public class Route
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string StartLocation { get; set; } = string.Empty;
    public string EndLocation { get; set; } = string.Empty;
    
    public double StartLatitude { get; set; }
    public double StartLongitude { get; set; }
    public double EndLatitude { get; set; }
    public double EndLongitude { get; set; }
    
    public double EstimatedDistanceKm { get; set; }
    public TimeSpan EstimatedDuration { get; set; }
    
    public double GeofenceRadiusKm { get; set; } = 2.0;
    public bool IsActive { get; set; } = true;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Checkpoint> Checkpoints { get; set; } = new List<Checkpoint>();
    public ICollection<Convoy> Convoys { get; set; } = new List<Convoy>();

    public GpsPosition GetStartPosition() => new(StartLatitude, StartLongitude);
    public GpsPosition GetEndPosition() => new(EndLatitude, EndLongitude);

    public Geofence GetRouteGeofence()
    {
        var centerLat = (StartLatitude + EndLatitude) / 2;
        var centerLon = (StartLongitude + EndLongitude) / 2;
        var center = new GpsPosition(centerLat, centerLon);
        
        var radius = Math.Max(EstimatedDistanceKm / 2 + GeofenceRadiusKm, GeofenceRadiusKm);
        
        return new Geofence(center, radius);
    }
}
