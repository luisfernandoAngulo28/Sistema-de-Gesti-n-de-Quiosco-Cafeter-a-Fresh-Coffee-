using AutoMapper;
using QuioscoAPI.DTOs;
using QuioscoAPI.Models;
using QuioscoAPI.Repositories.Interfaces;
using QuioscoAPI.Services.Interfaces;

namespace QuioscoAPI.Services;

public class PagoServiceLFAH : IPagoServiceLFAH
{
    private readonly IPagoRepositoryLFAH _repository;
    private readonly IMapper _mapper;

    public PagoServiceLFAH(IPagoRepositoryLFAH repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PagoDtoLFAH>> GetAll()
    {
        var pagos = await _repository.GetAll();
        return _mapper.Map<IEnumerable<PagoDtoLFAH>>(pagos);
    }

    public async Task<PagoDtoLFAH?> GetById(int id)
    {
        var pago = await _repository.GetById(id);
        return pago is null ? null : _mapper.Map<PagoDtoLFAH>(pago);
    }

    public async Task<PagoDtoLFAH?> GetByPedidoId(int pedidoId)
    {
        var pago = await _repository.GetByPedidoId(pedidoId);
        return pago is null ? null : _mapper.Map<PagoDtoLFAH>(pago);
    }

    public async Task<PagoDtoLFAH> Create(CreatePagoDtoLFAH dto)
    {
        var pago = _mapper.Map<Pago>(dto);
        var created = await _repository.Create(pago);
        return _mapper.Map<PagoDtoLFAH>(created);
    }

    public async Task<bool> Delete(int id)
    {
        return await _repository.Delete(id);
    }
}
