using Microsoft.EntityFrameworkCore;
using QuioscoAPI.Data;
using QuioscoAPI.Models;
using QuioscoAPI.Repositories.Interfaces;

namespace QuioscoAPI.Repositories;

public class PagoRepositoryLFAH : IPagoRepositoryLFAH
{
    private readonly QuioscoDbContext _context;

    public PagoRepositoryLFAH(QuioscoDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Pago>> GetAll()
    {
        return await _context.Pagos
            .Include(p => p.Pedido)
            .OrderByDescending(p => p.FechaPago)
            .ToListAsync();
    }

    public async Task<Pago?> GetById(int id)
    {
        return await _context.Pagos
            .Include(p => p.Pedido)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Pago?> GetByPedidoId(int pedidoId)
    {
        return await _context.Pagos
            .Include(p => p.Pedido)
            .FirstOrDefaultAsync(p => p.PedidoId == pedidoId);
    }

    public async Task<Pago> Create(Pago pago)
    {
        pago.FechaPago = DateTime.UtcNow;
        pago.CreatedAt = DateTime.UtcNow;
        pago.UpdatedAt = DateTime.UtcNow;
        _context.Pagos.Add(pago);

        var pedido = await _context.Pedidos.FindAsync(pago.PedidoId);
        if (pedido is not null)
        {
            pedido.Estado = true;
            pedido.UpdatedAt = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync();
        return pago;
    }

    public async Task<bool> Delete(int id)
    {
        var pago = await _context.Pagos.FindAsync(id);
        if (pago is null) return false;

        _context.Pagos.Remove(pago);
        await _context.SaveChangesAsync();
        return true;
    }
}
