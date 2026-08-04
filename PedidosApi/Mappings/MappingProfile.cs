using AutoMapper;
using PedidosApi.DTOs.Auth;
using PedidosApi.DTOs.Pedidos;
using PedidosApi.DTOs.Productos;
using PedidosApi.Entities;

namespace PedidosApi.Mappings
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<Producto, ProductoDTO>();

            CreateMap<CrearProductoDTO, Producto>();

            CreateMap<ActualizarProductoDTO, Producto>();


            CreateMap<Pedido, PedidoDTO>();

            CreateMap<DetallePedido, DetallePedidoDTO>()
                .ForMember(
                    destino => destino.NombreProducto,
                    opciones => opciones.MapFrom(
                        origen => origen.Producto.Nombre));

            CreateMap<RegistroUsuarioDTO, Usuario>();
        }
    }
}
