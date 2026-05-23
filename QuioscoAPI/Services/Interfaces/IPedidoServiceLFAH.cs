using QuioscoAPI.DTOs;

namespace QuioscoAPI.Services.Interfaces;

public interface IPedidoServiceLFAH
{
    Task<IEnumerable<PedidoResponseDtoLFAH>> GetAllResponse();
    Task<PedidoResponseDtoLFAH?> GetByIdResponse(int id);
    Task<IEnumerable<PedidoResponseDtoLFAH>> GetByUserId(int userId);
    Task<PedidoDtoLFAH> Create(CreatePedidoDtoLFAH dto, int userId = 0);
    Task<bool> CompleteOrder(int id);
    Task<bool> Delete(int id);
    Task<EstadisticasDtoLFAH> GetEstadisticas();
}
