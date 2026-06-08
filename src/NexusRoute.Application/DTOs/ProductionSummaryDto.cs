using NexusRoute.Domain.Enums;

namespace NexusRoute.Application.DTOs;

public record ProductionSummaryDto
{
    public double TotalTonnage { get; init; }
    public double AverageGrade { get; init; }
    public Dictionary<MaterialType, double> TonnageByMaterial { get; init; } = new();
    public double TargetTonnage { get; init; }
    public double AchievementPercentage { get; init; }
    public bool IsOnTrack { get; init; }
}

public record DailyProductionDto
{
    public DateTime Date { get; init; }
    public double Tonnage { get; init; }
    public double? AverageGrade { get; init; }
}
