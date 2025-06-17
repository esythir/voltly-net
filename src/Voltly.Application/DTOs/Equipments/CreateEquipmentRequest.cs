using System.ComponentModel.DataAnnotations;

namespace Voltly.Application.DTOs.Equipments;

public sealed record CreateEquipmentRequest(
    [Required, StringLength(120, MinimumLength = 2)]  string Name,
    [StringLength(250)]                               string? Description,
    [Range(0.1, 99.9)]                                double DailyLimitKwh);