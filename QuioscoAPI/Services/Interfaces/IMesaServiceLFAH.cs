using QuioscoAPI.DTOs;

namespace QuioscoAPI.Services.Interfaces;

public interface IMesaServiceLFAH
{
    Task<IEnumerable<MesaDtoLFAH>> GetAll();
    Task<MesaDtoLFAH?> GetById(int id);
    Task<MesaDtoLFAH> Create(CreateMesaDtoLFAH dto);
    Task<MesaDtoLFAH?> Update(int id, UpdateMesaDtoLFAH dto);
    Task<bool> Delete(int id);
}
