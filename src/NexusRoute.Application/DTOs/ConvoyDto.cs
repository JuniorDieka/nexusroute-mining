namespace NexusRoute.Application.DTOs;

public record ConvoyDto
{
    public Guid Id { get; init; }
    public string ConvoyCode { get; init; } = string.Empty;
    public Guid RouteId { get; init; }
    public string RouteName { get; init; } = string.Empty;
    public Guid LeadAssetId { get; init; }
    public string LeadAssetCode { get; init; } = string.Empty;
    public string CargoType { get; init; } = string.Empty;
    public double CargoValueUsd { get; init; }
    public DateTime ScheduledDepartureTime { get; init; }
    public DateTime? ActualDepartureTime { get; init; }
    public DateTime? ActualArrivalTime { get; init; }
    public string Status { get; init; } = string.Empty;
    public bool IsHighPriority { get; init; }
    public double? CurrentLatitude { get; init; }
    public double? CurrentLongitude { get; init; }
}
