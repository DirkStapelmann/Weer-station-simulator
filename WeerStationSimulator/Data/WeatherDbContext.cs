using Microsoft.EntityFrameworkCore;
using Weer_station_simulator.Models;

namespace Weer_station_simulator.Data
{
    public class WeatherDbContext : DbContext
    {
        public WeatherDbContext(DbContextOptions<WeatherDbContext> options) : base(options) { }

        public DbSet<TemperatureReading> TemperatureReadings { get; set; }
    }
}
