using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Voltly.Api.Controllers;
using Voltly.Application.DTOs;
using Voltly.Application.Features.IdleActions.CheckIdle;
using Xunit;

namespace Voltly.Tests.Unit.Controllers;

public class IdleActionsControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly IdleActionsController _controller;

    public IdleActionsControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new IdleActionsController(_mediatorMock.Object);
    }

    [Fact]
    public async Task CheckIdle_WhenCalled_ShouldReturn200OK()
    {
        // Arrange
        var query = new CheckIdleQuery(1, 30); // 30 minutes window
        var expectedResponse = new List<AutomaticActionDto>
        {
            new(1, 1, "SHUTDOWN", "Device shutdown due to inactivity", DateTime.UtcNow)
        };

        _mediatorMock.Setup(m => m.Send(query, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var actionResult = await _controller.CheckIdle(query, CancellationToken.None);
        var okResult = actionResult.Should().BeOfType<OkObjectResult>().Subject;

        // Assert
        okResult.StatusCode.Should().Be(200);
        okResult.Value.Should().BeEquivalentTo(expectedResponse);
    }
} 