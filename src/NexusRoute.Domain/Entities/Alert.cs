using NexusRoute.Domain.Enums;

namespace NexusRoute.Domain.Entities;

public class Alert
{
    public Guid Id { get; set; }
    public AlertType Type { get; set; }
    public AlertSeverity Severity { get; set; }
    
    public Guid? AssetId { get; set; }
    public Asset? Asset { get; set; }
    
    public Guid? ConvoyId { get; set; }
    public Convoy? Convoy { get; set; }
    
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string? Details { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? AcknowledgedAt { get; set; }
    public string? AcknowledgedBy { get; set; }
    
    public bool IsActive { get; set; } = true;
    public bool IsAcknowledged => AcknowledgedAt.HasValue;

    public void Acknowledge(string acknowledgedBy)
    {
        AcknowledgedAt = DateTime.UtcNow;
        AcknowledgedBy = acknowledgedBy;
        IsActive = false;
    }

    public static Alert CreateAssetHealthAlert(
        Asset asset,
        AlertType type,
        AlertSeverity severity,
        string message,
        string? details = null)
    {
        return new Alert
        {
            Id = Guid.NewGuid(),
            Type = type,
            Severity = severity,
            AssetId = asset.Id,
            Asset = asset,
            Title = $"{asset.AssetCode} - {type}",
            Message = message,
            Details = details,
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };
    }

    public static Alert CreateConvoyAlert(
        Convoy convoy,
        AlertType type,
        AlertSeverity severity,
        string message,
        string? details = null)
    {
        return new Alert
        {
            Id = Guid.NewGuid(),
            Type = type,
            Severity = severity,
            ConvoyId = convoy.Id,
            Convoy = convoy,
            AssetId = convoy.LeadAssetId,
            Title = $"{convoy.ConvoyCode} - {type}",
            Message = message,
            Details = details,
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };
    }
}
