using QuioscoAPI.Models;

namespace QuioscoAPI.Repositories.Interfaces;

public interface IProductoRepositoryLFAH
{
    Task<IEnumerable<Producto>> GetAll();
    Task<IEnumerable<Producto>> GetByCategoria(int categoriaId);
    Task<Producto?> GetById(int id);
    Task<Producto> Create(Producto producto);
    Task<Producto?> Update(int id, Producto producto);
    Task<bool> Delete(int id);
}
