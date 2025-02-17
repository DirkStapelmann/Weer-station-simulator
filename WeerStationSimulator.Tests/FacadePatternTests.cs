namespace WeerStationSimulator.Tests
{
    using Xunit;
    using Weer_station_simulator.Models;
    using Weer_station_simulator.Data;
    using Microsoft.EntityFrameworkCore;

    public class FacadePatternTests
    {
        [Fact]
        public void GenerateAndSaveTemperature_SavesDataCorrectly()
        {
            // Arrange: Maak een In-Memory database aan
            var options = new DbContextOptionsBuilder<WeatherDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDB")
                .Options;

            using (var dbContext = new WeatherDbContext(options))
            {
                var facade = new WeatherStationFacade(dbContext);

                // Act: Genereer en sla een temperatuur op
                facade.GenerateAndSaveTemperature("C");

                // Assert: Controleer of de data correct is opgeslagen
                var history = facade.GetTemperatureHistory();
                Assert.NotNull(history);
                Assert.True(history.Count > 0);
            }
        }
    }


}
