using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using PedidosApi.Data;
using PedidosApi.DTOs.Productos;
using PedidosApi.Entities;
using PedidosApi.Services.Interfaces;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace PedidosApi.Services.Implementations
{
    public class ProductoService : IProductoService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ProductoService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ProductoDTO> CreateAsync(CrearProductoDTO crearProductoDTO)
        {

            var nombreNormalizado = crearProductoDTO.Nombre.Trim();

            var existeProducto = await _context.Productos
                .AnyAsync(producto => producto.Nombre == nombreNormalizado);

            if (existeProducto)
            {
                throw new InvalidOperationException(
                    "Ya existe un producto con ese nombre.");
            }

            var producto = _mapper.Map<Producto>(crearProductoDTO); //aqui convertimos el dto que nos envia el cliente en la entidad producto

            _context.Productos.Add(producto); //agregamos la entidad al context, todavia no se ejecuta el insert en la db

            await _context.SaveChangesAsync(); //aqui si se ejecuta el insert y se guarda en la db

            return _mapper.Map<ProductoDTO>(producto); // ahora aqui la entidad se convierte en productodto y se devuelve al cliente
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var productos = await _context.Productos.FirstOrDefaultAsync(p => p.Id == id);

            if (productos is null)
            {
                return false;
            }

            productos.Activo = true; //aqui se cambia el estado del producto a activo, en lugar de eliminarlo fisicamente de la db

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<ProductoDTO>> GetAllAsync(bool incluirInactivos = false)
        {
            var query = _context.Productos.AsNoTracking().AsQueryable();

            if (!incluirInactivos)
            {
                query = query.Where(producto => producto.Activo);
            }

            var productos = await query.ToListAsync();

            return _mapper.Map<IEnumerable<ProductoDTO>>(productos);
        }
        public async Task<ProductoDTO?> GetByIdAsync(int id)
        {
            var productos = await _context.Productos.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);

            if (productos is null)
            {
                return null;
            }

            return _mapper.Map<ProductoDTO?>(productos);

        }

        public async Task<ProductoDTO?> UpdateAsync(int id, ActualizarProductoDTO actualizarProductoDTO)
        {
            var producto = await _context.Productos.FirstOrDefaultAsync(p => p.Id == id);

            if (producto is null)
            {
                return null;
            }

            _mapper.Map(actualizarProductoDTO, producto); //aqui se mapea el dto que nos envia el cliente a la entidad producto que ya existe en la db)

            await _context.SaveChangesAsync();

            return _mapper.Map<ProductoDTO?>(producto);
        }
    }
}
