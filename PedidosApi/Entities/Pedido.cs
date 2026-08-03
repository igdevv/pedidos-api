using PedidosApi.Enums;

namespace PedidosApi.Entities
{
    public class Pedido
    {
        public int Id { get; set; }
        public required string NombreCliente { get; set; }
        public required string CorreoCliente { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public EstadoPedido Estado { get; set; } = EstadoPedido.Pendiente;
        public decimal Total { get; set; }
        public ICollection<DetallePedido> Detalles { get; set; } = [];
    }
}
