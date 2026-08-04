using Microsoft.AspNetCore.Identity;

namespace PedidosApi.Entities
{
    public class Usuario
    {
        public int Id { get; set; }
        public  string Nombre { get; set; } = string.Empty;
        public  string Correo { get; set; } = string.Empty;
        public  string PasswordHash { get; set; } = string.Empty;
        public string Rol { get; set; } = "Usuario";
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
    }
}
