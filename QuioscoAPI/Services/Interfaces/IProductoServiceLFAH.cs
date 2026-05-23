using QuioscoAPI.DTOs;

namespace QuioscoAPI.Services.Interfaces;

public interface IProductoServiceLFAH
{
    Task<IEnumerable<ProductoDtoLFAH>> GetAll();
    Task<IEnumerable<ProductoDtoLFAH>> GetByCategoria(int categoriaId);
    Task<ProductoDtoLFAH?> GetById(int id);
    Task<ProductoDtoLFAH> Create(CreateProductoDtoLFAH dto);
    Task<ProductoDtoLFAH?> Update(int id, UpdateProductoDtoLFAH dto);
    Task<bool> Delete(int id);
}
