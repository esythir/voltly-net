using Mapster;
using Voltly.Application.DTOs.Users;
using Voltly.Domain.Entities;

namespace Voltly.Application.Mapping.Profiles;

public static class UserMapping
{
    public static void Register(TypeAdapterConfig cfg)
    {
        cfg.NewConfig<User, UserResponse>();

        cfg.NewConfig<RegisterUserRequest, User>()
            .Ignore(u => u.Id)
            .Ignore(u => u.CreatedAt)
            .Ignore(u => u.UpdatedAt)
            .Ignore(u => u.Role)
            .Map(u => u.IsActive, _ => true);

        cfg.NewConfig<UpdateProfileRequest, User>()
            .IgnoreNullValues(true);

        cfg.NewConfig<UpdateUserAdminRequest, User>()
            .IgnoreNullValues(true);
    }
}