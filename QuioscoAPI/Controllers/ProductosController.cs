using Microsoft.AspNetCore.Mvc;
using QuioscoAPI.Data;
using QuioscoAPI.DTOs;
using QuioscoAPI.Services.Interfaces;

namespace QuioscoAPI.Controllers;

/// <summary>Gestión del catálogo de productos del quiosco.</summary>
[ApiController]
[Route("api/[controller]")]
[Tags("Productos")]
public class ProductosController : ControllerBase
{
    private readonly IProductoServiceLFAH _service;
    private readonly QuioscoDbContext _context;

    public ProductosController(IProductoServiceLFAH service, QuioscoDbContext context)
    {
        _service = service;
        _context = context;
    }

    /// <summary>Listar todos los productos del menú.</summary>
    /// <remarks>Retorna `{ data: [...] }` con `categoria_id` en cada producto.</remarks>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var productos = await _service.GetAll();
        return Ok(new { data = productos });
    }

    /// <summary>Listar productos filtrados por categoría.</summary>
    [HttpGet("categoria/{categoriaId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByCategoria(int categoriaId)
    {
        var productos = await _service.GetByCategoria(categoriaId);
        return Ok(new { data = productos });
    }

    /// <summary>Obtener un producto por ID.</summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ProductoDtoLFAH), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var producto = await _service.GetById(id);
        if (producto is null) return NotFound(new { message = "Producto no encontrado" });
        return Ok(producto);
    }

    /// <summary>Crear un nuevo producto.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(ProductoDtoLFAH), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Create([FromBody] CreateProductoDtoLFAH dto)
    {
        var created = await _service.Create(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>Actualizar un producto o marcarlo como no disponible.</summary>
    /// <remarks>
    /// Si se envía `null` como body, el producto se marca como `disponible = false`.
    /// Si se envía el DTO completo, se actualizan todos los campos.
    /// </remarks>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ProductoDtoLFAH), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateProductoDtoLFAH? dto)
    {
        if (dto is null)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto is null) return NotFound(new { message = "Producto no encontrado" });
            producto.Disponible = false;
            producto.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return Ok(new { message = "Producto marcado como no disponible" });
        }

        var updated = await _service.Update(id, dto);
        if (updated is null) return NotFound(new { message = "Producto no encontrado" });
        return Ok(updated);
    }

    /// <summary>Eliminar un producto.</summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.Delete(id);
        if (!deleted) return NotFound(new { message = "Producto no encontrado" });
        return NoContent();
    }
}
