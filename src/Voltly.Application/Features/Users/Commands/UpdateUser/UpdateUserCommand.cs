using MediatR;
using Voltly.Application.DTOs.Users;

namespace Voltly.Application.Features.Users.Commands.UpdateUser;

/// <summary> Updates only the user's own profile (fields Name, Email, Password, and BirthDate).</summary>
public sealed record UpdateUserCommand(long Id, UpdateProfileRequest Request)
    : IRequest<UserResponse>;
