namespace WeerStationSimulator.Tests
{
    using Xunit;

    public class SingletonPatternTests
    {
        [Fact]
        public void WeatherStation_Is_Singleton()
        {
            // Act
            var instance1 = WeatherStation.GetInstance();
            var instance2 = WeatherStation.GetInstance();

            // Assert
            Assert.Same(instance1, instance2);
        }
    }

}
