using NexusRoute.Domain.Entities;
using NexusRoute.Domain.Enums;

namespace NexusRoute.Domain.Services;

public class TonnageAggregator : ITonnageAggregator
{
    public double CalculateTotalTonnage(IEnumerable<OreMovementLog> logs, MaterialType? materialType = null)
    {
        var query = logs.AsEnumerable();

        if (materialType.HasValue)
        {
            query = query.Where(l => l.MaterialType == materialType.Value);
        }

        return query.Sum(l => l.TonnageEstimate);
    }

    public double CalculateAverageGrade(IEnumerable<OreMovementLog> logs)
    {
        var oreLogs = logs.Where(l => l.IsOre && l.GradeEstimate.HasValue).ToList();

        if (!oreLogs.Any())
            return 0;

        var totalWeight = oreLogs.Sum(l => l.TonnageEstimate);
        if (totalWeight == 0)
            return 0;

        var weightedGradeSum = oreLogs.Sum(l => l.TonnageEstimate * l.GradeEstimate!.Value);
        return weightedGradeSum / totalWeight;
    }

    public Dictionary<MaterialType, double> GetTonnageByMaterialType(IEnumerable<OreMovementLog> logs)
    {
        return logs
            .GroupBy(l => l.MaterialType)
            .ToDictionary(
                g => g.Key,
                g => g.Sum(l => l.TonnageEstimate)
            );
    }

    public double CalculateDailyProduction(IEnumerable<OreMovementLog> logs, DateTime date)
    {
        var dailyLogs = logs.Where(l =>
            l.EventTime.Date == date.Date &&
            l.EventType.Equals("Dump", StringComparison.OrdinalIgnoreCase)
        );

        return dailyLogs.Sum(l => l.TonnageEstimate);
    }
}
