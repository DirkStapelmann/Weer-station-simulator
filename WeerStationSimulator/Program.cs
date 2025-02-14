using Microsoft.EntityFrameworkCore;
using Weer_station_simulator.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;


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

// Add services to the container.
builder.Services.AddControllers();

var app = builder.Build();

// Gebruik de CORS-policy vóór andere middleware
app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
