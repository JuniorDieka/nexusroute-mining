using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NexusRoute.Application.Interfaces;

namespace NexusRoute.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AlertsController : ControllerBase
{
    private readonly IAlertRepository _alertRepository;
    private readonly ILogger<AlertsController> _logger;

    public AlertsController(IAlertRepository alertRepository, ILogger<AlertsController> logger)
    {
        _alertRepository = alertRepository ?? throw new ArgumentNullException(nameof(alertRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet("active")]
    public async Task<IActionResult> GetActive()
    {
        var alerts = await _alertRepository.GetActiveAlertsAsync();
        return Ok(alerts);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var alert = await _alertRepository.GetByIdAsync(id);
        if (alert == null)
            return NotFound();
        
        return Ok(alert);
    }
}
