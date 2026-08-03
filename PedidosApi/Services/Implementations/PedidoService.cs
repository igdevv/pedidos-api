using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PedidosApi.Data;
using PedidosApi.DTOs.Pedidos;
using PedidosApi.Entities;
using PedidosApi.Services.Interfaces;

namespace PedidosApi.Services.Implementations
{
    public class PedidoService : IPedidoService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public PedidoService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PedidoDTO>> GetAllAsync()
        {
            var pedidos = await _context.Pedidos.AsNoTracking()
                .Include(pedido => pedido.Detalles)
                .ThenInclude(detalle => detalle.Producto)
                .OrderByDescending(pedido => pedido.FechaCreacion)
                .ToListAsync();

            return _mapper.Map<IEnumerable<PedidoDTO>>(pedidos);
        }

        public async Task<PedidoDTO?> GetByIdAsync(int id)
        {
            var pedido = await _context.Pedidos
                .AsNoTracking()
                .Include(pedido => pedido.Detalles)
                .ThenInclude(detalle => detalle.Producto)
                .FirstOrDefaultAsync(pedido => pedido.Id == id);

            if (pedido is null)
            {
                return null;
            }

            return _mapper.Map<PedidoDTO>(pedido);
        }

        public async Task<PedidoDTO> CreateAsync(CrearPedidoDTO crearPedidoDTO)
        {
            if (crearPedidoDTO.Detalles.Count == 0)
            {
                throw new InvalidOperationException(
                    "El pedido debe contener al menos un producto.");
            }

            var pedido = new Pedido
            {
                NombreCliente = crearPedidoDTO.NombreCliente.Trim(),
                CorreoCliente = crearPedidoDTO.CorreoCliente.Trim()
            };

            foreach (var detalleDTO in crearPedidoDTO.Detalles)
            {
                var producto = await _context.Productos
                    .FirstOrDefaultAsync(
                        producto => producto.Id == detalleDTO.ProductoId);

                if (producto is null)
                {
                    throw new InvalidOperationException(
                        $"El producto con ID {detalleDTO.ProductoId} no existe.");
                }

                if (!producto.Activo)
                {
                    throw new InvalidOperationException(
                        $"El producto '{producto.Nombre}' está inactivo.");
                }

                if (producto.Stock < detalleDTO.Cantidad)
                {
                    throw new InvalidOperationException(
                        $"No hay suficiente stock de '{producto.Nombre}'. " +
                        $"Disponible: {producto.Stock}. " +
                        $"Solicitado: {detalleDTO.Cantidad}.");
                }

                var subtotal = producto.Precio * detalleDTO.Cantidad;

                var detallePedido = new DetallePedido
                {
                    ProductoId = producto.Id,
                    Cantidad = detalleDTO.Cantidad,
                    PrecioUnitario = producto.Precio,
                    Subtotal = subtotal
                };

                pedido.Detalles.Add(detallePedido);

                pedido.Total += subtotal;

                producto.Stock -= detalleDTO.Cantidad;
            }

            _context.Pedidos.Add(pedido);

            await _context.SaveChangesAsync();

            var pedidoCreado = await GetByIdAsync(pedido.Id);

            return pedidoCreado
                ?? throw new InvalidOperationException(
                    "El pedido fue creado, pero no pudo recuperarse.");
        }
    }
}
