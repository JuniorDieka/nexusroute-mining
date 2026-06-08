using NexusRoute.Domain.Enums;

namespace NexusRoute.Application.DTOs;

public record OreMovementDto
{
    public Guid AssetId { get; init; }
    public string EventType { get; init; } = string.Empty;
    public MaterialType MaterialType { get; init; }
    public double TonnageEstimate { get; init; }
    public double? GradeEstimate { get; init; }
    public string SourceLocation { get; init; } = string.Empty;
    public string? DestinationLocation { get; init; }
    public double SourceLatitude { get; init; }
    public double SourceLongitude { get; init; }
    public double? DestinationLatitude { get; init; }
    public double? DestinationLongitude { get; init; }
    public DateTime? EventTime { get; init; }
    public Guid? CycleId { get; init; }
    public string? OperatorName { get; init; }
    public string? Notes { get; init; }
}

public record OreMovementResponseDto
{
    public Guid Id { get; init; }
    public Guid AssetId { get; init; }
    public string AssetCode { get; init; } = string.Empty;
    public string EventType { get; init; } = string.Empty;
    public MaterialType MaterialType { get; init; }
    public double TonnageEstimate { get; init; }
    public double? GradeEstimate { get; init; }
    public string SourceLocation { get; init; } = string.Empty;
    public string? DestinationLocation { get; init; }
    public DateTime EventTime { get; init; }
}
