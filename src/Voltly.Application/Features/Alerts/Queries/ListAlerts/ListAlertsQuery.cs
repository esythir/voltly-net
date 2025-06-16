using MediatR;
using Voltly.Application.DTOs;

namespace Voltly.Application.Features.Alerts.Queries.ListAlerts;

public record ListAlertsQuery(
    long? EquipmentId,
    int   Page  = 1,
    int   Size  = 20,
    string? Sort = null
) : IRequest<PagedResponse<AlertDto>>;
