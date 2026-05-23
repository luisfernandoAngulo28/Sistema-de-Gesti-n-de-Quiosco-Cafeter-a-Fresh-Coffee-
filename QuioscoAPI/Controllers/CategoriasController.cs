using Microsoft.AspNetCore.Mvc;
using QuioscoAPI.DTOs;
using QuioscoAPI.Services.Interfaces;

namespace QuioscoAPI.Controllers;

/// <summary>Gestión de categorías del menú.</summary>
[ApiController]
[Route("api/[controller]")]
[Tags("Categorías")]
public class CategoriasController : ControllerBase
{
    private readonly ICategoriaServiceLFAH _service;

    public CategoriasController(ICategoriaServiceLFAH service)
    {
        _service = service;
    }

    /// <summary>Listar todas las categorías disponibles.</summary>
    /// <remarks>Retorna `{ data: [...] }`. No requiere autenticación.</remarks>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var categorias = await _service.GetAll();
        return Ok(new { data = categorias });
    }

    /// <summary>Obtener una categoría por ID.</summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(CategoriaDtoLFAH), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var categoria = await _service.GetById(id);
        if (categoria is null) return NotFound(new { message = "Categoría no encontrada" });
        return Ok(categoria);
    }

    /// <summary>Crear una nueva categoría.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(CategoriaDtoLFAH), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Create([FromBody] CreateCategoriaDtoLFAH dto)
    {
        var created = await _service.Create(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>Actualizar una categoría existente.</summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(CategoriaDtoLFAH), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCategoriaDtoLFAH dto)
    {
        var updated = await _service.Update(id, dto);
        if (updated is null) return NotFound(new { message = "Categoría no encontrada" });
        return Ok(updated);
    }

    /// <summary>Eliminar una categoría.</summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.Delete(id);
        if (!deleted) return NotFound(new { message = "Categoría no encontrada" });
        return NoContent();
    }
}
