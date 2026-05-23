using AutoMapper;
using QuioscoAPI.DTOs;
using QuioscoAPI.Models;
using QuioscoAPI.Repositories.Interfaces;
using QuioscoAPI.Services.Interfaces;

namespace QuioscoAPI.Services;

public class MesaServiceLFAH : IMesaServiceLFAH
{
    private readonly IMesaRepositoryLFAH _repository;
    private readonly IMapper _mapper;

    public MesaServiceLFAH(IMesaRepositoryLFAH repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<MesaDtoLFAH>> GetAll()
    {
        var mesas = await _repository.GetAll();
        return _mapper.Map<IEnumerable<MesaDtoLFAH>>(mesas);
    }

    public async Task<MesaDtoLFAH?> GetById(int id)
    {
        var mesa = await _repository.GetById(id);
        return mesa is null ? null : _mapper.Map<MesaDtoLFAH>(mesa);
    }

    public async Task<MesaDtoLFAH> Create(CreateMesaDtoLFAH dto)
    {
        var mesa = _mapper.Map<Mesa>(dto);
        var created = await _repository.Create(mesa);
        return _mapper.Map<MesaDtoLFAH>(created);
    }

    public async Task<MesaDtoLFAH?> Update(int id, UpdateMesaDtoLFAH dto)
    {
        var mesa = _mapper.Map<Mesa>(dto);
        var updated = await _repository.Update(id, mesa);
        return updated is null ? null : _mapper.Map<MesaDtoLFAH>(updated);
    }

    public async Task<bool> Delete(int id)
    {
        return await _repository.Delete(id);
    }
}
