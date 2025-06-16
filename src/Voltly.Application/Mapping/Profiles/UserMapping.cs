using Mapster;
using Voltly.Application.DTOs;
using Voltly.Domain.Entities;
using Voltly.Application.Features.Users.Commands.CreateUser;

namespace Voltly.Application.Mapping.Profiles;

public static class UserMapping
{
    public static void Register(TypeAdapterConfig cfg)
    {
        cfg.NewConfig<User, UserDto>();
        
        cfg.NewConfig<CreateUserCommand, User>()
            .IgnoreNonMapped(true)
            .Map(dest => dest.Id, _ => 0);
    }
}