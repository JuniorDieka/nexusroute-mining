using FluentValidation;
using NexusRoute.Application.DTOs;

namespace NexusRoute.Application.Validators;

public class OreMovementValidator : AbstractValidator<OreMovementDto>
{
    public OreMovementValidator()
    {
        RuleFor(x => x.AssetId)
            .NotEmpty()
            .WithMessage("AssetId is required");

        RuleFor(x => x.EventType)
            .NotEmpty()
            .WithMessage("EventType is required")
            .Must(BeValidEventType)
            .WithMessage("EventType must be one of: Load, Haul, Dump, Queue");

        RuleFor(x => x.TonnageEstimate)
            .GreaterThan(0)
            .LessThanOrEqualTo(500)
            .WithMessage("Tonnage estimate must be between 0 and 500 tonnes");

        RuleFor(x => x.GradeEstimate)
            .GreaterThanOrEqualTo(0)
            .LessThanOrEqualTo(100)
            .When(x => x.GradeEstimate.HasValue)
            .WithMessage("Grade estimate must be between 0 and 100 g/t");

        RuleFor(x => x.SourceLocation)
            .NotEmpty()
            .WithMessage("Source location is required");

        RuleFor(x => x.SourceLatitude)
            .GreaterThanOrEqualTo(-90)
            .LessThanOrEqualTo(90)
            .WithMessage("Source latitude must be between -90 and 90 degrees");

        RuleFor(x => x.SourceLongitude)
            .GreaterThanOrEqualTo(-180)
            .LessThanOrEqualTo(180)
            .WithMessage("Source longitude must be between -180 and 180 degrees");
    }

    private static bool BeValidEventType(string eventType)
    {
        var validTypes = new[] { "Load", "Haul", "Dump", "Queue" };
        return validTypes.Contains(eventType, StringComparer.OrdinalIgnoreCase);
    }
}
