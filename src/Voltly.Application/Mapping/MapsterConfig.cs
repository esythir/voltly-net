using Mapster;
using Voltly.Domain.Entities;
using Voltly.Application.DTOs;
using Voltly.Application.Mapping.Profiles;
using Voltly.Application.DTOs.Equipments;

namespace Voltly.Application.Mapping;

public static class MapsterConfig
{
    public static void Configure(TypeAdapterConfig cfg)
    {
        UserMapping.Register(cfg);
        //EquipmentMapping.Register(cfg);
        //SensorMapping.Register(cfg);
        
        // Entidade → DTO
        cfg.NewConfig<Equipment,        EquipmentDto>();
        cfg.NewConfig<Sensor,           SensorDto>();
        cfg.NewConfig<EnergyReading,    EnergyReadingDto>();
        cfg.NewConfig<ConsumptionLimit, ConsumptionLimitDto>();
        cfg.NewConfig<AutomaticAction,  AutomaticActionDto>();
        cfg.NewConfig<Alert,            AlertDto>();
        cfg.NewConfig<DailyReport,      DailyReportDto>();
        cfg.NewConfig<User,             UserDto>()
            .IgnoreNullValues(true);
        
        cfg.NewConfig<Equipment,               EquipmentResponse>();
        cfg.NewConfig<CreateEquipmentRequest,  Equipment>()
            .Ignore(e => e.Id)
            .Ignore(e => e.Sensors)
            .Ignore(e => e.Owner)
            .Map  (e => e.Active,     _ => true);
        cfg.NewConfig<UpdateEquipmentRequest,  Equipment>()
            .IgnoreNullValues(true);

    }
}