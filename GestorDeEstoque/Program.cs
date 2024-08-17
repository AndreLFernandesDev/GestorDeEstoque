using Microsoft.EntityFrameworkCore;
using GestorDeEstoque.Data;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Configuração dos serviços
builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configuração do pipeline de middleware
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyAPI v1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

app.UseRouting(); // Habilita o roteamento

app.UseAuthentication();
app.UseAuthorization();

// Mapeia os controladores para as rotas da API
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers(); // Mapeia todas as rotas definidas nos controladores
});

app.Run();

