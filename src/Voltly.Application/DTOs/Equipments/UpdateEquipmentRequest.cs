namespace Voltly.Application.DTOs.Equipments;

using System.ComponentModel.DataAnnotations;

public sealed record UpdateEquipmentRequest(
    [Required, StringLength(120, MinimumLength = 2)]
    string  Name,
    
    [StringLength(250)]
    string? Description,
    
    [Required, Range(0.1, 9999)]
    double  DailyLimitKwh);