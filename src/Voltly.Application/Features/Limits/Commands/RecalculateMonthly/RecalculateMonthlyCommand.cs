using MediatR;

namespace Voltly.Application.Features.Limits.Commands.RecalculateMonthly;

public sealed record RecalculateMonthlyCommand(int? YearMonth) : IRequest;