using FluentValidation;

namespace Voltly.Application.Features.Reports.Commands.GenerateDaily;

public sealed class GenerateDailyReportValidator : AbstractValidator<GenerateDailyReportCommand>
{
    public GenerateDailyReportValidator()
    {
        RuleFor(x => x.EquipmentId).GreaterThan(0);
    }
}