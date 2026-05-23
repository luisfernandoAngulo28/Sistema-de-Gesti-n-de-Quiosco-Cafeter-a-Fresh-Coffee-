using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuioscoAPI.DTOs;
using QuioscoAPI.Services.Interfaces;

namespace QuioscoAPI.Controllers;

/// <summary>Registro y consulta de pagos de pedidos.</summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
[Tags("Pagos")]
public class PagosController : ControllerBase
{
    private readonly IPagoServiceLFAH _service;

    public PagosController(IPagoServiceLFAH service)
    {
        _service = service;
    }

    /// <summary>Listar todos los pagos registrados.</summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAll()
    {
        var pagos = await _service.GetAll();
        return Ok(new { data = pagos });
    }

    /// <summary>Obtener un pago por ID.</summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(PagoDtoLFAH), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var pago = await _service.GetById(id);
        if (pago is null) return NotFound(new { message = "Pago no encontrado" });
        return Ok(pago);
    }

    /// <summary>Obtener el pago asociado a un pedido específico (GET relacional).</summary>
    /// <remarks>Consulta el pago vinculado al `pedido_id` indicado en la ruta.</remarks>
    [HttpGet("pedido/{pedidoId}")]
    [ProducesResponseType(typeof(PagoDtoLFAH), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByPedido(int pedidoId)
    {
        var pago = await _service.GetByPedidoId(pedidoId);
        if (pago is null) return NotFound(new { message = "Este pedido aún no tiene pago registrado" });
        return Ok(pago);
    }

    /// <summary>Registrar el pago de un pedido.</summary>
    /// <remarks>
    /// Al crear el pago, automáticamente se marca el pedido asociado como `estado = true` (completado).
    /// Métodos aceptados: `efectivo`, `tarjeta`, `transferencia`.
    /// El campo `pedido_id` debe corresponder a un pedido existente con `estado = false`.
    /// </remarks>
    [HttpPost]
    [ProducesResponseType(typeof(PagoDtoLFAH), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Create([FromBody] CreatePagoDtoLFAH dto)
    {
        var created = await _service.Create(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>Eliminar un pago.</summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.Delete(id);
        if (!deleted) return NotFound(new { message = "Pago no encontrado" });
        return NoContent();
    }
}
