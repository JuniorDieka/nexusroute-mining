using NexusRoute.Domain.Entities;
using NexusRoute.Domain.Enums;
using NexusRoute.Domain.ValueObjects;

namespace NexusRoute.Domain.Services;

public class HealthMonitor : IHealthMonitor
{
    public (AlertSeverity? severity, string? message) EvaluateTelemetry(
        Telemetry telemetry,
        TelemetryThresholds thresholds)
    {
        if (telemetry == null || thresholds == null)
            return (null, null);

        if (telemetry.ExceedsThreshold(thresholds, out var criticalIssue))
        {
            return (AlertSeverity.Critical, criticalIssue);
        }

        if (telemetry.HasWarning(thresholds, out var warning))
        {
            return (AlertSeverity.Warning, warning);
        }

        return (null, null);
    }

    public bool RequiresImmediateAttention(Telemetry telemetry, TelemetryThresholds thresholds)
    {
        if (telemetry == null || thresholds == null)
            return false;

        return telemetry.ExceedsThreshold(thresholds, out _);
    }

    public AlertType? DetermineAlertType(Telemetry telemetry, TelemetryThresholds thresholds)
    {
        if (telemetry == null || thresholds == null)
            return null;

        if (telemetry.EngineTemperatureCelsius >= thresholds.WarningEngineTemperatureCelsius)
            return AlertType.EngineTemperature;

        if (telemetry.TirePressurePsi <= thresholds.WarningTirePressurePsi)
            return AlertType.TirePressure;

        if (telemetry.PayloadWeightTonnes >= thresholds.WarningPayloadTonnes)
            return AlertType.PayloadOverload;

        if (telemetry.FuelLevelPercentage <= thresholds.WarningFuelPercentage)
            return AlertType.FuelLow;

        return null;
    }
}
