using PedidosApi.Enums;

namespace PedidosApi.DTOs.Pedidos
{
    public class PedidoDTO
    {
        public int Id { get; set; }

        public required string NombreCliente { get; set; }

        public required string CorreoCliente { get; set; }

        public DateTime FechaCreacion { get; set; }

        public EstadoPedido Estado { get; set; }

        public decimal Total { get; set; }

        public ICollection<DetallePedidoDTO> Detalles { get; set; }
            = new List<DetallePedidoDTO>();
    }
}
