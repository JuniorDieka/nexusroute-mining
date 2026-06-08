using NexusRoute.Domain.Entities;
using NexusRoute.Domain.Enums;

namespace NexusRoute.Domain.Services;

public interface ITonnageAggregator
{
    double CalculateTotalTonnage(IEnumerable<OreMovementLog> logs, MaterialType? materialType = null);
    
    double CalculateAverageGrade(IEnumerable<OreMovementLog> logs);
    
    Dictionary<MaterialType, double> GetTonnageByMaterialType(IEnumerable<OreMovementLog> logs);
    
    double CalculateDailyProduction(IEnumerable<OreMovementLog> logs, DateTime date);
}
