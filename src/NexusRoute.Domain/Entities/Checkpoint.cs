using NexusRoute.Domain.ValueObjects;

namespace NexusRoute.Domain.Entities;

public class Checkpoint
{
    public Guid Id { get; set; }
    public Guid RouteId { get; set; }
    public Route Route { get; set; } = null!;
    
    public string Name { get; set; } = string.Empty;
    public int SequenceNumber { get; set; }
    
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double GeofenceRadiusKm { get; set; } = 0.5;
    
    public TimeSpan ExpectedTimeFromStart { get; set; }
    public TimeSpan AllowedDeviationTime { get; set; } = TimeSpan.FromMinutes(15);
    
    public bool IsMandatory { get; set; } = true;
    public bool IsActive { get; set; } = true;

    public GpsPosition GetPosition() => new(Latitude, Longitude);
    
    public Geofence GetGeofence() => new(GetPosition(), GeofenceRadiusKm);

    public bool IsWithinGeofence(GpsPosition position) => GetGeofence().Contains(position);

    public bool IsOverdue(DateTime convoyStartTime, DateTime currentTime)
    {
        var expectedArrival = convoyStartTime.Add(ExpectedTimeFromStart);
        var latestAllowedTime = expectedArrival.Add(AllowedDeviationTime);
        return currentTime > latestAllowedTime;
    }
}
