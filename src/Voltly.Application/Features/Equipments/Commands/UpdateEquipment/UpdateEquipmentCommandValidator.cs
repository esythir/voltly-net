using FluentValidation;

namespace Voltly.Application.Features.Equipments.Commands.UpdateEquipment;

public sealed class UpdateEquipmentCommandValidator
    : AbstractValidator<UpdateEquipmentCommand>
{
    public UpdateEquipmentCommandValidator()
    {
        RuleFor(c => c.Id)      .GreaterThan(0);
        RuleFor(c => c.OwnerId) .GreaterThan(0);
        RuleFor(c => c.Request) .NotNull();
    }
}