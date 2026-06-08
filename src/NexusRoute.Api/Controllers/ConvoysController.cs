using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NexusRoute.Api.Authentication;
using NexusRoute.Application.Interfaces;

namespace NexusRoute.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = RoleConstants.Dispatcher)]
public class ConvoysController : ControllerBase
{
    private readonly IConvoyRepository _convoyRepository;
    private readonly ILogger<ConvoysController> _logger;

    public ConvoysController(IConvoyRepository convoyRepository, ILogger<ConvoysController> logger)
    {
        _convoyRepository = convoyRepository ?? throw new ArgumentNullException(nameof(convoyRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet("active")]
    public async Task<IActionResult> GetActive()
    {
        var convoys = await _convoyRepository.GetActiveConvoysAsync();
        return Ok(convoys);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var convoy = await _convoyRepository.GetByIdAsync(id);
        if (convoy == null)
            return NotFound();
        
        return Ok(convoy);
    }
}
