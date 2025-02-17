using Weer_station_simulator.Data;
using Weer_station_simulator.Models;
using WeerStationSimulator.Models;

namespace Weer_station_simulator.Models
{
    public class WeatherStationFacade
    {
        private readonly WeatherStation _weatherStation;
        private readonly WeatherDbContext _dbContext;
        private WeatherModeContext _weatherModeContext;
        private SensorContext _sensorContext; // Verwijder 'readonly' zodat we dit kunnen wijzigen

        public WeatherStationFacade(WeatherDbContext dbContext)
        {
            _weatherStation = WeatherStation.GetInstance();  // Singleton Pattern
            _dbContext = dbContext;
            _weatherModeContext = new WeatherModeContext(new DayMode()); // Standaard dagmodus
            _sensorContext = new SensorContext(); // Zorgt ervoor dat er altijd een sensorstatus is
        }

        // 🌡️ Sensorstatus ophalen
        public string GetSensorStatus()
        {
            return _sensorContext.GetStatus(); // _sensorContext kan nooit null zijn
        }

        // 🔄 Sensorstatus wijzigen
        public void SetSensorStatus(string status)
        {
            switch (status.ToLower())
            {
                case "active":
                    _sensorContext.SetState(new ActiveState());
                    break;
                case "error":
                    _sensorContext.SetState(new ErrorState());
                    break;
                case "offline":
                    _sensorContext.SetState(new OfflineState());
                    break;
                default:
                    throw new ArgumentException("Ongeldige status. Gebruik 'active', 'error' of 'offline'.");
            }
        }

        // 🌡️ Haal de laatste temperatuur op in de gekozen eenheid
        public float GetLatestTemperature(ITemperatureConverter converter)
        {
            _weatherStation.SetTemperatureUnit(converter);
            return _weatherStation.GetConvertedTemperature();
        }

        // 🔄 Genereer een nieuwe temperatuur en sla op in de database
        public void GenerateAndSaveTemperature(string unit = "C")
        {
            _weatherStation.GenerateTemperature();

            // Bepaal de juiste conversiestrategie
            ITemperatureConverter converter = unit.ToUpper() switch
            {
                "F" => new FahrenheitConverter(),
                "K" => new KelvinConverter(),
                _ => new CelsiusConverter()
            };

            // Pas de temperatuurconversie toe
            _weatherStation.SetTemperatureUnit(converter);
            float convertedTemperature = _weatherStation.GetConvertedTemperature();

            var tempReading = new TemperatureReading
            {
                Temperature = convertedTemperature,
                Unit = unit.ToUpper(),
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
}
