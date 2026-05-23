using Microsoft.EntityFrameworkCore;
using QuioscoAPI.Data;
using QuioscoAPI.Models;
using QuioscoAPI.Repositories.Interfaces;

namespace QuioscoAPI.Repositories;

public class ProductoRepositoryLFAH : IProductoRepositoryLFAH
{
    private readonly QuioscoDbContext _context;

    public ProductoRepositoryLFAH(QuioscoDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Producto>> GetAll()
    {
        return await _context.Productos
            .Include(p => p.Categoria)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Producto>> GetByCategoria(int categoriaId)
    {
        return await _context.Productos
            .Include(p => p.Categoria)
            .Where(p => p.CategoriaId == categoriaId && p.Disponible)
            .ToListAsync();
    }

    public async Task<Producto?> GetById(int id)
    {
        return await _context.Productos
            .Include(p => p.Categoria)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Producto> Create(Producto producto)
    {
        producto.CreatedAt = DateTime.UtcNow;
        producto.UpdatedAt = DateTime.UtcNow;
        _context.Productos.Add(producto);
        await _context.SaveChangesAsync();
        return producto;
    }

    public async Task<Producto?> Update(int id, Producto producto)
    {
        var existing = await _context.Productos.FindAsync(id);
        if (existing is null) return null;

        existing.Nombre = producto.Nombre;
        existing.Precio = producto.Precio;
        existing.Imagen = producto.Imagen;
        existing.Disponible = producto.Disponible;
        existing.CategoriaId = producto.CategoriaId;
        existing.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> Delete(int id)
    {
        var producto = await _context.Productos.FindAsync(id);
        if (producto is null) return false;

        _context.Productos.Remove(producto);
        await _context.SaveChangesAsync();
        return true;
    }
}
