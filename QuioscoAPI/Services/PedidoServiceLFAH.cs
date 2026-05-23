using AutoMapper;
using QuioscoAPI.DTOs;
using QuioscoAPI.Models;
using QuioscoAPI.Repositories.Interfaces;
using QuioscoAPI.Services.Interfaces;

namespace QuioscoAPI.Services;

public class PedidoServiceLFAH : IPedidoServiceLFAH
{
    private readonly IPedidoRepositoryLFAH _repository;
    private readonly IMapper _mapper;

    public PedidoServiceLFAH(IPedidoRepositoryLFAH repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PedidoResponseDtoLFAH>> GetAllResponse()
    {
        var pedidos = await _repository.GetPendientes();
        return pedidos.Select(MapResponse);
    }

    public async Task<PedidoResponseDtoLFAH?> GetByIdResponse(int id)
    {
        var pedido = await _repository.GetById(id);
        return pedido is null ? null : MapResponse(pedido);
    }

    public async Task<IEnumerable<PedidoResponseDtoLFAH>> GetByUserId(int userId)
    {
        var pedidos = await _repository.GetByUserId(userId);
        return pedidos.Select(MapResponse);
    }

    public async Task<PedidoDtoLFAH> Create(CreatePedidoDtoLFAH dto, int userId = 0)
    {
        var pedido = new Pedido
        {
            UserId = userId,
            Total = dto.Total,
            Estado = false,
            MesaId = dto.MesaId,
            PedidoProductos = dto.Productos.Select(p => new PedidoProducto
            {
                ProductoId = p.Id,
                Cantidad = p.Cantidad,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }).ToList()
        };

        var created = await _repository.Create(pedido);
        var withRelations = await _repository.GetById(created.Id);
        return _mapper.Map<PedidoDtoLFAH>(withRelations!);
    }

    public async Task<bool> CompleteOrder(int id)
    {
        return await _repository.CompleteOrder(id);
    }

    public async Task<bool> Delete(int id)
    {
        return await _repository.Delete(id);
    }

    public async Task<EstadisticasDtoLFAH> GetEstadisticas()
    {
        return await _repository.GetEstadisticas();
    }

    private static PedidoResponseDtoLFAH MapResponse(Pedido p) => new()
    {
        Id = p.Id,
        Total = p.Total,
        Estado = p.Estado,
        Mesa = p.Mesa is null ? null : new MesaDtoLFAH
        {
            Id = p.Mesa.Id,
            Numero = p.Mesa.Numero,
            Capacidad = p.Mesa.Capacidad,
            Disponible = p.Mesa.Disponible
        },
        Pago = p.Pago is null ? null : new PagoDtoLFAH
        {
            Id = p.Pago.Id,
            PedidoId = p.Pago.PedidoId,
            Metodo = p.Pago.Metodo,
            Monto = p.Pago.Monto,
            FechaPago = p.Pago.FechaPago
        },
        CreatedAt = p.CreatedAt,
        User = new UserBriefDtoLFAH
        {
            Id = p.User.Id,
            Name = p.User.Name,
            Email = p.User.Email
        },
        Productos = p.PedidoProductos.Select(pp => new ProductoPivotDtoLFAH
        {
            Id = pp.Producto.Id,
            Nombre = pp.Producto.Nombre,
            Precio = pp.Producto.Precio,
            Imagen = pp.Producto.Imagen,
            Pivot = new PivotDtoLFAH { Cantidad = pp.Cantidad }
        }).ToList()
    };
}
