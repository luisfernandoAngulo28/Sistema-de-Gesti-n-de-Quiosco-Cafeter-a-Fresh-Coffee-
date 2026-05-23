using QuioscoAPI.DTOs;

namespace QuioscoAPI.Services.Interfaces;

public interface IPagoServiceLFAH
{
    Task<IEnumerable<PagoDtoLFAH>> GetAll();
    Task<PagoDtoLFAH?> GetById(int id);
    Task<PagoDtoLFAH?> GetByPedidoId(int pedidoId);
    Task<PagoDtoLFAH> Create(CreatePagoDtoLFAH dto);
    Task<bool> Delete(int id);
}
