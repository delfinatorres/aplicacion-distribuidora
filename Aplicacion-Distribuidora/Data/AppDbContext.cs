using Microsoft.EntityFrameworkCore;
using Aplicacion_Distribuidora.Models;

namespace Aplicacion_Distribuidora.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<DetallePedido> DetallesPedido { get; set; }
        public DbSet<Vendedor> Vendedores { get; set; }
        public DbSet<Pago> Pagos { get; set; }
        public DbSet<Boleta> Boletas { get; set; }
        public DbSet<Entrega> Entregas { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            var connectionString = "Server=localhost;Port=3306;Database=DBDistribuidora;User=root;Password=root;";
            options.UseMySql(
                connectionString,
                ServerVersion.AutoDetect(connectionString)
            );
        }
    }
}
