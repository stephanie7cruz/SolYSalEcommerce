using Microsoft.EntityFrameworkCore;
using SolYSalEcommerce.Data.Models;
using SolYSalEcommerce.Models; // Asegúrate de que este namespace es correcto para tus modelos

namespace SolYSalEcommerce.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // DbSets para cada una de tus entidades
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductVariant> ProductVariants { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; } // ¡NUEVO: Añade este DbSet!

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Definir índices únicos y otras configuraciones si es necesario
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<ProductVariant>()
                .HasIndex(pv => pv.SKU)
                .IsUnique();

            // Relaciones de uno a muchos y muchos a muchos (existentes)
            modelBuilder.Entity<Product>()
                .HasMany(p => p.Variants)
                .WithOne(pv => pv.Product)
                .HasForeignKey(pv => pv.ProductId)
                .OnDelete(DeleteBehavior.Cascade); // Si se borra un producto, sus variantes se borran

            modelBuilder.Entity<User>()
                .HasMany(u => u.CartItems)
                .WithOne(ci => ci.User)
                .HasForeignKey(ci => ci.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProductVariant>()
                .HasMany(pv => pv.CartItems)
                .WithOne(ci => ci.ProductVariant)
                .HasForeignKey(ci => ci.ProductVariantId)
                .OnDelete(DeleteBehavior.Restrict); // No borrar variante si está en un carrito

            modelBuilder.Entity<User>()
                .HasMany(u => u.Orders)
                .WithOne(o => o.User)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderDetails)
                .WithOne(od => od.Order)
                .HasForeignKey(od => od.OrderId)
                .OnDelete(DeleteBehavior.Cascade); // Si se borra un pedido, sus detalles se borran

            modelBuilder.Entity<ProductVariant>()
                .HasMany(pv => pv.OrderDetails)
                .WithOne(od => od.ProductVariant)
                .HasForeignKey(od => od.ProductVariantId)
                .OnDelete(DeleteBehavior.Restrict); // No borrar variante si está en un detalle de pedido

            // --- ¡NUEVAS CONFIGURACIONES PARA ProductImage! ---

            // Relación de Product a ProductImage (un producto puede tener muchas imágenes)
            modelBuilder.Entity<ProductImage>()
                .HasOne(pi => pi.Product) // Una imagen pertenece a un producto
                .WithMany(p => p.Images)  // Un producto puede tener muchas imágenes
                .HasForeignKey(pi => pi.ProductId) // La clave foránea en ProductImage apunta a Product
                .OnDelete(DeleteBehavior.Cascade); // Si se elimina un producto, sus imágenes también se eliminan

            // Relación de ProductVariant a ProductImage (una imagen puede pertenecer a una variante específica, opcionalmente)
            modelBuilder.Entity<ProductImage>()
                .HasOne(pi => pi.ProductVariant) // Una imagen puede pertenecer a una ProductVariant
                .WithMany() // No necesitamos una colección inversa en ProductVariant para esto, pero podrías añadirla si la usas
                .HasForeignKey(pi => pi.ProductVariantId) // La clave foránea en ProductImage apunta a ProductVariant
                .IsRequired(false) // Permite que ProductVariantId sea nulo (es decir, la imagen no está asociada a una variante específica)
                .OnDelete(DeleteBehavior.Restrict); // No borres la variante si aún tiene imágenes. Considera SetNull si la imagen debe permanecer aunque la variante se borre.
        }
    }
}