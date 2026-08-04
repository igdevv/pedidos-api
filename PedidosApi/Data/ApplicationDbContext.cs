using Microsoft.EntityFrameworkCore;
using PedidosApi.Entities;

namespace PedidosApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }


        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<DetallePedido> DetallesPedidos { get; set; }
       public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Usuario>()
                .HasIndex(usuario => usuario.Correo)
                .IsUnique();
        }
    
    }


}
