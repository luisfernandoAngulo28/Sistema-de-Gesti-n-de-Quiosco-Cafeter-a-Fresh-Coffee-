using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuioscoAPI.DTOs;
using QuioscoAPI.Services.Interfaces;

namespace QuioscoAPI.Controllers;

/// <summary>Gestión de pedidos del quiosco.</summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
[Tags("Pedidos")]
public class PedidosController : ControllerBase
{
    private readonly IPedidoServiceLFAH _service;

    public PedidosController(IPedidoServiceLFAH service)
    {
        _service = service;
    }

    /// <summary>Listar todos los pedidos pendientes (GET relacional — 5 tablas).</summary>
    /// <remarks>
    /// Retorna pedidos con estado=false (pendientes de pago).
    /// Cada pedido incluye datos relacionados de: pedidos + users + mesas + pagos + productos.
    /// Solo administradores deberían acceder a este endpoint en producción.
    /// </remarks>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAll()
    {
        var resultado = await _service.GetAllResponse();
        return Ok(new { data = resultado });
    }

    /// <summary>Obtener un pedido por ID con todos sus datos relacionados.</summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(PedidoResponseDtoLFAH), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetById(int id)
    {
        var pedido = await _service.GetByIdResponse(id);
        if (pedido is null) return NotFound(new { message = "Pedido no encontrado" });
        return Ok(pedido);
    }

    /// <summary>Listar el historial de pedidos del usuario autenticado.</summary>
    /// <remarks>Usa el claim `sub` del JWT para filtrar los pedidos del usuario actual.</remarks>
    [HttpGet("mios")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetMios()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var resultado = await _service.GetByUserId(userId);
        return Ok(new { data = resultado });
    }

    /// <summary>Crear un nuevo pedido.</summary>
    /// <remarks>
    /// El frontend envía: `{ total, mesa_id, productos: [{ id, cantidad }] }` en snake_case.
    /// El userId se extrae automáticamente del token JWT (claim `sub`).
    /// Retorna el ID del pedido creado para poder registrar el pago a continuación.
    /// </remarks>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Create([FromBody] CreatePedidoDtoLFAH dto)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var created = await _service.Create(dto, userId);
        return Ok(new { message = "Pedido realizado correctamente.", id = created.Id });
    }

    /// <summary>Marcar un pedido como completado. Solo administradores.</summary>
    /// <remarks>Requiere claim `admin = true`. Cambia `estado` a true sin necesidad de registrar un pago.</remarks>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id)
    {
        var esAdmin = User.FindFirst("admin")?.Value == "true";
        if (!esAdmin) return Forbid();

        var completado = await _service.CompleteOrder(id);
        if (!completado) return NotFound(new { message = "Pedido no encontrado" });
        return Ok(new { message = "Pedido completado." });
    }

    /// <summary>Eliminar un pedido.</summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var eliminado = await _service.Delete(id);
        if (!eliminado) return NotFound(new { message = "Pedido no encontrado" });
        return NoContent();
    }
}
