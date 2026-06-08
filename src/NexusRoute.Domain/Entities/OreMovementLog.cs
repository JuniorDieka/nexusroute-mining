using NexusRoute.Domain.Enums;

namespace NexusRoute.Domain.Entities;

public class OreMovementLog
{
    public Guid Id { get; set; }
    public Guid AssetId { get; set; }
    public Asset Asset { get; set; } = null!;
    
    public string EventType { get; set; } = string.Empty;
    public MaterialType MaterialType { get; set; }
    public double TonnageEstimate { get; set; }
    public double? GradeEstimate { get; set; }
    
    public string SourceLocation { get; set; } = string.Empty;
    public string? DestinationLocation { get; set; }
    
    public double SourceLatitude { get; set; }
    public double SourceLongitude { get; set; }
    public double? DestinationLatitude { get; set; }
    public double? DestinationLongitude { get; set; }
    
    public DateTime EventTime { get; set; } = DateTime.UtcNow;
    public DateTime ReceivedAt { get; set; } = DateTime.UtcNow;
    
    public Guid? CycleId { get; set; }
    public string? OperatorName { get; set; }
    public string? Notes { get; set; }

    public bool IsOre => MaterialType == MaterialType.Ore || MaterialType == MaterialType.LowGradeOre;
    
    public double EstimatedValue()
    {
        if (!IsOre || !GradeEstimate.HasValue)
            return 0;

        return TonnageEstimate * GradeEstimate.Value;
    }
}
