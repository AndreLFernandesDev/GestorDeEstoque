using GestorDeEstoque.Models;
using Microsoft.EntityFrameworkCore;

namespace GestorDeEstoque.Data
{
    public class ApplicationDbContext : DbContext
    {
        private IConfiguration _configuration;

        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Estoque> Estoques { get; set; }
        public DbSet<LogEstoque> LogsEstoques { get; set; }
        public DbSet<ProdutoEstoque> ProdutosEstoques { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        public ApplicationDbContext(IConfiguration configuration, DbContextOptions options)
            : base(options)
        {
            _configuration = configuration ?? throw new ArgumentException(nameof(configuration));
            Produtos = Set<Produto>();
            Estoques = Set<Estoque>();
            LogsEstoques = Set<LogEstoque>();
            ProdutosEstoques = Set<ProdutoEstoque>();
            Usuarios = Set<Usuario>();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseNpgsql(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Tabela Produto
            modelBuilder.Entity<Produto>().HasKey(p => p.Id);

            modelBuilder.Entity<Produto>().Property(p => p.Nome).IsRequired();

            modelBuilder.Entity<Produto>().Property(p => p.Descricao);

            modelBuilder.Entity<Produto>().Property(p => p.Preco).IsRequired();

            // Tabela Estoque
            modelBuilder.Entity<Estoque>().HasKey(e => e.Id);

            modelBuilder.Entity<Estoque>().Property(e => e.Nome).IsRequired();

            //Tabela LogEstoque
            modelBuilder.Entity<LogEstoque>().HasKey(l => l.Id);

            modelBuilder.Entity<LogEstoque>().Property(l => l.Quantidade).IsRequired();

            modelBuilder.Entity<LogEstoque>().Property(l => l.Data).IsRequired();

            modelBuilder.Entity<LogEstoque>().Property(l => l.EstoqueId).IsRequired();

            //Relacionamento: LogEstoque -> Produto
            modelBuilder
                .Entity<LogEstoque>()
                .HasOne(l => l.Produto)
                .WithMany(p => p.LogsEstoque)
                .HasForeignKey(l => l.ProdutoId);

            //Tabela ProdutoEstoque
            modelBuilder.Entity<ProdutoEstoque>().HasKey(pe => new { pe.ProdutoId, pe.EstoqueId });

            modelBuilder
                .Entity<ProdutoEstoque>()
                .HasOne(pe => pe.Produto)
                .WithMany(p => p.ProdutosEstoques)
                .HasForeignKey(pe => pe.ProdutoId);

            modelBuilder
                .Entity<ProdutoEstoque>()
                .HasOne(pe => pe.Estoque)
                .WithMany(e => e.ProdutosEstoques)
                .HasForeignKey(pe => pe.EstoqueId);

            modelBuilder.Entity<ProdutoEstoque>().Property(pe => pe.Quantidade).IsRequired();

            //Tabela Usuario
            modelBuilder.Entity<Usuario>().HasKey(u => u.Id);
            modelBuilder.Entity<Usuario>().Property(u => u.NomeUsuario).IsRequired();
            modelBuilder.Entity<Usuario>().Property(u => u.SenhaHash).IsRequired();
            modelBuilder.Entity<Usuario>().HasIndex(u => u.NomeUsuario).IsUnique();
        }
    }
}
