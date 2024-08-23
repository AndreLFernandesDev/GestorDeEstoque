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
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configuração do pipeline de middleware
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
}

app.UseHttpsRedirection();

app.UseRouting(); // Habilita o roteamento

app.UseAuthentication();
app.UseAuthorization();

// Mapeia os controladores para as rotas da API
app.MapControllers();

app.Run();

