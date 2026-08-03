using System.ComponentModel.DataAnnotations;

namespace PedidosApi.DTOs.Pedidos
{
    public class CrearPedidoDTO
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public required string NombreCliente { get; set; }

        [Required]
        [EmailAddress]
        public required string CorreoCliente { get; set; }

        [Required]
        [MinLength(1)]
        public ICollection<CrearDetallePedidoDTO> Detalles { get; set; } = new List<CrearDetallePedidoDTO>();
    }
}
