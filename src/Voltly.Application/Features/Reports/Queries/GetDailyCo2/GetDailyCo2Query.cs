using MediatR;
using Voltly.Application.DTOs;

namespace Voltly.Application.Features.Reports.Queries.GetDailyCo2;

public sealed record GetDailyCo2Query(
    long? EquipmentId = null,
    DateOnly? From    = null,
    DateOnly? To      = null
) : IRequest<IEnumerable<DailyReportDto>>;