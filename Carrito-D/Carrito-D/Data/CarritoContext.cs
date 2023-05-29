using Carrito_D.Models;
using Microsoft.EntityFrameworkCore;

namespace Carrito_D.Data
{
    public class CarritoContext : DbContext
    {
        public CarritoContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Producto>().Property(p => p.PrecioVigente).HasPrecision(38,18);

            modelBuilder.Entity<CarritoItem>().HasKey(ci => new { ci.CarritoId, ci.ProductoId });

            modelBuilder.Entity<CarritoItem>()
                .HasOne(ci => ci.Carrito)
                .WithMany(c => c.CarritoItems)
                .HasForeignKey(ci => ci.CarritoId);

            modelBuilder.Entity<CarritoItem>()
               .HasOne(ci => ci.Producto)
               .WithMany(i => i.CarritoItems)
               .HasForeignKey(ci => ci.ProductoId);

            modelBuilder.Entity<StockItem>().HasKey(sp => new { sp.SucursalId, sp.ProductoId });

            modelBuilder.Entity<StockItem>()
                .HasOne(sp => sp.Sucursal)
                .WithMany(s => s.StockItems)
                .HasForeignKey(sp => sp.SucursalId);

            modelBuilder.Entity<StockItem>()
                .HasOne(sp => sp.Producto)
                .WithMany(p => p.StockItems)
                .HasForeignKey(sp => sp.ProductoId);

        }

        public DbSet<Persona> Personas { get; set; }

        public DbSet<Carrito> Carritos { get; set; }

        public DbSet<Categoria> Categorias { get; set; }

        public DbSet<Cliente> Clientes { get; set; }

        public DbSet<Compra> Compras { get; set; }

        public DbSet<Empleado> Empleados { get; set; }

        public DbSet<Producto> Productos { get; set; }

        public DbSet<Sucursal> Sucursales { get; set; }

        public DbSet<CarritoItem> CarritoItems { get; set; }

        public DbSet<StockItem> StockItems { get; set; }

    }
}
