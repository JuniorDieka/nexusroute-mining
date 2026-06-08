using NexusRoute.Domain.Enums;

namespace NexusRoute.Application.DTOs;

public record AlertDto
{
    public Guid Id { get; init; }
    public AlertType Type { get; init; }
    public AlertSeverity Severity { get; init; }
    public Guid? AssetId { get; init; }
    public string? AssetCode { get; init; }
    public Guid? ConvoyId { get; init; }
    public string? ConvoyCode { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Message { get; init; } = string.Empty;
    public string? Details { get; init; }
    public DateTime CreatedAt { get; init; }
    public bool IsActive { get; init; }
    public bool IsAcknowledged { get; init; }
}
