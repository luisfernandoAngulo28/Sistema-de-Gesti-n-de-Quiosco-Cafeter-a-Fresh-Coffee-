using AutoMapper;
using QuioscoAPI.DTOs;
using QuioscoAPI.Models;

namespace QuioscoAPI.Mapping;

public class MappingProfileLFAH : Profile
{
    public MappingProfileLFAH()
    {
        // User
        CreateMap<User, UserDtoLFAH>();
        CreateMap<CreateUserDtoLFAH, User>();
        CreateMap<UpdateUserDtoLFAH, User>();

        // Categoria
        CreateMap<Categoria, CategoriaDtoLFAH>();
        CreateMap<CreateCategoriaDtoLFAH, Categoria>();
        CreateMap<UpdateCategoriaDtoLFAH, Categoria>();

        // Producto
        CreateMap<Producto, ProductoDtoLFAH>()
            .ForMember(dest => dest.CategoriaNombre, opt => opt.MapFrom(src => src.Categoria.Nombre));
        CreateMap<CreateProductoDtoLFAH, Producto>();
        CreateMap<UpdateProductoDtoLFAH, Producto>();

        // Pedido
        CreateMap<Pedido, PedidoDtoLFAH>()
            .ForMember(dest => dest.Usuario, opt => opt.MapFrom(src => src.User))
            .ForMember(dest => dest.Productos, opt => opt.MapFrom(src => src.PedidoProductos));
        CreateMap<CreatePedidoDtoLFAH, Pedido>();
        CreateMap<UpdatePedidoDtoLFAH, Pedido>();

        // PedidoProducto
        CreateMap<PedidoProducto, PedidoProductoDtoLFAH>()
            .ForMember(dest => dest.ProductoNombre, opt => opt.MapFrom(src => src.Producto.Nombre))
            .ForMember(dest => dest.Precio, opt => opt.MapFrom(src => src.Producto.Precio));
        CreateMap<CreatePedidoProductoDtoLFAH, PedidoProducto>();

        // Mesa
        CreateMap<Mesa, MesaDtoLFAH>()
            .ForMember(dest => dest.Ocupada, opt => opt.MapFrom(src => src.Pedidos.Any(p => !p.Estado)));
        CreateMap<CreateMesaDtoLFAH, Mesa>();
        CreateMap<UpdateMesaDtoLFAH, Mesa>();

        // Pago
        CreateMap<Pago, PagoDtoLFAH>();
        CreateMap<CreatePagoDtoLFAH, Pago>();
    }
}
