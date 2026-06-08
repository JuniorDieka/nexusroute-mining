namespace NexusRoute.Application.DTOs;

public record TelemetryDto
{
    public Guid AssetId { get; init; }
    public double EngineTemperatureCelsius { get; init; }
    public double PayloadWeightTonnes { get; init; }
    public double TirePressurePsi { get; init; }
    public double FuelLevelPercentage { get; init; }
    public double Latitude { get; init; }
    public double Longitude { get; init; }
    public double? Altitude { get; init; }
    public DateTime? Timestamp { get; init; }
}

public record TelemetryResponseDto
{
    public Guid Id { get; init; }
    public Guid AssetId { get; init; }
    public string AssetCode { get; init; } = string.Empty;
    public double EngineTemperatureCelsius { get; init; }
    public double PayloadWeightTonnes { get; init; }
    public double TirePressurePsi { get; init; }
    public double FuelLevelPercentage { get; init; }
    public double Latitude { get; init; }
    public double Longitude { get; init; }
    public DateTime Timestamp { get; init; }
}
