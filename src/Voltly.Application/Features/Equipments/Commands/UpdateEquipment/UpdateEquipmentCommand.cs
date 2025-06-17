using MediatR;
using Voltly.Application.DTOs.Equipments;

namespace Voltly.Application.Features.Equipments.Commands.UpdateEquipment;

public sealed record UpdateEquipmentCommand(
    long                   Id,       
    long                   OwnerId,  
    UpdateEquipmentRequest Request  
) : IRequest<EquipmentResponse>;