using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NexusRoute.Api.Authentication;
using NexusRoute.Application.DTOs;
using NexusRoute.Application.Interfaces;

namespace NexusRoute.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = $"{RoleConstants.Dispatcher},{RoleConstants.Maintenance}")]
public class AssetsController : ControllerBase
{
    private readonly IAssetRepository _assetRepository;
    private readonly ILogger<AssetsController> _logger;

    public AssetsController(IAssetRepository assetRepository, ILogger<AssetsController> logger)
    {
        _assetRepository = assetRepository ?? throw new ArgumentNullException(nameof(assetRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AssetDto>>> GetAll()
    {
        var assets = await _assetRepository.GetAllAsync();
        var dtos = assets.Select(a => new AssetDto
        {
            Id = a.Id,
            AssetCode = a.AssetCode,
            Name = a.Name,
            Type = a.Type.ToString(),
            Status = a.Status.ToString(),
            CurrentLocation = a.CurrentLocation,
            CurrentLatitude = a.CurrentLatitude,
            CurrentLongitude = a.CurrentLongitude,
            LastTelemetryTime = a.LastTelemetryTime,
            IsActive = a.IsActive
        });
        
        return Ok(dtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AssetDto>> GetById(Guid id)
    {
        var asset = await _assetRepository.GetByIdAsync(id);
        if (asset == null)
            return NotFound();

        var dto = new AssetDto
        {
            Id = asset.Id,
            AssetCode = asset.AssetCode,
            Name = asset.Name,
            Type = asset.Type.ToString(),
            Status = asset.Status.ToString(),
            CurrentLocation = asset.CurrentLocation,
            CurrentLatitude = asset.CurrentLatitude,
            CurrentLongitude = asset.CurrentLongitude,
            LastTelemetryTime = asset.LastTelemetryTime,
            IsActive = asset.IsActive
        };
        
        return Ok(dto);
    }

    [HttpGet("active")]
    public async Task<ActionResult<IEnumerable<AssetDto>>> GetActive()
    {
        var assets = await _assetRepository.GetActiveAssetsAsync();
        var dtos = assets.Select(a => new AssetDto
        {
            Id = a.Id,
            AssetCode = a.AssetCode,
            Name = a.Name,
            Type = a.Type.ToString(),
            Status = a.Status.ToString(),
            CurrentLocation = a.CurrentLocation,
            CurrentLatitude = a.CurrentLatitude,
            CurrentLongitude = a.CurrentLongitude,
            LastTelemetryTime = a.LastTelemetryTime,
            IsActive = a.IsActive
        });
        
        return Ok(dtos);
    }
}

public class AssetDto
{
    public Guid Id { get; set; }
    public string AssetCode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string? CurrentLocation { get; set; }
    public double? CurrentLatitude { get; set; }
    public double? CurrentLongitude { get; set; }
    public DateTime? LastTelemetryTime { get; set; }
    public bool IsActive { get; set; }
}
