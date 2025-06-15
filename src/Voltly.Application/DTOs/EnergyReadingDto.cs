namespace Voltly.Application.DTOs;

public record EnergyReadingDto(long Id, double PowerKw, double OccupancyPct, DateTime TakenAt, long SensorId);