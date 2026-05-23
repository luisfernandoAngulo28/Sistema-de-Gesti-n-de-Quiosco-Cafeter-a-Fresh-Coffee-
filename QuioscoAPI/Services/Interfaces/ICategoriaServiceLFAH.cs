using QuioscoAPI.DTOs;

namespace QuioscoAPI.Services.Interfaces;

public interface ICategoriaServiceLFAH
{
    Task<IEnumerable<CategoriaDtoLFAH>> GetAll();
    Task<CategoriaDtoLFAH?> GetById(int id);
    Task<CategoriaDtoLFAH> Create(CreateCategoriaDtoLFAH dto);
    Task<CategoriaDtoLFAH?> Update(int id, UpdateCategoriaDtoLFAH dto);
    Task<bool> Delete(int id);
}
