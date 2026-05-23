using QuioscoAPI.DTOs;
using QuioscoAPI.Models;

namespace QuioscoAPI.Repositories.Interfaces;

public interface IPedidoRepositoryLFAH
{
    Task<IEnumerable<Pedido>> GetAll();
    Task<IEnumerable<Pedido>> GetPendientes();
    Task<Pedido?> GetById(int id);
    Task<IEnumerable<Pedido>> GetByUserId(int userId);
    Task<Pedido> Create(Pedido pedido);
    Task<Pedido?> Update(int id, Pedido pedido);
    Task<bool> CompleteOrder(int id);
    Task<bool> Delete(int id);
    Task<EstadisticasDtoLFAH> GetEstadisticas();
}
