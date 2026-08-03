using System.ComponentModel.DataAnnotations;

namespace PedidosApi.DTOs.Pedidos
{
    public class CrearDetallePedidoDTO
    {
        [Range(1, int.MaxValue)]
        public int ProductoId { get; set; }

        [Range(1, int.MaxValue)]
        public int Cantidad { get; set; }
    }
}
