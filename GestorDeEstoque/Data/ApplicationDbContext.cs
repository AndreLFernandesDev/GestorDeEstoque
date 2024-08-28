using Microsoft.EntityFrameworkCore;
using GestorDeEstoque.Models;

namespace GestorDeEstoque.Data
{
    public class ApplicationDbContext : DbContext
    {
        private IConfiguration _configuration;

        public DbSet<User> Users { get; set; }

        public DbSet<Produto> Produto { get; set; }

        public ApplicationDbContext(IConfiguration configuration, DbContextOptions options) : base(options)
        {
            _configuration = configuration ?? throw new ArgumentException(nameof(configuration));
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseNpgsql(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Tabela Produto
            modelBuilder.Entity<Produto>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<Produto>()
                .Property(p => p.Nome)
                .IsRequired()
                .HasMaxLength(100);

            base.OnModelCreating(modelBuilder);
        }
    }
}