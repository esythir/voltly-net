using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
using Voltly.Api.Controllers;
using Voltly.Application.DTOs;
using Voltly.Application.DTOs.Users;
using Voltly.Application.Features.Users.Commands.CreateUser;
using Voltly.Application.Features.Users.Commands.DeactivateUser;
using Voltly.Application.Features.Users.Commands.DeleteUser;
using Voltly.Application.Features.Users.Commands.UpdateUser;
using Voltly.Application.Features.Users.Commands.UpdateUserAdmin;
using Voltly.Application.Features.Users.Queries.GetUserById;
using Voltly.Application.Features.Users.Queries.ListUsers;
using Voltly.Application.Features.Users.Queries.SearchUsers;
using Voltly.Domain.Enums;
using Xunit;

namespace Voltly.Tests.Unit.Controllers;

public class UsersControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly UsersController _controller;

    public UsersControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new UsersController(_mediatorMock.Object);
    }

    [Fact]
    public async Task Create_WhenValidRequest_ShouldReturnCreatedResult()
    {
        // Arrange
        var birthDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-30));
        var request = new AdminCreateUserRequest(
            "Test User",
            "test@example.com",
            "password123",
            birthDate,
            UserRole.User.ToString()
        );

        var expectedResponse = new UserDto(
            1,
            request.Name,
            request.Email,
            request.Role,
            true
        );

        _mediatorMock.Setup(m => m.Send(
            It.Is<CreateUserCommand>(cmd => 
                cmd.Name == request.Name && 
                cmd.Email == request.Email && 
                cmd.Password == request.Password &&
                cmd.BirthDate == request.BirthDate &&
                cmd.Role == request.Role),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.Create(request, CancellationToken.None);

        // Assert
        var createdResult = result.Should().BeOfType<CreatedAtActionResult>().Subject;
        createdResult.ActionName.Should().Be(nameof(UsersController.GetById));
        createdResult.RouteValues.Should().ContainKey("id").WhoseValue.Should().Be(expectedResponse.Id);
        createdResult.Value.Should().BeEquivalentTo(expectedResponse);
    }

    [Fact]
    public async Task UpdateOwn_WhenValidRequest_ShouldReturnUpdatedUser()
    {
        // Arrange
        var userId = 1L;
        var birthDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-25));
        var request = new UpdateProfileRequest(
            "Updated Name",
            "updated@example.com",
            "newpassword123",
            birthDate
        );

        var expectedResponse = new UserResponse(
            userId,
            request.Name,
            request.Email,
            request.BirthDate!.Value,
            true,
            UserRole.User.ToString(),
            DateTime.UtcNow,
            null
        );

        _mediatorMock.Setup(m => m.Send(
            It.Is<UpdateUserCommand>(cmd => 
                cmd.Id == userId && 
                cmd.Request == request),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        SetupUserContext(userId, "USER");

        // Act
        var result = await _controller.UpdateOwn(request, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expectedResponse);
    }

    [Fact]
    public async Task UpdateAdmin_WhenValidRequest_ShouldReturnUpdatedUser()
    {
        // Arrange
        var userId = 1L;
        var birthDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-25));
        var request = new UpdateUserAdminRequest(
            "Updated By Admin",
            "updated@example.com",
            "newpassword123",
            birthDate,
            "ADMIN",
            true
        );

        var expectedResponse = new UserResponse(
            userId,
            request.Name,
            request.Email,
            request.BirthDate!.Value,
            request.IsActive,
            request.Role,
            DateTime.UtcNow,
            null
        );

        _mediatorMock.Setup(m => m.Send(
            It.Is<UpdateUserAdminCommand>(cmd => 
                cmd.Id == userId && 
                cmd.Request == request),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.Update(userId, request, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expectedResponse);
    }

    [Fact]
    public async Task DeactivateOwn_WhenCalled_ShouldReturnNoContent()
    {
        // Arrange
        var userId = 1L;
        SetupUserContext(userId, "USER");

        _mediatorMock.Setup(m => m.Send(
            It.Is<DeactivateUserCommand>(cmd => cmd.UserId == userId),
            It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeactivateOwn(CancellationToken.None);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task Delete_WhenCalled_ShouldReturnNoContent()
    {
        // Arrange
        var userId = 1L;

        _mediatorMock.Setup(m => m.Send(
            It.Is<DeleteUserCommand>(cmd => cmd.Id == userId),
            It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Delete(userId, CancellationToken.None);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task GetById_WhenUserIsAdmin_ShouldReturnUser()
    {
        // Arrange
        var userId = 1L;
        var birthDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-30));
        var expectedResponse = new UserResponse(
            userId,
            "Test User",
            "test@example.com",
            birthDate,
            true,
            UserRole.User.ToString(),
            DateTime.UtcNow,
            null
        );

        _mediatorMock.Setup(m => m.Send(
            It.Is<GetUserByIdQuery>(q => q.Id == userId),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        SetupUserContext(2L, "ADMIN"); // Different user ID, but admin role

        // Act
        var result = await _controller.GetById(userId, CancellationToken.None);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeEquivalentTo(expectedResponse);
    }

    [Fact]
    public async Task GetById_WhenUserIsNotAdminAndNotOwner_ShouldReturnForbid()
    {
        // Arrange
        var userId = 1L;
        SetupUserContext(2L, "USER"); // Different user ID, non-admin role

        // Act
        var result = await _controller.GetById(userId, CancellationToken.None);

        // Assert
        result.Result.Should().BeOfType<ForbidResult>();
    }

    [Fact]
    public async Task List_WhenCalled_ShouldReturnPagedResponse()
    {
        // Arrange
        var query = new ListUsersQuery(1, 10);
        var birthDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-30));
        var expectedResponse = new PagedResponse<UserResponse>(
            new List<UserResponse>
            {
                new(
                    1,
                    "Test User",
                    "test@example.com",
                    birthDate,
                    true,
                    UserRole.User.ToString(),
                    DateTime.UtcNow,
                    null
                )
            },
            1, 1, 10);

        _mediatorMock.Setup(m => m.Send(query, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.List(query, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expectedResponse);
    }

    [Fact]
    public async Task Search_WhenCalled_ShouldReturnMatchingUsers()
    {
        // Arrange
        var searchTerm = "test";
        var birthDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-30));
        var expectedResponse = new List<UserResponse>
        {
            new(
                1,
                "Test User",
                "test@example.com",
                birthDate,
                true,
                UserRole.User.ToString(),
                DateTime.UtcNow,
                null
            )
        };

        _mediatorMock.Setup(m => m.Send(
            It.Is<SearchUsersQuery>(q => q.NamePart == searchTerm),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.Search(searchTerm, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expectedResponse);
    }

    private void SetupUserContext(long userId, string role)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userId.ToString()),
            new(ClaimTypes.Role, role)
        };
        var identity = new ClaimsIdentity(claims);
        var principal = new ClaimsPrincipal(identity);
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = principal }
        };
    }
} 