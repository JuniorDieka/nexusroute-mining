using NexusRoute.Domain.Enums;

namespace NexusRoute.Application.DTOs;

public record AssetStatusDto
{
    public Guid Id { get; init; }
    public string AssetCode { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public AssetType Type { get; init; }
    public AssetStatus Status { get; init; }
    public string? CurrentLocation { get; init; }
    public double? CurrentLatitude { get; init; }
    public double? CurrentLongitude { get; init; }
    public DateTime? LastTelemetryTime { get; init; }
    public bool IsActive { get; init; }
}
