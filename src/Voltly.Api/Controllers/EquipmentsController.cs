using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Voltly.Api.Extensions;
using Voltly.Application.DTOs;
using Voltly.Application.DTOs.Equipments;
using Voltly.Application.Features.Equipments.Commands.ActivateEquipment;
using Voltly.Application.Features.Equipments.Commands.CreateEquipment;
using Voltly.Application.Features.Equipments.Commands.DeactivateEquipment;
using Voltly.Application.Features.Equipments.Commands.UpdateEquipment;
using Voltly.Application.Features.Equipments.Queries.ListEquipments;

[ApiController, Route("api/equipments"), Authorize(Roles="ADMIN,USER")]
public sealed class EquipmentsController : ControllerBase
{
    private readonly IMediator _med;
    public EquipmentsController(IMediator med) => _med = med;

    /// <summary>Register a new equipment for the logged-in user.</summary>
    [HttpPost]
    public Task<EquipmentResponse> Create(
        CreateEquipmentRequest req, CancellationToken ct) =>
        _med.Send(new CreateEquipmentCommand(User.GetUserId(), req), ct);

    /// <summary>List all equipments registered by the logged-in user.</summary>
    [HttpGet]
    public Task<PagedResponse<EquipmentResponse>> List(
        [FromQuery] int page = 1, [FromQuery] int size = 20, CancellationToken ct = default) =>
        _med.Send(new ListEquipmentsQuery(User.GetUserId(), page, size), ct);

    /// <summary>Get details of a specific equipment by its ID.</summary>
    [HttpPut("{id:long}")]
    public Task<EquipmentResponse> Update(
        long id, UpdateEquipmentRequest req, CancellationToken ct) =>
        _med.Send(new UpdateEquipmentCommand(id, User.GetUserId(), req), ct);

    /// <summary>Inactivate a specific equipment by its ID.</summary>
    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Deactivate(long id, CancellationToken ct)
    {
        await _med.Send(new DeactivateEquipmentCommand(id, User.GetUserId()), ct);
        return NoContent();
    }

    /// <summary>Activate a specific equipment by its ID.</summary>
    [HttpPost("{id:long}/activate")]
    public async Task<IActionResult> Activate(long id, CancellationToken ct)
    {
        await _med.Send(new ActivateEquipmentCommand(id, User.GetUserId()), ct);
        return NoContent();
    }
}
