namespace Voltly.Application.DTOs;

public record ConsumptionLimitDto(long Id, long EquipmentId, double LimitKwh, DateOnly ComputedAt);