using Microsoft.EntityFrameworkCore;
using QuioscoAPI.Data;
using QuioscoAPI.Models;
using QuioscoAPI.Repositories.Interfaces;

namespace QuioscoAPI.Repositories;

public class MesaRepositoryLFAH : IMesaRepositoryLFAH
{
    private readonly QuioscoDbContext _context;

    public MesaRepositoryLFAH(QuioscoDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Mesa>> GetAll()
    {
        return await _context.Mesas
            .Include(m => m.Pedidos.Where(p => !p.Estado))
            .OrderBy(m => m.Numero)
            .ToListAsync();
    }

    public async Task<Mesa?> GetById(int id)
    {
        return await _context.Mesas.FindAsync(id);
    }

    public async Task<Mesa> Create(Mesa mesa)
    {
        mesa.CreatedAt = DateTime.UtcNow;
        mesa.UpdatedAt = DateTime.UtcNow;
        _context.Mesas.Add(mesa);
        await _context.SaveChangesAsync();
        return mesa;
    }

    public async Task<Mesa?> Update(int id, Mesa mesa)
    {
        var existing = await _context.Mesas.FindAsync(id);
        if (existing is null) return null;

        existing.Numero = mesa.Numero;
        existing.Capacidad = mesa.Capacidad;
        existing.Disponible = mesa.Disponible;
        existing.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> Delete(int id)
    {
        var mesa = await _context.Mesas.FindAsync(id);
        if (mesa is null) return false;

        _context.Mesas.Remove(mesa);
        await _context.SaveChangesAsync();
        return true;
    }
}
