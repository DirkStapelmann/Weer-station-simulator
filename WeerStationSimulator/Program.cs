using Microsoft.EntityFrameworkCore;
using Weer_station_simulator.Data;
using Weer_station_simulator.Models;

var builder = WebApplication.CreateBuilder(args);

// Configureert CORS om toegang van andere domeinen toe te staan (bijv. frontend-apps).
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()  // Staat verzoeken toe van alle domeinen
                  .AllowAnyMethod()   // Staat GET, POST, etc. toe
                  .AllowAnyHeader();  // Staat alle headers toe
        });
});

// Configureert de database met SQLite en registreert de databasecontext (Repository Pattern).
builder.Services.AddDbContext<WeatherDbContext>(options =>
    options.UseSqlite("Data Source=weather.db"));

// Singleton Pattern: Registreert IWeatherMode als een singleton en initialiseert met DayMode.
builder.Services.AddSingleton<IWeatherMode, DayMode>();

// Singleton Pattern: WeatherModeContext wordt slechts één keer geïnstantieerd en gebruikt IWeatherMode.
builder.Services.AddSingleton<WeatherModeContext>(provider =>
{
    var mode = provider.GetRequiredService<IWeatherMode>();
    return new WeatherModeContext(mode);
});

// Facade Pattern: WeatherStationFacade wordt als een scoped service toegevoegd.
builder.Services.AddScoped<WeatherStationFacade>();

// Controllers toevoegen aan de dependency injection container.
builder.Services.AddControllers();

var app = builder.Build();

// CORS-policy toepassen vóór andere middleware.
app.UseCors("AllowAll");

// Forceert HTTPS-omleidingen voor beveiliging.
app.UseHttpsRedirection();

// Activeert de ingebouwde autorisatie.
app.UseAuthorization();

// Verbindt de controllers met de API-endpoints.
app.MapControllers();

app.Run();
