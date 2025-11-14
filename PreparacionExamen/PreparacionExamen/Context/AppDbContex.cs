using Microsoft.EntityFrameworkCore;
using PreparacionExamen.Models;

namespace PreparacionExamen.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Proveedor> Proveedores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //  Producto
            modelBuilder.Entity<Producto>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Nombre).IsRequired().HasMaxLength(100);
                entity.Property(p => p.DescripcionCorta).HasMaxLength(200);
                entity.Property(p => p.Precio).HasColumnType("decimal(18,2)");

                // Relaciones
                entity.HasOne(p => p.Categoria)
                      .WithMany(c => c.Productos)
                      .HasForeignKey(p => p.CategoriaId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(p => p.Proveedor)
                      .WithMany(pr => pr.Productos)
                      .HasForeignKey(p => p.ProveedorId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Configuración de Categoría
            modelBuilder.Entity<Categoria>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Nombre).IsRequired().HasMaxLength(100);
                entity.Property(c => c.Descripcion).HasMaxLength(500);

              //aqui se encuetra la categoria 
                entity.HasIndex(c => c.Nombre).IsUnique();
            });

            //  Proveedor
            modelBuilder.Entity<Proveedor>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.RazonSocial).IsRequired().HasMaxLength(200);
                entity.Property(p => p.Contacto).HasMaxLength(100);

                // razones sociales
                entity.HasIndex(p => p.RazonSocial).IsUnique();
            });

            // pruebas...
            modelBuilder.Entity<Categoria>().HasData(
                new Categoria { Id = 1, Nombre = "Electrónicos", Descripcion = "Productos electrónicos " },
                new Categoria { Id = 2, Nombre = "Hogar", Descripcion = "Productos para el hogar" },
                new Categoria { Id = 3, Nombre = "Ropa", Descripcion = "Prendas de vestir" }
            );

            modelBuilder.Entity<Proveedor>().HasData(
                new Proveedor { Id = 1, RazonSocial = "Cocacola", Contacto = "contacto@techcorp.com" },
                new Proveedor { Id = 2, RazonSocial = "Fancesa", Contacto = "ventas@homesupplies.com" },
                new Proveedor { Id = 3, RazonSocial = "salviety", Contacto = "info@fashionstyle.com" }
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}