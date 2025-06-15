namespace Voltly.Application.DTOs;

public record AlertDto(long Id, long EquipmentId, DateOnly AlertDate, double ConsumptionKwh, double LimitKwh, double ExceededByKwh, string? Message);