namespace NexusRoute.Domain.Entities;

public class ConvoyCheckpointLog
{
    public Guid Id { get; set; }
    public Guid ConvoyId { get; set; }
    public Convoy Convoy { get; set; } = null!;
    
    public Guid CheckpointId { get; set; }
    public Checkpoint Checkpoint { get; set; } = null!;
    
    public DateTime? CheckInTime { get; set; }
    public bool IsMissed { get; set; }
    public bool IsOverdue { get; set; }
    
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public bool IsCompleted => CheckInTime.HasValue;
}
