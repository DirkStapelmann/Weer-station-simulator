namespace WeerStationSimulator.Tests
{
    using Xunit;
    using WeerStationSimulator.Models;

    public class StatePatternTests
    {
        [Fact]
        public void SensorState_Changes_Correctly()
        {
            // Arrange
            var sensorContext = new SensorContext();

            // Act & Assert
            sensorContext.SetState(new ActiveState());
            Assert.Equal("Sensor is actief", sensorContext.GetStatus());

            sensorContext.SetState(new ErrorState());
            Assert.Equal("Sensor heeft een storing", sensorContext.GetStatus());

            sensorContext.SetState(new OfflineState());
            Assert.Equal("Sensor is offline", sensorContext.GetStatus());
        }
    }

}
