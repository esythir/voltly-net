namespace Voltly.Application.DTOs.Equipments;

public sealed record EquipmentResponse(
    long    Id,
    long    OwnerId,
    string  Name,
    string? Description,
    double  DailyLimitKwh,
    bool    Active);