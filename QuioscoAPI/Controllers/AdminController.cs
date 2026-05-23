using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuioscoAPI.DTOs;
using QuioscoAPI.Services.Interfaces;

namespace QuioscoAPI.Controllers;

/// <summary>Panel de estadísticas y operaciones exclusivas del administrador.</summary>
[ApiController]
[Route("api/admin")]
[Authorize]
[Tags("Admin")]
public class AdminController : ControllerBase
{
    private readonly IPedidoServiceLFAH _service;

    public AdminController(IPedidoServiceLFAH service)
    {
        _service = service;
    }

    /// <summary>Obtener estadísticas del negocio. Solo administradores.</summary>
    /// <remarks>
    /// Requiere claim `admin = true` en el JWT.
    /// Retorna: total de pedidos, pedidos pendientes, pedidos completados e ingresos totales.
    /// </remarks>
    [HttpGet("estadisticas")]
    [ProducesResponseType(typeof(EstadisticasDtoLFAH), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetEstadisticas()
    {
        var esAdmin = User.FindFirst("admin")?.Value == "true";
        if (!esAdmin) return Forbid();

        var stats = await _service.GetEstadisticas();
        return Ok(stats);
    }
}
