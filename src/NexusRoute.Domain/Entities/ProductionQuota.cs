using NexusRoute.Domain.Enums;

namespace NexusRoute.Domain.Entities;

public class ProductionQuota
{
    public Guid Id { get; set; }
    public string PeriodName { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    
    public double TargetTonnage { get; set; }
    public double? TargetGrade { get; set; }
    public MaterialType MaterialType { get; set; }
    
    public double ActualTonnage { get; set; }
    public double? ActualGrade { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public double TonnageAchievementPercentage => TargetTonnage > 0
        ? (ActualTonnage / TargetTonnage) * 100
        : 0;

    public bool IsOnTrack
    {
        get
        {
            var totalDays = (EndDate - StartDate).TotalDays;
            var elapsedDays = (DateTime.UtcNow - StartDate).TotalDays;
            
            if (totalDays <= 0 || elapsedDays < 0)
                return true;

            var expectedProgress = Math.Min(elapsedDays / totalDays, 1.0);
            var actualProgress = ActualTonnage / TargetTonnage;

            return actualProgress >= (expectedProgress * 0.9);
        }
    }

    public double RemainingTonnage => Math.Max(TargetTonnage - ActualTonnage, 0);

    public void UpdateActuals(double tonnage, double? grade = null)
    {
        ActualTonnage += tonnage;
        
        if (grade.HasValue && ActualGrade.HasValue)
        {
            var totalWeight = ActualTonnage - tonnage;
            ActualGrade = ((ActualGrade.Value * totalWeight) + (grade.Value * tonnage)) / ActualTonnage;
        }
        else if (grade.HasValue)
        {
            ActualGrade = grade.Value;
        }

        UpdatedAt = DateTime.UtcNow;
    }
}
