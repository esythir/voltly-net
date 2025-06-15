namespace Voltly.Application.DTOs;

public record AutomaticActionDto(long Id, long EquipmentId, string Type, string Details, DateTime ExecutedAt);