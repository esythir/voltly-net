using MediatR;

namespace Voltly.Application.Features.Equipments.Commands.ActivateEquipment;

public sealed record ActivateEquipmentCommand(long Id, long OwnerId) : IRequest;