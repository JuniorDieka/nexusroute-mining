using NexusRoute.Domain.Entities;
using NexusRoute.Domain.Enums;
using NexusRoute.Domain.ValueObjects;

namespace NexusRoute.Domain.Services;

public interface IHealthMonitor
{
    (AlertSeverity? severity, string? message) EvaluateTelemetry(
        Telemetry telemetry,
        TelemetryThresholds thresholds);
    
    bool RequiresImmediateAttention(Telemetry telemetry, TelemetryThresholds thresholds);
    
    AlertType? DetermineAlertType(Telemetry telemetry, TelemetryThresholds thresholds);
}
