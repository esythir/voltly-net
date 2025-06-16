using MediatR;

namespace Voltly.Application.Features.Alerts.Commands.GenerateAlerts;

public sealed record GenerateAlertsCommand : IRequest<int>;