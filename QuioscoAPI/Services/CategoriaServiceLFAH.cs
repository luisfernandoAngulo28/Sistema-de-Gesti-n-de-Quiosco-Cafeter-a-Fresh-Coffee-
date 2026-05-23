using AutoMapper;
using QuioscoAPI.DTOs;
using QuioscoAPI.Models;
using QuioscoAPI.Repositories.Interfaces;
using QuioscoAPI.Services.Interfaces;

namespace QuioscoAPI.Services;

public class CategoriaServiceLFAH : ICategoriaServiceLFAH
{
    private readonly ICategoriaRepositoryLFAH _repository;
    private readonly IMapper _mapper;

    public CategoriaServiceLFAH(ICategoriaRepositoryLFAH repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CategoriaDtoLFAH>> GetAll()
    {
        var categorias = await _repository.GetAll();
        return _mapper.Map<IEnumerable<CategoriaDtoLFAH>>(categorias);
    }

    public async Task<CategoriaDtoLFAH?> GetById(int id)
    {
        var categoria = await _repository.GetById(id);
        return categoria is null ? null : _mapper.Map<CategoriaDtoLFAH>(categoria);
    }

    public async Task<CategoriaDtoLFAH> Create(CreateCategoriaDtoLFAH dto)
    {
        var categoria = _mapper.Map<Categoria>(dto);
        var created = await _repository.Create(categoria);
        return _mapper.Map<CategoriaDtoLFAH>(created);
    }

    public async Task<CategoriaDtoLFAH?> Update(int id, UpdateCategoriaDtoLFAH dto)
    {
        var categoria = _mapper.Map<Categoria>(dto);
        var updated = await _repository.Update(id, categoria);
        return updated is null ? null : _mapper.Map<CategoriaDtoLFAH>(updated);
    }

    public async Task<bool> Delete(int id)
    {
        return await _repository.Delete(id);
    }
}
