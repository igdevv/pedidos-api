using System.ComponentModel.DataAnnotations;

namespace PedidosApi.DTOs.Productos
{
    public class ProductoDTO
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public required string Nombre { get; set; }
        [StringLength(500)]
        public string? Descripcion { get; set; }
        [Range(0.01, 999999999)]
        public decimal Precio { get; set; }
        [Range(0, int.MaxValue)]
        public int Stock { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
