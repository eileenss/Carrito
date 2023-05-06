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

            modelBuilder.Entity<CarritoItem>().HasKey(ci => new { ci.CarritoId, ci.ProductoId });

            modelBuilder.Entity<CarritoItem>()
                .HasOne(ci => ci.Carrito)
                .WithMany(c => c.CarritoItems)
                .HasForeignKey(ci => ci.CarritoId);

            modelBuilder.Entity<CarritoItem>()
               .HasOne(ci => ci.Producto)
               .WithMany(i => i.CarritoItems)
               .HasForeignKey(ci => ci.ProductoId);

        }

        public DbSet<Persona> Personas { get; set; }

    }
}
