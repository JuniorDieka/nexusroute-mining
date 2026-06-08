using FluentValidation;
using Microsoft.Extensions.Logging;
using NexusRoute.Application.DTOs;
using NexusRoute.Application.Interfaces;
using NexusRoute.Domain.Entities;
using NexusRoute.Domain.Services;

namespace NexusRoute.Application.Services;

public class ProductionService
{
    private readonly IOreMovementRepository _oreMovementRepository;
    private readonly IAssetRepository _assetRepository;
    private readonly ITonnageAggregator _tonnageAggregator;
    private readonly INotificationHub _notificationHub;
    private readonly IValidator<OreMovementDto> _validator;
    private readonly ILogger<ProductionService> _logger;

    public ProductionService(
        IOreMovementRepository oreMovementRepository,
        IAssetRepository assetRepository,
        ITonnageAggregator tonnageAggregator,
        INotificationHub notificationHub,
        IValidator<OreMovementDto> validator,
        ILogger<ProductionService> logger)
    {
        _oreMovementRepository = oreMovementRepository ?? throw new ArgumentNullException(nameof(oreMovementRepository));
        _assetRepository = assetRepository ?? throw new ArgumentNullException(nameof(assetRepository));
        _tonnageAggregator = tonnageAggregator ?? throw new ArgumentNullException(nameof(tonnageAggregator));
        _notificationHub = notificationHub ?? throw new ArgumentNullException(nameof(notificationHub));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<OreMovementResponseDto> LogOreMovementAsync(OreMovementDto dto, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(dto, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var asset = await _assetRepository.GetByIdAsync(dto.AssetId, cancellationToken);
        if (asset == null)
        {
            throw new InvalidOperationException($"Asset with ID {dto.AssetId} not found");
        }

        var log = new OreMovementLog
        {
            Id = Guid.NewGuid(),
            AssetId = dto.AssetId,
            EventType = dto.EventType,
            MaterialType = dto.MaterialType,
            TonnageEstimate = dto.TonnageEstimate,
            GradeEstimate = dto.GradeEstimate,
            SourceLocation = dto.SourceLocation,
            DestinationLocation = dto.DestinationLocation,
            SourceLatitude = dto.SourceLatitude,
            SourceLongitude = dto.SourceLongitude,
            DestinationLatitude = dto.DestinationLatitude,
            DestinationLongitude = dto.DestinationLongitude,
            EventTime = dto.EventTime ?? DateTime.UtcNow,
            ReceivedAt = DateTime.UtcNow,
            CycleId = dto.CycleId,
            OperatorName = dto.OperatorName,
            Notes = dto.Notes
        };

        await _oreMovementRepository.AddAsync(log, cancellationToken);

        _logger.LogInformation("Ore movement logged: {EventType} - {Tonnage}t by {AssetCode}",
            log.EventType, log.TonnageEstimate, asset.AssetCode);

        return new OreMovementResponseDto
        {
            Id = log.Id,
            AssetId = log.AssetId,
            AssetCode = asset.AssetCode,
            EventType = log.EventType,
            MaterialType = log.MaterialType,
            TonnageEstimate = log.TonnageEstimate,
            GradeEstimate = log.GradeEstimate,
            SourceLocation = log.SourceLocation,
            DestinationLocation = log.DestinationLocation,
            EventTime = log.EventTime
        };
    }

    public async Task<ProductionSummaryDto> GetProductionSummaryAsync(
        DateTime startDate,
        DateTime endDate,
        CancellationToken cancellationToken = default)
    {
        var logs = await _oreMovementRepository.GetByDateRangeAsync(startDate, endDate, cancellationToken);
        var logsList = logs.ToList();

        var totalTonnage = _tonnageAggregator.CalculateTotalTonnage(logsList);
        var averageGrade = _tonnageAggregator.CalculateAverageGrade(logsList);
        var tonnageByMaterial = _tonnageAggregator.GetTonnageByMaterialType(logsList);

        return new ProductionSummaryDto
        {
            TotalTonnage = totalTonnage,
            AverageGrade = averageGrade,
            TonnageByMaterial = tonnageByMaterial,
            TargetTonnage = 12000,
            AchievementPercentage = totalTonnage / 12000 * 100,
            IsOnTrack = totalTonnage >= 12000 * 0.9
        };
    }

    public async Task<IEnumerable<DailyProductionDto>> GetDailyProductionAsync(
        DateTime startDate,
        DateTime endDate,
        CancellationToken cancellationToken = default)
    {
        var logs = await _oreMovementRepository.GetByDateRangeAsync(startDate, endDate, cancellationToken);
        var logsList = logs.ToList();

        var dailyProduction = new List<DailyProductionDto>();
        
        for (var date = startDate.Date; date <= endDate.Date; date = date.AddDays(1))
        {
            var dailyLogs = logsList.Where(l => l.EventTime.Date == date).ToList();
            var tonnage = _tonnageAggregator.CalculateDailyProduction(dailyLogs, date);
            var grade = dailyLogs.Any() ? _tonnageAggregator.CalculateAverageGrade(dailyLogs) : (double?)null;

            dailyProduction.Add(new DailyProductionDto
            {
                Date = date,
                Tonnage = tonnage,
                AverageGrade = grade
            });
        }

        return dailyProduction;
    }
}
