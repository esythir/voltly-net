using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Voltly.Api.Controllers;
using Voltly.Application.DTOs;
using Voltly.Application.Features.Alerts.Queries.ListAlerts;
using Xunit;

namespace Voltly.Tests.Unit.Controllers;

public class AlertsControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly AlertsController _controller;

    public AlertsControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new AlertsController(_mediatorMock.Object);
    }

    [Fact]
    public async Task List_WhenCalled_ShouldReturn200OK()
    {
        // Arrange
        var query = new ListAlertsQuery(1, 10);
        var expectedResponse = new PagedResponse<AlertDto>(
            new List<AlertDto>
            {
                new(1, 1, DateOnly.FromDateTime(DateTime.UtcNow), 100.5, 50.2, 50.3, "Test Alert")
            },
            1, 1, 10);

        _mediatorMock.Setup(m => m.Send(query, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var actionResult = await _controller.List(query, CancellationToken.None);
        var okResult = actionResult.Should().BeOfType<OkObjectResult>().Subject;

        // Assert
        okResult.StatusCode.Should().Be(200);
        okResult.Value.Should().BeEquivalentTo(expectedResponse);
    }
} 