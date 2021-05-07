using Microsoft.EntityFrameworkCore;

#nullable disable

namespace IA2Assessment.Models
{
    /// <summary>
    ///     Handles connection to the tuckshop database
    /// </summary>
    public partial class TuckshopDbContext : DbContext
    {
        public TuckshopDbContext()
        {
        }

        public TuckshopDbContext(DbContextOptions<TuckshopDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        ///     MenuItems table
        /// </summary>
        public virtual DbSet<MenuItem> MenuItems { get; set; }
        
        /// <summary>
        ///     Orders table
        /// </summary>
        public virtual DbSet<Order> Orders { get; set; }
        
        /// <summary>
        ///     OrdersDetails table
        /// </summary>
        public virtual DbSet<OrdersDetail> OrdersDetails { get; set; }
        
        /// <summary>
        ///     Users table
        /// </summary>
        public virtual DbSet<User> Users { get; set; }
        
        /// <summary>
        ///     UserRoles table
        /// </summary>
        public virtual DbSet<UserRole> UserRoles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //If we are not configured then connect to the database
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySQL("Name=ConnectionStrings:TuckshopConnection");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Configure primary keys
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
