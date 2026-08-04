using System.ComponentModel.DataAnnotations;

namespace PedidosApi.DTOs.Auth
{
    public class LoginUsuarioDTO
    {
        [Required]
        [EmailAddress]
        public string Correo { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
