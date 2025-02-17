namespace WeerStationSimulator.Tests
{
    using Xunit;

    public class StrategyPatternTests
    {
        [Theory]
        [InlineData(0, "C", 0)]
        [InlineData(100, "C", 100)]
        [InlineData(0, "F", 32)]
        [InlineData(100, "F", 212)]
        [InlineData(0, "K", 273.15)]
        [InlineData(100, "K", 373.15)]
        public void TemperatureConversion_Works_Correctly(float inputTemp, string unit, float expectedOutput)
        {
            // Arrange
            ITemperatureConverter converter = unit switch
            {
                "F" => new FahrenheitConverter(),
                "K" => new KelvinConverter(),
                _ => new CelsiusConverter()
            };

            var context = new TemperatureContext(converter);

            // Act
            float result = context.GetConvertedTemperature(inputTemp);

            // Assert
            Assert.Equal(expectedOutput, result, 2);
        }
    }

}
