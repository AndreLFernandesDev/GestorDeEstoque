using Microsoft.EntityFrameworkCore;

namespace GestorDeEstoque.Repositories
{
    public class ApplicationDbContext : DbContext
    {
        private IConfiguration _configuration;

        public DbSet<User> Users { get; set; }
        public ApplicationDbContext(IConfiguration configuration, DbContextOptions options) : base(options)
        {
            _configuration = configuration ?? throw new ArgumentException(nameof(configuration));
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseNpgsql(connectionString);
        }
    }
}