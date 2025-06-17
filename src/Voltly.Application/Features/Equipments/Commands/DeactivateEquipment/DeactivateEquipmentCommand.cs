using MediatR;

namespace Voltly.Application.Features.Equipments.Commands.DeactivateEquipment;

public sealed record DeactivateEquipmentCommand(long Id, long OwnerId) : IRequest;