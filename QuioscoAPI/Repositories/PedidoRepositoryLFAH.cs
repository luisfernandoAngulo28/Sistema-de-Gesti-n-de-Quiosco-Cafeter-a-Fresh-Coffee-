using Microsoft.EntityFrameworkCore;
using QuioscoAPI.Data;
using QuioscoAPI.DTOs;
using QuioscoAPI.Models;
using QuioscoAPI.Repositories.Interfaces;

namespace QuioscoAPI.Repositories;

public class PedidoRepositoryLFAH : IPedidoRepositoryLFAH
{
    private readonly QuioscoDbContext _context;

    public PedidoRepositoryLFAH(QuioscoDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Pedido>> GetAll()
    {
        return await _context.Pedidos
            .Include(p => p.User)
            .Include(p => p.Mesa)
            .Include(p => p.Pago)
            .Include(p => p.PedidoProductos)
                .ThenInclude(pp => pp.Producto)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Pedido>> GetPendientes()
    {
        return await _context.Pedidos
            .Include(p => p.User)
            .Include(p => p.Mesa)
            .Include(p => p.Pago)
            .Include(p => p.PedidoProductos)
                .ThenInclude(pp => pp.Producto)
            .Where(p => !p.Estado)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    public async Task<Pedido?> GetById(int id)
    {
        return await _context.Pedidos
            .Include(p => p.User)
            .Include(p => p.Mesa)
            .Include(p => p.Pago)
            .Include(p => p.PedidoProductos)
                .ThenInclude(pp => pp.Producto)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Pedido>> GetByUserId(int userId)
    {
        return await _context.Pedidos
            .Include(p => p.User)
            .Include(p => p.Mesa)
            .Include(p => p.Pago)
            .Include(p => p.PedidoProductos)
                .ThenInclude(pp => pp.Producto)
            .Where(p => p.UserId == userId)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    public async Task<Pedido> Create(Pedido pedido)
    {
        pedido.CreatedAt = DateTime.UtcNow;
        pedido.UpdatedAt = DateTime.UtcNow;
        _context.Pedidos.Add(pedido);
        await _context.SaveChangesAsync();
        return pedido;
    }

    public async Task<Pedido?> Update(int id, Pedido pedido)
    {
        var existing = await _context.Pedidos.FindAsync(id);
        if (existing is null) return null;

        existing.Estado = pedido.Estado;
        existing.Total = pedido.Total;
        existing.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> CompleteOrder(int id)
    {
        var pedido = await _context.Pedidos.FindAsync(id);
        if (pedido is null) return false;

        pedido.Estado = true;
        pedido.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> Delete(int id)
    {
        var pedido = await _context.Pedidos.FindAsync(id);
        if (pedido is null) return false;

        _context.Pedidos.Remove(pedido);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<EstadisticasDtoLFAH> GetEstadisticas()
    {
        var totalPedidos = await _context.Pedidos.CountAsync();
        var pedidosPendientes = await _context.Pedidos.CountAsync(p => !p.Estado);
        var pedidosCompletados = await _context.Pedidos.CountAsync(p => p.Estado);
        var ingresoTotal = await _context.Pedidos.SumAsync(p => p.Total);

        var topProductos = await _context.PedidoProductos
            .Include(pp => pp.Producto)
            .GroupBy(pp => new { pp.ProductoId, pp.Producto.Nombre, pp.Producto.Imagen })
            .Select(g => new TopProductoDtoLFAH
            {
                Id = g.Key.ProductoId,
                Nombre = g.Key.Nombre,
                Imagen = g.Key.Imagen,
                TotalVendido = g.Sum(pp => pp.Cantidad)
            })
            .OrderByDescending(x => x.TotalVendido)
            .Take(5)
            .ToListAsync();

        return new EstadisticasDtoLFAH
        {
            TotalPedidos = totalPedidos,
            PedidosPendientes = pedidosPendientes,
            PedidosCompletados = pedidosCompletados,
            IngresoTotal = ingresoTotal,
            TopProductos = topProductos
        };
    }
}
