using PedidosApi.DTOs.Productos;
using PedidosApi.Entities;

namespace PedidosApi.Services.Interfaces
{
    public interface IProductoService
    {
        // obtener todos los productos
        Task<IEnumerable<ProductoDTO>> GetAllAsync(bool incluirInactivos = false);

        // obtener un producto por id
        Task<ProductoDTO?> GetByIdAsync(int id);

        // crear un producto
        Task<ProductoDTO> CreateAsync(CrearProductoDTO crearProductoDTO);

        // actualizar un producto
        Task<ProductoDTO?> UpdateAsync(int id, ActualizarProductoDTO actualizarProductoDTO);

        // eliminar un producto
        Task<bool> DeleteAsync(int id);
    }
}
