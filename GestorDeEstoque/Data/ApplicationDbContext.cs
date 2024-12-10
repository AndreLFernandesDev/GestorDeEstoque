using Microsoft.EntityFrameworkCore;
using GestorDeEstoque.Models;

namespace GestorDeEstoque.Data
{
    public class ApplicationDbContext : DbContext
    {
        private IConfiguration _configuration;

        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Estoque> Estoques { get; set; }
        public DbSet<LogEstoque> LogsEstoques { get; set; }
        public DbSet<ProdutoEstoque> ProdutosEstoques { get; set; }

        public ApplicationDbContext(IConfiguration configuration, DbContextOptions options) : base(options)
        {
            _configuration = configuration ?? throw new ArgumentException(nameof(configuration));
            Produtos = Set<Produto>();
            Estoques = Set<Estoque>();
            LogsEstoques = Set<LogEstoque>();
            ProdutosEstoques = Set<ProdutoEstoque>();
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

            // Tabela Estoque
            modelBuilder.Entity<Estoque>().HasKey(e => e.Id);

            modelBuilder.Entity<Estoque>().Property(e => e.Nome).IsRequired().HasMaxLength(100);

            //Tabela LogEstoque
            modelBuilder.Entity<LogEstoque>().HasKey(l => l.Id);

            modelBuilder.Entity<LogEstoque>().Property(l => l.Quantidade).IsRequired();

            modelBuilder.Entity<LogEstoque>().Property(l => l.Data).IsRequired();

            modelBuilder.Entity<LogEstoque>().Property(l => l.EstoqueId).IsRequired();

            //Relacionamento: LogEstoque -> Produto
            modelBuilder.Entity<LogEstoque>().HasOne(l => l.Produto).WithMany(p => p.LogsEstoque).HasForeignKey(l => l.ProdutoId);

            //Tabela ProdutoEstoque
            modelBuilder.Entity<ProdutoEstoque>().HasKey(pe => new { pe.ProdutoId, pe.EstoqueId });

            modelBuilder.Entity<ProdutoEstoque>().HasOne(pe => pe.Produto).WithMany(p => p.ProdutosEstoques).HasForeignKey(pe => pe.ProdutoId);

            modelBuilder.Entity<ProdutoEstoque>().HasOne(pe => pe.Estoque).WithMany(e => e.ProdutosEstoques).HasForeignKey(pe => pe.EstoqueId);

            modelBuilder.Entity<ProdutoEstoque>().Property(pe => pe.Quantidade).IsRequired();
        }
    }
}