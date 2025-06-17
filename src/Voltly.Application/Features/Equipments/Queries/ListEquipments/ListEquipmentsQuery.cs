using MediatR;
using Voltly.Application.DTOs;
using Voltly.Application.DTOs.Equipments;

namespace Voltly.Application.Features.Equipments.Queries.ListEquipments;

public sealed record ListEquipmentsQuery(
    long OwnerId,
    int  Page = 1,
    int  Size = 20)
    : IRequest<PagedResponse<EquipmentResponse>>;
