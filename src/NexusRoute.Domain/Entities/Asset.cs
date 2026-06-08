using NexusRoute.Domain.Enums;
using NexusRoute.Domain.ValueObjects;

namespace NexusRoute.Domain.Entities;

public class Asset
{
    public Guid Id { get; set; }
    public string AssetCode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public AssetType Type { get; set; }
    public AssetStatus Status { get; set; }
    public string? CurrentLocation { get; set; }
    public double? CurrentLatitude { get; set; }
    public double? CurrentLongitude { get; set; }
    public DateTime? LastTelemetryTime { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Telemetry> TelemetryRecords { get; set; } = new List<Telemetry>();
    public ICollection<OreMovementLog> OreMovementLogs { get; set; } = new List<OreMovementLog>();

    public GpsPosition? GetCurrentPosition()
    {
        if (!CurrentLatitude.HasValue || !CurrentLongitude.HasValue)
            return null;

        return new GpsPosition(
            CurrentLatitude.Value,
            CurrentLongitude.Value,
            timestamp: LastTelemetryTime ?? DateTime.UtcNow
        );
    }

    public void UpdatePosition(GpsPosition position)
    {
        if (position == null)
            throw new ArgumentNullException(nameof(position));

        CurrentLatitude = position.Latitude;
        CurrentLongitude = position.Longitude;
        LastTelemetryTime = position.Timestamp;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateStatus(AssetStatus newStatus)
    {
        Status = newStatus;
        UpdatedAt = DateTime.UtcNow;
    }
}
