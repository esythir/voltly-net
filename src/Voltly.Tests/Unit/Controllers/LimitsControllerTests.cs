using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Voltly.Api.Controllers;
using Voltly.Application.Features.Limits.Commands.RecalculateMonthly;
using Xunit;

namespace Voltly.Tests.Unit.Controllers;

public class LimitsControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly LimitsController _controller;

    public LimitsControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new LimitsController(_mediatorMock.Object);
    }

    [Fact]
    public async Task RecalculateCurrent_WhenCalled_ShouldReturnNoContent()
    {
        // Arrange
        _mediatorMock.Setup(m => m.Send(
            It.Is<RecalculateMonthlyCommand>(cmd => cmd.YearMonth == null),
            It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.RecalculateCurrent(CancellationToken.None);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }
} 