using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Voltly.Api.Controllers;
using Voltly.Application.DTOs;
using Voltly.Application.Features.Reports.Queries.GetDailyHistory;
using Xunit;

namespace Voltly.Tests.Unit.Controllers;

public class ReportsControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly ReportsController _controller;

    public ReportsControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new ReportsController(_mediatorMock.Object);
    }

    [Fact]
    public async Task History_WhenCalled_ShouldReturn200OK()
    {
        // Arrange
        var query = new GetDailyHistoryQuery(1);
        var expectedResponse = new PagedResponse<DailyReportDto>(
            new List<DailyReportDto>
            {
                new(
                    1,
                    1,
                    DateOnly.FromDateTime(DateTime.UtcNow),
                    100.5,
                    50.2,
                    "GOOD"
                )
            },
            1, 1, 10);

        _mediatorMock.Setup(m => m.Send(query, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var actionResult = await _controller.History(query, CancellationToken.None);
        var okResult = actionResult.Should().BeOfType<OkObjectResult>().Subject;

        // Assert
        okResult.StatusCode.Should().Be(200);
        okResult.Value.Should().BeEquivalentTo(expectedResponse);
    }
} 