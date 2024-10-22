using Microsoft.EntityFrameworkCore;
using GestorDeEstoque.Data;
using GestorDeEstoque.Repositories;
using GestorDeEstoque.Models;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Configuração dos serviços
builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddScoped<ProdutoRepository>();
builder.Services.AddScoped<EstoqueRepository>();
builder.Services.AddScoped<LogRepository>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

app.Run();