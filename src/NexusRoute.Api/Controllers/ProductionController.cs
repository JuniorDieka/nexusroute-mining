using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NexusRoute.Api.Authentication;
using NexusRoute.Infrastructure.Data;

namespace NexusRoute.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = $"{RoleConstants.Dispatcher},{RoleConstants.Maintenance}")]
public class ProductionController : ControllerBase
{
    private readonly NexusRouteDbContext _context;
    private readonly ILogger<ProductionController> _logger;

    public ProductionController(NexusRouteDbContext context, ILogger<ProductionController> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary()
    {
        var today = DateTime.UtcNow.Date;
        var dailyQuota = await _context.ProductionQuotas
            .Where(q => q.PeriodName == "Daily" && q.StartDate <= today && q.EndDate > today)
            .FirstOrDefaultAsync();

        var weeklyQuota = await _context.ProductionQuotas
            .Where(q => q.PeriodName == "Weekly")
            .FirstOrDefaultAsync();

        var totalTonnage = dailyQuota?.ActualTonnage ?? 0;
        var averageGrade = dailyQuota?.ActualGrade ?? 0;
        var achievementPercentage = weeklyQuota?.TonnageAchievementPercentage ?? 0;

        return Ok(new
        {
            totalTonnage,
            averageGrade,
            achievementPercentage
        });
    }

    [HttpGet("quotas")]
    public async Task<IActionResult> GetQuotas()
    {
        var quotas = await _context.ProductionQuotas.ToListAsync();
        return Ok(quotas);
    }
}
