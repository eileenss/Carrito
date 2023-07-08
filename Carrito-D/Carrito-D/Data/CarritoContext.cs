using Carrito_D.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Carrito_D.Data
{
    public class CarritoContext : IdentityDbContext<IdentityUser<int>, IdentityRole<int>, int>
    {
        public CarritoContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Producto>().Property(p => p.PrecioVigente).HasPrecision(38, 18);

            modelBuilder.Entity<Compra>().Property(c => c.Total).HasPrecision(38, 18);

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

            #region Identity Stores para resolver la utilización de ASPNETUSERS en las tablas
            modelBuilder.Entity<IdentityUser<int>>().ToTable("Personas");
            modelBuilder.Entity<IdentityRole<int>>().ToTable("Roles");
            modelBuilder.Entity<IdentityUserRole<int>>().ToTable("PersonasRoles");
            #endregion

            #region Unique
            //modelBuilder.Entity<Persona>().HasIndex(p => p.DNI).IsUnique();
            modelBuilder.Entity<Cliente>().HasIndex(c => c.DNI).IsUnique();
            modelBuilder.Entity<Empleado>().HasIndex(e => e.DNI).IsUnique();
            modelBuilder.Entity<Empleado>().HasIndex(e => e.Legajo).IsUnique();
            modelBuilder.Entity<Producto>().HasIndex(p => p.Nombre).IsUnique();
            modelBuilder.Entity<Categoria>().HasIndex(c => c.Nombre).IsUnique();
            modelBuilder.Entity<Sucursal>().HasIndex(s => s.Direccion).IsUnique();
            modelBuilder.Entity<Sucursal>().HasIndex(s => s.Nombre).IsUnique();
            #endregion

            #region Secuencia automática para legajo
            modelBuilder.HasSequence<int>("Legajo").StartsAt(1000).IncrementsBy(1);
            modelBuilder.Entity<Empleado>().Property(e => e.Legajo).HasDefaultValueSql("NEXT VALUE FOR Legajo");
            #endregion
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

        //public DbSet<IdentityRole<int>> Roles { get; set; }

    }
}
