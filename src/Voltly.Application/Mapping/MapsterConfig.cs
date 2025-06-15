using Mapster;
using Voltly.Domain.Entities;
using Voltly.Application.DTOs;

namespace Voltly.Application.Mapping;

public static class MapsterConfig
{
    public static void Configure(TypeAdapterConfig cfg)
    {
        // Entidade → DTO
        cfg.NewConfig<Equipment,       EquipmentDto>();
        cfg.NewConfig<Sensor,          SensorDto>();
        cfg.NewConfig<EnergyReading,   EnergyReadingDto>();
        cfg.NewConfig<ConsumptionLimit, ConsumptionLimitDto>();
        cfg.NewConfig<AutomaticAction, AutomaticActionDto>();
        cfg.NewConfig<Alert,            AlertDto>();
        cfg.NewConfig<DailyReport,      DailyReportDto>();
        cfg.NewConfig<User,             UserDto>()
            .IgnoreNullValues(true);

        // (Opcional) DTO → Entidade para Commands de criação/atualização
        // ex:
        // cfg.NewConfig<CreateEquipmentDto, Equipment>()
        //    .Map(dest => dest.Id, src => 0)
        //    .IgnoreNonMapped(true);
    }
}