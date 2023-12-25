using Composition.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace Composition.Database
{
    public class AppDBContext : IdentityDbContext<User, UserRole, Guid>
    {
        public AppDBContext(DbContextOptions<AppDBContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<UserPicture> UsersPictures { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<SubCategory> SubCategories {  get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //User's relationships
            modelBuilder.Entity<User>()
                .HasKey(t => new { t.Id });
            modelBuilder.Entity<User>()
                .HasMany(p => p.UserPictures)
                .WithOne(u => u.User)
                .HasForeignKey(g => g.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<User>()
                .HasMany(a => a.Orders)
                .WithOne(b => b.User)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            //Order's relationships
            modelBuilder.Entity<Order>()
                .HasMany(a => a.OrderItems)
                .WithOne(b => b.Order)
                .HasForeignKey(c => c.OrderId)
                .OnDelete(DeleteBehavior.SetNull);

            //Product's relationships
            modelBuilder.Entity<Product>()
                .HasMany(a => a.SubCategories)
                .WithOne(b => b.Product)
                .HasForeignKey(c => c.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
            //n : n relationships

            modelBuilder.Entity<ProductCategory>()
                .HasKey(pf => new { pf.CategoryId, pf.ProductId });
            modelBuilder.Entity<ProductCategory>()
                .HasOne(pc => pc.Product)
                .WithMany(pc => pc.ProductCategories)
                .HasForeignKey(pc => pc.ProductId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ProductCategory>()
                .HasOne(pc => pc.Category)
                .WithMany(pc => pc.ProductCategories)
                .HasForeignKey(pc => pc.CategoryId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}
