using MediatR;
using Voltly.Application.DTOs.Equipments;

namespace Voltly.Application.Features.Equipments.Commands.CreateEquipment;

public sealed record CreateEquipmentCommand(
    long OwnerId,
    CreateEquipmentRequest Request) : IRequest<EquipmentResponse>;