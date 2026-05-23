using QuioscoAPI.Models;

namespace QuioscoAPI.Repositories.Interfaces;

public interface IMesaRepositoryLFAH
{
    Task<IEnumerable<Mesa>> GetAll();
    Task<Mesa?> GetById(int id);
    Task<Mesa> Create(Mesa mesa);
    Task<Mesa?> Update(int id, Mesa mesa);
    Task<bool> Delete(int id);
}
