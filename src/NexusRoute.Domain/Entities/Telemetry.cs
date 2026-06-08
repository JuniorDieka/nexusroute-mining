using NexusRoute.Domain.ValueObjects;

namespace NexusRoute.Domain.Entities;

public class Telemetry
{
    public Guid Id { get; set; }
    public Guid AssetId { get; set; }
    public Asset Asset { get; set; } = null!;
    
    public double EngineTemperatureCelsius { get; set; }
    public double PayloadWeightTonnes { get; set; }
    public double TirePressurePsi { get; set; }
    public double FuelLevelPercentage { get; set; }
    
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double? Altitude { get; set; }
    
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public DateTime ReceivedAt { get; set; } = DateTime.UtcNow;

    public GpsPosition GetPosition() => new(Latitude, Longitude, Altitude, Timestamp);

    public bool ExceedsThreshold(TelemetryThresholds thresholds, out string? issue)
    {
        issue = null;

        if (EngineTemperatureCelsius >= thresholds.MaxEngineTemperatureCelsius)
        {
            issue = $"Engine temperature critical: {EngineTemperatureCelsius:F1}°C";
            return true;
        }

        if (TirePressurePsi <= thresholds.MinTirePressurePsi)
        {
            issue = $"Tire pressure critical: {TirePressurePsi:F1} PSI";
            return true;
        }

        if (PayloadWeightTonnes >= thresholds.MaxPayloadTonnes)
        {
            issue = $"Payload overload: {PayloadWeightTonnes:F1} tonnes";
            return true;
        }

        if (FuelLevelPercentage <= thresholds.MinFuelPercentage)
        {
            issue = $"Fuel critically low: {FuelLevelPercentage:F1}%";
            return true;
        }

        return false;
    }

    public bool HasWarning(TelemetryThresholds thresholds, out string? warning)
    {
        warning = null;

        if (EngineTemperatureCelsius >= thresholds.WarningEngineTemperatureCelsius &&
            EngineTemperatureCelsius < thresholds.MaxEngineTemperatureCelsius)
        {
            warning = $"Engine temperature elevated: {EngineTemperatureCelsius:F1}°C";
            return true;
        }

        if (TirePressurePsi <= thresholds.WarningTirePressurePsi &&
            TirePressurePsi > thresholds.MinTirePressurePsi)
        {
            warning = $"Tire pressure low: {TirePressurePsi:F1} PSI";
            return true;
        }

        if (PayloadWeightTonnes >= thresholds.WarningPayloadTonnes &&
            PayloadWeightTonnes < thresholds.MaxPayloadTonnes)
        {
            warning = $"Payload near limit: {PayloadWeightTonnes:F1} tonnes";
            return true;
        }

        if (FuelLevelPercentage <= thresholds.WarningFuelPercentage &&
            FuelLevelPercentage > thresholds.MinFuelPercentage)
        {
            warning = $"Fuel low: {FuelLevelPercentage:F1}%";
            return true;
        }

        return false;
    }
}
