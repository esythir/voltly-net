using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Voltly.Api.Controllers;
using Voltly.Application.DTOs.Auth;
using Voltly.Application.DTOs.Users;
using Voltly.Application.Features.Auth.Login;
using Xunit;

namespace Voltly.Tests.Unit.Controllers;

public class AuthControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly AuthController _controller;

    public AuthControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new AuthController(_mediatorMock.Object);
    }

    [Fact]
    public async Task Login_WhenValidCredentials_ShouldReturn200OK()
    {
        // Arrange
        var request = new LoginRequest("test@example.com", "password123");
        var expectedResponse = new AuthResponse(
            "jwt-token",
            DateTime.UtcNow.AddHours(1),
            new UserResponse(
                1,
                "Test User",
                request.Email,
                DateOnly.FromDateTime(DateTime.Now.AddYears(-30)),
                true,
                "USER",
                DateTime.UtcNow,
                null
            )
        );

        _mediatorMock.Setup(m => m.Send(
            It.Is<LoginCommand>(cmd => cmd.Request == request),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var actionResult = await _controller.Login(request, CancellationToken.None);
        var okResult = actionResult.Should().BeOfType<OkObjectResult>().Subject;

        // Assert
        okResult.StatusCode.Should().Be(200);
        okResult.Value.Should().BeEquivalentTo(expectedResponse);
    }
} 