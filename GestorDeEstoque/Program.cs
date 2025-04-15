using System.Text.Json.Serialization;
using GestorDeEstoque.Data;
using GestorDeEstoque.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Configuração dos serviços
builder
    .Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));
builder.Services.AddScoped<ProdutoRepository>();
builder.Services.AddScoped<EstoqueRepository>();
builder.Services.AddScoped<LogRepository>();
builder.Services.AddScoped<ProdutoEstoqueRepository>();
builder.Services.AddScoped<UsuarioRepository>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

app.Run();
