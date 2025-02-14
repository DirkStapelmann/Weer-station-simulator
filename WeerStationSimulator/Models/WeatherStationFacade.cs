using Weer_station_simulator.Data;
using Weer_station_simulator.Models;


public class WeatherStationFacade
{
    private readonly WeatherStation _weatherStation;
    private readonly WeatherDbContext _dbContext;
    private WeatherModeContext _weatherModeContext;

    public WeatherStationFacade(WeatherDbContext dbContext)
    {
        _weatherStation = WeatherStation.GetInstance();  // Singleton Pattern
        _dbContext = dbContext;
        _weatherModeContext = new WeatherModeContext(new DayMode()); // Standaard dagmodus
    }

    // 🌡️ Haal de laatste temperatuur op in de gekozen eenheid
    public float GetLatestTemperature(ITemperatureConverter converter)
    {
        _weatherStation.SetTemperatureUnit(converter);
        return _weatherStation.GetConvertedTemperature();
    }

    // 🔄 Genereer een nieuwe temperatuur en sla op in de database
    public void GenerateAndSaveTemperature()
    {
        _weatherStation.GenerateTemperature();
        var tempReading = new TemperatureReading
        {
            Temperature = _weatherStation.GetConvertedTemperature(),
            Unit = "C",
            Timestamp = DateTime.UtcNow
        };

        _dbContext.TemperatureReadings.Add(tempReading);
        _dbContext.SaveChanges();
    }

    // 📊 Haal de temperatuurgeschiedenis op
    public List<TemperatureReading> GetTemperatureHistory()
    {
        return _dbContext.TemperatureReadings.OrderByDescending(t => t.Timestamp).ToList();
    }

    // 🌙 Dag- en Nachtmodus wisselen (Strategy Pattern)
    public void SetWeatherMode(IWeatherMode mode)
    {
        _weatherModeContext.SetMode(mode);
    }

    public string GetCurrentWeatherMode()
    {
        return _weatherModeContext.GetMode();
    }
}
