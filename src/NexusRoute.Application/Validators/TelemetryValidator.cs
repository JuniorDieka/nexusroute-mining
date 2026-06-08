using FluentValidation;
using NexusRoute.Application.DTOs;

namespace NexusRoute.Application.Validators;

public class TelemetryValidator : AbstractValidator<TelemetryDto>
{
    public TelemetryValidator()
    {
        RuleFor(x => x.AssetId)
            .NotEmpty()
            .WithMessage("AssetId is required");

        RuleFor(x => x.EngineTemperatureCelsius)
            .GreaterThanOrEqualTo(-50)
            .LessThanOrEqualTo(200)
            .WithMessage("Engine temperature must be between -50°C and 200°C");

        RuleFor(x => x.PayloadWeightTonnes)
            .GreaterThanOrEqualTo(0)
            .LessThanOrEqualTo(500)
            .WithMessage("Payload weight must be between 0 and 500 tonnes");

        RuleFor(x => x.TirePressurePsi)
            .GreaterThanOrEqualTo(0)
            .LessThanOrEqualTo(200)
            .WithMessage("Tire pressure must be between 0 and 200 PSI");

        RuleFor(x => x.FuelLevelPercentage)
            .GreaterThanOrEqualTo(0)
            .LessThanOrEqualTo(100)
            .WithMessage("Fuel level must be between 0% and 100%");

        RuleFor(x => x.Latitude)
            .GreaterThanOrEqualTo(-90)
            .LessThanOrEqualTo(90)
            .WithMessage("Latitude must be between -90 and 90 degrees");

        RuleFor(x => x.Longitude)
            .GreaterThanOrEqualTo(-180)
            .LessThanOrEqualTo(180)
            .WithMessage("Longitude must be between -180 and 180 degrees");
    }
}
