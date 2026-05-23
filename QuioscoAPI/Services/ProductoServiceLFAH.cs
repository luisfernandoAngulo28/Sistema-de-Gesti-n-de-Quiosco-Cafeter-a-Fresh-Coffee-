using AutoMapper;
using QuioscoAPI.DTOs;
using QuioscoAPI.Models;
using QuioscoAPI.Repositories.Interfaces;
using QuioscoAPI.Services.Interfaces;

namespace QuioscoAPI.Services;

public class ProductoServiceLFAH : IProductoServiceLFAH
{
    private readonly IProductoRepositoryLFAH _repository;
    private readonly IMapper _mapper;

    public ProductoServiceLFAH(IProductoRepositoryLFAH repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProductoDtoLFAH>> GetAll()
    {
        var productos = await _repository.GetAll();
        return _mapper.Map<IEnumerable<ProductoDtoLFAH>>(productos);
    }

    public async Task<IEnumerable<ProductoDtoLFAH>> GetByCategoria(int categoriaId)
    {
        var productos = await _repository.GetByCategoria(categoriaId);
        return _mapper.Map<IEnumerable<ProductoDtoLFAH>>(productos);
    }

    public async Task<ProductoDtoLFAH?> GetById(int id)
    {
        var producto = await _repository.GetById(id);
        return producto is null ? null : _mapper.Map<ProductoDtoLFAH>(producto);
    }

    public async Task<ProductoDtoLFAH> Create(CreateProductoDtoLFAH dto)
    {
        var producto = _mapper.Map<Producto>(dto);
        var created = await _repository.Create(producto);
        var withCategory = await _repository.GetById(created.Id);
        return _mapper.Map<ProductoDtoLFAH>(withCategory!);
    }

    public async Task<ProductoDtoLFAH?> Update(int id, UpdateProductoDtoLFAH dto)
    {
        var producto = _mapper.Map<Producto>(dto);
        var updated = await _repository.Update(id, producto);
        if (updated is null) return null;
        var withCategory = await _repository.GetById(updated.Id);
        return _mapper.Map<ProductoDtoLFAH>(withCategory!);
    }

    public async Task<bool> Delete(int id)
    {
        return await _repository.Delete(id);
    }
}
