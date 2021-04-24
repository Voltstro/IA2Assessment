using IA2Assessment.Models.Views;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace IA2Assessment.Models
{
    public partial class TuckshopDbContext : DbContext
    {
        public TuckshopDbContext()
        {
        }

        public TuckshopDbContext(DbContextOptions<TuckshopDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<MenuItem> MenuItems { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrdersDetail> OrdersDetails { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySQL("Name=ConnectionStrings:TuckshopConnection");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MenuItem>(entity =>
            {
                entity.HasKey(e => e.ItemId)
                    .HasName("PRIMARY");
            });

            modelBuilder.Entity<OrdersDetail>(entity =>
            {
                entity.HasKey(e => e.OrderDetailId)
                    .HasName("PRIMARY");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PRIMARY");
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(e => e.RoleId)
                    .HasName("PRIMARY");
            });
            
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
