using QuioscoAPI.Models;

namespace QuioscoAPI.Repositories.Interfaces;

public interface ICategoriaRepositoryLFAH
{
    Task<IEnumerable<Categoria>> GetAll();
    Task<Categoria?> GetById(int id);
    Task<Categoria> Create(Categoria categoria);
    Task<Categoria?> Update(int id, Categoria categoria);
    Task<bool> Delete(int id);
}
