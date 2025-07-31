using Microsoft.EntityFrameworkCore;
using Task4.Models;

namespace Task4.AppDbContext
{


    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {
        }

        public DbSet<UserModel> Users { get; set; }
        public DbSet<RoleModel> Rols { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //UserModel configuration
            modelBuilder.Entity<UserModel>( entity =>
            {
                //Primary key configuration
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Id).ValueGeneratedOnAdd();
                // Unique index for Email
                entity.HasIndex(u => u.Email).IsUnique();
                // Property configurations
                entity.Property(u => u.Name).HasMaxLength(100);
                entity.Property(u => u.Email).IsRequired().HasMaxLength(100);
                entity.Property(u => u.Password).IsRequired().HasMaxLength(100);
                entity.HasOne(u => u.Role)
                      .WithMany()
                      .HasForeignKey(x => x.RoleId);
            });

            //RoleModel configuration
            modelBuilder.Entity<RoleModel>(entity =>
            {
                //Primary key configuration
                entity.HasKey(r => r.Id);
                entity.Property(r => r.Id).ValueGeneratedOnAdd();
                // Unique index for Role
                entity.HasIndex(r => r.Role).IsUnique();
                // Property configurations
                entity.Property(r => r.Role).IsRequired().HasMaxLength(50);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}