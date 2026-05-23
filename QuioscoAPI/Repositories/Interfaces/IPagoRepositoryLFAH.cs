using QuioscoAPI.Models;

namespace QuioscoAPI.Repositories.Interfaces;

public interface IPagoRepositoryLFAH
{
    Task<IEnumerable<Pago>> GetAll();
    Task<Pago?> GetById(int id);
    Task<Pago?> GetByPedidoId(int pedidoId);
    Task<Pago> Create(Pago pago);
    Task<bool> Delete(int id);
}
