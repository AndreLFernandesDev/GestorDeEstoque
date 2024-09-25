using Microsoft.EntityFrameworkCore;
using GestorDeEstoque.Models;

namespace GestorDeEstoque.Data
{
    public class ApplicationDbContext : DbContext
    {
        private IConfiguration _configuration;

        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Estoque> Estoques { get; set; }

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

            modelBuilder.Entity<Produto>()
            .Property(p => p.Descricao)
            .HasMaxLength(100);

            modelBuilder.Entity<Produto>()
            .Property(p => p.Preco)
            .IsRequired();

            modelBuilder.Entity<Produto>().Property(p => p.EstoqueId).IsRequired();

            modelBuilder.Entity<Produto>().Property(p => p.Quantidade).IsRequired();

            // Tabela Estoque
            modelBuilder.Entity<Estoque>().HasKey(e => e.Id);

            modelBuilder.Entity<Estoque>().Property(e => e.Nome).IsRequired().HasMaxLength(100);

            //Relacionamento: Estoque -> Produto
            modelBuilder.Entity<Estoque>().HasMany(e => e.Produtos).WithOne(p => p.Estoque).HasForeignKey(p => p.EstoqueId);

            //Tabela LogEstoque
            modelBuilder.Entity<LogEstoque>().HasKey(l => l.Id);

            modelBuilder.Entity<LogEstoque>().Property(l => l.Quantidade).IsRequired();

            modelBuilder.Entity<LogEstoque>().Property(l => l.Data).IsRequired();

            //Relacionamento: LogEstoque -> Produto
            modelBuilder.Entity<LogEstoque>().HasOne(l => l.Produto).WithMany(p => p.LogsEstoque).HasForeignKey(l => l.ProdutoId);

        }
    }
}