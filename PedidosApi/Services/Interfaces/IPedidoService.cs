using Microsoft.AspNetCore.Mvc;
using PedidosApi.DTOs.Pedidos;

namespace PedidosApi.Services.Interfaces
{
    public interface IPedidoService
    {
        Task<IEnumerable<PedidoDTO>> GetAllAsync();

        Task <PedidoDTO?> GetByIdAsync(int id);

        Task<PedidoDTO> CreateAsync(CrearPedidoDTO crearPedidoDTO);
    }
}
