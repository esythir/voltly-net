using Mapster;
using Voltly.Domain.Entities;
using Voltly.Application.DTOs;

namespace Voltly.Application.Mapping;

public static class MapsterConfig
{
    public static void Configure(TypeAdapterConfig cfg)
    {
        cfg.NewConfig<Equipment, EquipmentDto>()
            .Map(dest => dest.OwnerId, src => src.OwnerId);

        cfg.NewConfig<User, UserDto>()
            .IgnoreNullValues(true);
    }
}