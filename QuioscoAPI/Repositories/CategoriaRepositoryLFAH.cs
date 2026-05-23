using Microsoft.EntityFrameworkCore;
using QuioscoAPI.Data;
using QuioscoAPI.Models;
using QuioscoAPI.Repositories.Interfaces;

namespace QuioscoAPI.Repositories;

public class CategoriaRepositoryLFAH : ICategoriaRepositoryLFAH
{
    private readonly QuioscoDbContext _context;

    public CategoriaRepositoryLFAH(QuioscoDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Categoria>> GetAll()
    {
        return await _context.Categorias.OrderBy(c => c.Nombre).ToListAsync();
    }

    public async Task<Categoria?> GetById(int id)
    {
        return await _context.Categorias.FindAsync(id);
    }

    public async Task<Categoria> Create(Categoria categoria)
    {
        categoria.CreatedAt = DateTime.UtcNow;
        categoria.UpdatedAt = DateTime.UtcNow;
        _context.Categorias.Add(categoria);
        await _context.SaveChangesAsync();
        return categoria;
    }

    public async Task<Categoria?> Update(int id, Categoria categoria)
    {
        var existing = await _context.Categorias.FindAsync(id);
        if (existing is null) return null;

        existing.Nombre = categoria.Nombre;
        existing.Icono = categoria.Icono;
        existing.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> Delete(int id)
    {
        var categoria = await _context.Categorias.FindAsync(id);
        if (categoria is null) return false;

        _context.Categorias.Remove(categoria);
        await _context.SaveChangesAsync();
        return true;
    }
}
