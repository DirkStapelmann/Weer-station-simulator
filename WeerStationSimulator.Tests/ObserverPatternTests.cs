namespace WeerStationSimulator.Tests
{
    using Xunit;
    using WeerStationSimulator.Models;

    public class ObserverPatternTests
    {
        private class TestObserver : IObserver
        {
            public float ReceivedTemperature { get; private set; }

            public void Update(float temperature)
            {
                ReceivedTemperature = temperature;
            }
        }

        [Fact]
        public void Observer_Receives_Updated_Temperature()
        {
            // Arrange
            var weatherStation = WeatherStation.GetInstance();
            var observer = new TestObserver();

            weatherStation.AddObserver(observer);

            // Act
            weatherStation.GenerateTemperature();

            // Assert
            Assert.NotEqual(0, observer.ReceivedTemperature);
        }
    }

}