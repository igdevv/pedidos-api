using System.ComponentModel.DataAnnotations;

namespace PedidosApi.DTOs.Auth
{
    public class RegistroUsuarioDTO
    {
        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        [MaxLength(150)]
        public string Correo { get; set; } = string.Empty;
        [Required]
        [MinLength(6)]
        [MaxLength(100)]
        public string Password { get; set; } = string.Empty;
    }
}
