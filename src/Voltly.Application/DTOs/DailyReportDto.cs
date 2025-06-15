namespace Voltly.Application.DTOs;

public record DailyReportDto(long Id, long EquipmentId, DateOnly ReportDate, double ConsumptionKwh, double Co2EmissionKg, string EfficiencyRating);