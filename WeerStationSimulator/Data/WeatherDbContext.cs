using Microsoft.EntityFrameworkCore;
using Weer_station_simulator.Models;

namespace Weer_station_simulator.Data
{
    // Database-contextklasse voor het beheren van temperatuurmetingen.
    // Maakt gebruik van Entity Framework Core (EF Core) als ORM.
    public class WeatherDbContext : DbContext
    {
        // Constructor die de configuratieparameters ontvangt via Dependency Injection.
        public WeatherDbContext(DbContextOptions<WeatherDbContext> options) : base(options) { }

        // DbSet representeert de tabel 'TemperatureReadings' in de database.
        // Dit wordt gebruikt om temperatuurmetingen op te slaan en op te vragen.
        public DbSet<TemperatureReading> TemperatureReadings { get; set; }
    }
}
