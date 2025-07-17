using Microsoft.AspNetCore.Identity; 
using Microsoft.AspNetCore.Identity.EntityFrameworkCore; 
using Microsoft.EntityFrameworkCore;
using SolYSalEcommerce.Models;
using System; 

namespace SolYSalEcommerce.Data
{
    // ApplicationDbContext hereda de IdentityDbContext con tus tipos de User y Role
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Tus DbSets existentes (excepto DbSet<User>, que ya lo maneja IdentityDbContext)
        public DbSet<Product> Products { get; set; } = default!;
        // public DbSet<User> Users { get; set; } = default!; // 
        public DbSet<ProductVariant> ProductVariants { get; set; } = default!;
        public DbSet<CartItem> CartItems { get; set; } = default!;
        public DbSet<Order> Orders { get; set; } = default!;
        public DbSet<OrderDetail> OrderDetails { get; set; } = default!;
        public DbSet<ProductImage> ProductImages { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProductVariant>()
                .HasIndex(pv => pv.SKU)
                .IsUnique();

            // Relaciones de tus entidades personalizadas
            modelBuilder.Entity<Product>()
                .HasMany(p => p.Variants)
                .WithOne(pv => pv.Product)
                .HasForeignKey(pv => pv.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relaciones que involucran a tu clase User (ahora AspNetUsers)
            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.User)
                .WithMany(u => u.CartItems) 
                .HasForeignKey(ci => ci.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders) 
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<ProductVariant>()
                .HasMany(pv => pv.CartItems)
                .WithOne(ci => ci.ProductVariant)
                .HasForeignKey(ci => ci.ProductVariantId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderDetails)
                .WithOne(od => od.Order)
                .HasForeignKey(od => od.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProductVariant>()
                .HasMany(pv => pv.OrderDetails)
                .WithOne(od => od.ProductVariant)
                .HasForeignKey(od => od.ProductVariantId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuraciones para ProductImage
            modelBuilder.Entity<ProductImage>()
                .HasOne(pi => pi.Product)
                .WithMany(p => p.Images)
                .HasForeignKey(pi => pi.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProductImage>()
                .HasOne(pi => pi.ProductVariant)
                .WithMany()
                .HasForeignKey(pi => pi.ProductVariantId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}