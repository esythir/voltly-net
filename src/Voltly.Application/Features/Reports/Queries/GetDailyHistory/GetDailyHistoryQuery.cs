using MediatR;
using Voltly.Application.DTOs;

namespace Voltly.Application.Features.Reports.Queries.GetDailyHistory;

public sealed record GetDailyHistoryQuery(
    long? EquipmentId = null,
    DateOnly? From    = null,
    DateOnly? To      = null,
    int Page = 1,
    int Size = 20
) : IRequest<PagedResponse<DailyReportDto>>;