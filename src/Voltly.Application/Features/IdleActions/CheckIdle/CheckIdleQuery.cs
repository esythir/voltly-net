using MediatR;
using Voltly.Application.DTOs;

namespace Voltly.Application.Features.IdleActions.CheckIdle;

public sealed record CheckIdleQuery(
    long? EquipmentId = null,
    int   MinutesWindow = 15
) : IRequest<IEnumerable<AutomaticActionDto>>;