namespace NexusRoute.Domain.Entities;

public class Convoy
{
    public Guid Id { get; set; }
    public string ConvoyCode { get; set; } = string.Empty;
    public Guid RouteId { get; set; }
    public Route Route { get; set; } = null!;
    
    public Guid LeadAssetId { get; set; }
    public Asset LeadAsset { get; set; } = null!;
    
    public string CargoType { get; set; } = string.Empty;
    public double CargoValueUsd { get; set; }
    
    public DateTime ScheduledDepartureTime { get; set; }
    public DateTime? ActualDepartureTime { get; set; }
    public DateTime? ActualArrivalTime { get; set; }
    
    public string Status { get; set; } = "Scheduled";
    public bool IsHighPriority { get; set; } = true;
    public bool IsActive { get; set; } = true;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<ConvoyCheckpointLog> CheckpointLogs { get; set; } = new List<ConvoyCheckpointLog>();

    public bool IsInTransit => ActualDepartureTime.HasValue && !ActualArrivalTime.HasValue;
    
    public bool IsCompleted => ActualArrivalTime.HasValue;

    public void Start()
    {
        ActualDepartureTime = DateTime.UtcNow;
        Status = "InTransit";
        UpdatedAt = DateTime.UtcNow;
    }

    public void Complete()
    {
        ActualArrivalTime = DateTime.UtcNow;
        Status = "Completed";
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarkAsDeviated(string reason)
    {
        Status = $"Deviated: {reason}";
        UpdatedAt = DateTime.UtcNow;
    }
}
