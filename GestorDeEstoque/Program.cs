using Microsoft.EntityFrameworkCore;
using GestorDeEstoque.Data;
using GestorDeEstoque.Repositories;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Configuração dos serviços
builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddScoped<UserRepository>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseRouting(); // Habilita o roteamento
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers(); // Mapeia os controladores para as rotas da API

app.Run();

