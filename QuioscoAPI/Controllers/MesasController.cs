using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuioscoAPI.DTOs;
using QuioscoAPI.Services.Interfaces;

namespace QuioscoAPI.Controllers;

/// <summary>Gestión de mesas del local.</summary>
[ApiController]
[Route("api/[controller]")]
[Tags("Mesas")]
public class MesasController : ControllerBase
{
    private readonly IMesaServiceLFAH _service;

    public MesasController(IMesaServiceLFAH service)
    {
        _service = service;
    }

    /// <summary>Listar todas las mesas con su estado de ocupación.</summary>
    /// <remarks>El campo `ocupada` es calculado: true si la mesa tiene pedidos con estado=false (pendientes).</remarks>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var mesas = await _service.GetAll();
        return Ok(new { data = mesas });
    }

    /// <summary>Obtener una mesa por ID.</summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(MesaDtoLFAH), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var mesa = await _service.GetById(id);
        if (mesa is null) return NotFound(new { message = "Mesa no encontrada" });
        return Ok(mesa);
    }

    /// <summary>Crear una nueva mesa.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(MesaDtoLFAH), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Create([FromBody] CreateMesaDtoLFAH dto)
    {
        var created = await _service.Create(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>Actualizar una mesa. Solo administradores.</summary>
    /// <remarks>Requiere claim `admin = true` en el JWT. Permite cambiar número, capacidad y disponibilidad.</remarks>
    [Authorize]
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(MesaDtoLFAH), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateMesaDtoLFAH dto)
    {
        var esAdmin = User.FindFirst("admin")?.Value == "true";
        if (!esAdmin) return Forbid();

        var updated = await _service.Update(id, dto);
        if (updated is null) return NotFound(new { message = "Mesa no encontrada" });
        return Ok(updated);
    }

    /// <summary>Eliminar una mesa. Solo administradores.</summary>
    /// <remarks>Requiere claim `admin = true` en el JWT.</remarks>
    [Authorize]
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var esAdmin = User.FindFirst("admin")?.Value == "true";
        if (!esAdmin) return Forbid();

        var deleted = await _service.Delete(id);
        if (!deleted) return NotFound(new { message = "Mesa no encontrada" });
        return NoContent();
    }
}
