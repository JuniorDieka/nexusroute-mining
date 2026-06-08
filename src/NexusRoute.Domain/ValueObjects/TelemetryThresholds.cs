namespace NexusRoute.Domain.ValueObjects;

public record TelemetryThresholds
{
    public double MaxEngineTemperatureCelsius { get; init; }
    public double WarningEngineTemperatureCelsius { get; init; }
    public double MinTirePressurePsi { get; init; }
    public double WarningTirePressurePsi { get; init; }
    public double MaxPayloadTonnes { get; init; }
    public double WarningPayloadTonnes { get; init; }
    public double MinFuelPercentage { get; init; }
    public double WarningFuelPercentage { get; init; }

    public TelemetryThresholds(
        double maxEngineTemp,
        double warningEngineTemp,
        double minTirePressure,
        double warningTirePressure,
        double maxPayload,
        double warningPayload,
        double minFuel,
        double warningFuel)
    {
        if (warningEngineTemp >= maxEngineTemp)
            throw new ArgumentException("Warning temperature must be less than max temperature.");
        if (warningTirePressure <= minTirePressure)
            throw new ArgumentException("Warning tire pressure must be greater than min tire pressure.");
        if (warningPayload >= maxPayload)
            throw new ArgumentException("Warning payload must be less than max payload.");
        if (warningFuel <= minFuel)
            throw new ArgumentException("Warning fuel must be greater than min fuel.");

        MaxEngineTemperatureCelsius = maxEngineTemp;
        WarningEngineTemperatureCelsius = warningEngineTemp;
        MinTirePressurePsi = minTirePressure;
        WarningTirePressurePsi = warningTirePressure;
        MaxPayloadTonnes = maxPayload;
        WarningPayloadTonnes = warningPayload;
        MinFuelPercentage = minFuel;
        WarningFuelPercentage = warningFuel;
    }

    public static TelemetryThresholds CreateDefault() => new(
        maxEngineTemp: 105.0,
        warningEngineTemp: 95.0,
        minTirePressure: 80.0,
        warningTirePressure: 90.0,
        maxPayload: 220.0,
        warningPayload: 200.0,
        minFuel: 10.0,
        warningFuel: 20.0
    );
}
