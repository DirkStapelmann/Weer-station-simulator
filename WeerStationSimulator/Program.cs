using Microsoft.EntityFrameworkCore;
using Weer_station_simulator.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Weer_station_simulator.Models;

var builder = WebApplication.CreateBuilder(args);

// Voeg CORS toe
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()  // Staat verzoeken toe van alle domeinen (frontend)
                  .AllowAnyMethod()   // Staat GET, POST, etc. toe
                  .AllowAnyHeader();  // Staat alle headers toe
        });
});

// Voeg SQLite database toe
builder.Services.AddDbContext<WeatherDbContext>(options =>
    options.UseSqlite("Data Source=weather.db"));

// **Registreer IWeatherMode met een standaard implementatie (DayMode)**
builder.Services.AddSingleton<IWeatherMode, DayMode>();

// **Registreer WeatherModeContext afhankelijk van IWeatherMode**
builder.Services.AddSingleton<WeatherModeContext>(provider =>
{
    var mode = provider.GetRequiredService<IWeatherMode>(); // Haal de standaardmodus op
    return new WeatherModeContext(mode);
});

builder.Services.AddScoped<WeatherStationFacade>();

// Add services to the container.
builder.Services.AddControllers();

var app = builder.Build();

// Gebruik de CORS-policy vóór andere middleware
app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
