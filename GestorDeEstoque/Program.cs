using Microsoft.EntityFrameworkCore;
using GestorDeEstoque.Data;

var builderDb = WebApplication.CreateBuilder(args);
var connectionString = builderDb.Configuration.GetConnectionString("DefaultConnection");

// Configuração dos serviços
builderDb.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

var app = builderDb.Build();

// Testar a conexão
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    if (dbContext.Database.CanConnect())
    {
        Console.WriteLine("Conexão com o banco de dados estabelecida com sucesso.");
    }
    else
    {
        Console.WriteLine("Falha ao conectar com o banco de dados.");
    }
}

// Configuração do pipeline HTTP
app.Run();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var appWeb = builder.Build();

// Configure the HTTP request pipeline.
if (appWeb.Environment.IsDevelopment())
{
    appWeb.UseSwagger();
    appWeb.UseSwaggerUI();
}

appWeb.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

appWeb.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

appWeb.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
