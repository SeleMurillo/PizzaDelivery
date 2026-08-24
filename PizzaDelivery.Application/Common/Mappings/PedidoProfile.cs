using AutoMapper;
using PizzaDelivery.Application.Pedidos.DTOs;
using PizzaDelivery.Domain.DTOs;
using PizzaDelivery.Domain.Entities;

namespace PizzaDelivery.Application.Common.Mappings
{
    public class PedidoProfile : Profile
    {
        public PedidoProfile()
        {
            // Mapeo de Pedido → PedidoDto
            CreateMap<Pedido, PedidoDto>()
                .ForMember(dest => dest.ClienteNombre,
                    opt => opt.MapFrom(src => src.Cliente != null ? src.Cliente.Nombre : "Cliente no especificado"))
                .ForMember(dest => dest.Direccion,
                    opt => opt.MapFrom(src => $"{src.DireccionEntrega.Calle} #{src.DireccionEntrega.Numero}, {src.DireccionEntrega.Colonia}, {src.DireccionEntrega.Ciudad}"))
                .ForMember(dest => dest.Pizzas,
                    opt => opt.MapFrom(src => src.Pizzas.Select(p => p.Nombre).ToList()))
                .ForMember(dest => dest.Estado,
                    opt => opt.MapFrom(src => src.Estado.ToString()))
                .ForMember(dest => dest.FechaCreacion,
                    opt => opt.MapFrom(src => src.CreatedAt));

            // Mapeo de Pedido → PedidoResumenDto
            CreateMap<Pedido, PedidoResumenDto>()
                .ForMember(dest => dest.Cliente,
                    opt => opt.MapFrom(src => src.Cliente != null ? src.Cliente.Nombre : "Cliente no especificado"))
                .ForMember(dest => dest.CantidadPizzas,
                    opt => opt.MapFrom(src => src.Pizzas.Count))
                .ForMember(dest => dest.Fecha,
                    opt => opt.MapFrom(src => src.CreatedAt));
        }
    }
}
