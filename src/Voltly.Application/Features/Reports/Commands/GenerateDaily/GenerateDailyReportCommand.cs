using MediatR;
using Voltly.Application.DTOs;

namespace Voltly.Application.Features.Reports.Commands.GenerateDaily;

public sealed record GenerateDailyReportCommand(
    long     EquipmentId,
    DateOnly? ReportDate = null
) : IRequest<DailyReportDto>;