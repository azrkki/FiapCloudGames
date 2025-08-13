using FCG.Core.Entity;
using FCG.Core.Entity.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace FCG.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        private readonly string _connectionString;
        
        public ApplicationDbContext()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            _connectionString = configuration.GetConnectionString("ConnectionString");
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public ApplicationDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserGameLibrary> UserGameLibraries { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured && !string.IsNullOrEmpty(_connectionString))
                optionsBuilder.UseSqlServer(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // User configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnType("INT").UseIdentityColumn();
                entity.Property(e => e.Name).HasColumnType("VARCHAR(100)").IsRequired();
                entity.Property(e => e.Email).HasColumnType("VARCHAR(100)").IsRequired();
                entity.Property(e => e.Password)
                    .HasColumnType("VARCHAR(255)")
                    .IsRequired()
                    .HasConversion(
                        password => password.Value,
                        hashedValue => Password.FromHashedValue(hashedValue));
                entity.Property(e => e.RemoveFlag).HasColumnType("nvarchar(1)").IsRequired();
                entity.Property(e => e.RoleId).HasColumnType("INT").IsRequired();

                entity.HasOne(e => e.Role)
                    .WithMany(r => r.Users)
                    .HasForeignKey(e => e.RoleId);
            });

            // Game configuration
            modelBuilder.Entity<Game>(entity =>
            {
                entity.ToTable("Game");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnType("INT").UseIdentityColumn();
                entity.Property(e => e.Name).HasColumnType("VARCHAR(100)").IsRequired();
                entity.Property(e => e.Description).HasColumnType("VARCHAR(500)").IsRequired();
                entity.Property(e => e.Price).HasColumnType("decimal(18,2)").IsRequired();
                entity.Property(e => e.Discount).HasColumnType("int").IsRequired();
            });

            // Role configuration
            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnType("INT").UseIdentityColumn();
                entity.Property(e => e.Name).HasColumnType("VARCHAR(50)").IsRequired();
            });

            // UserGameLibrary configuration
            modelBuilder.Entity<UserGameLibrary>(entity =>
            {
                entity.ToTable("UserGameLibrary");
                entity.HasKey(e => new { e.UserId, e.GameId });
                entity.Property(e => e.UserId).HasColumnType("INT");
                entity.Property(e => e.GameId).HasColumnType("INT");

                entity.HasOne(e => e.User)
                    .WithMany(u => u.GameLibrary)
                    .HasForeignKey(e => e.UserId);

                entity.HasOne(e => e.Game)
                    .WithMany(g => g.UserGameLibraries)
                    .HasForeignKey(e => e.GameId);
            });
        }
    }
}