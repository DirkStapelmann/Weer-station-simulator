using Weer_station_simulator.Data;
using WeerStationSimulator.Models;

namespace Weer_station_simulator.Models
{
    // Facade Pattern: Biedt een eenvoudige interface om verschillende systemen te beheren.
    public class WeatherStationFacade
    {
        private readonly WeatherStation _weatherStation;  // Singleton Pattern wordt gebruikt
        private readonly WeatherDbContext _dbContext;    // Database Context (Repository Pattern)
        private WeatherModeContext _weatherModeContext;  // State Pattern voor weermodus
        private SensorContext _sensorContext;            // State Pattern voor sensorstatus

        // Constructor initialiseert de afhankelijkheden en standaardwaarden.
        public WeatherStationFacade(WeatherDbContext dbContext)
        {
            _weatherStation = WeatherStation.GetInstance(); // Singleton Pattern
            _dbContext = dbContext;
            _weatherModeContext = new WeatherModeContext(new DayMode()); // State Pattern
            _sensorContext = new SensorContext(); // Zorgt ervoor dat er altijd een sensorstatus is
        }

        // Ophalen van de sensorstatus via het State Pattern.
        public string GetSensorStatus()
        {
            return _sensorContext.GetStatus();
        }

        // Wijzigt de sensorstatus via het State Pattern.
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

        // Strategy Pattern: Haalt de laatste temperatuur op en past de juiste conversiestrategie toe.
        public float GetLatestTemperature(ITemperatureConverter converter)
        {
            _weatherStation.SetTemperatureUnit(converter);
            return _weatherStation.GetConvertedTemperature();
        }

        // Genereert een nieuwe temperatuur en slaat deze op in de database.
        public void GenerateAndSaveTemperature(string unit = "C")
        {
            _weatherStation.GenerateTemperature();

            // Strategy Pattern: Bepaalt de juiste conversiestrategie
            ITemperatureConverter converter = unit.ToUpper() switch
            {
                "F" => new FahrenheitConverter(),
                "K" => new KelvinConverter(),
                _ => new CelsiusConverter()
            };

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

        // Ophalen van de temperatuurgeschiedenis uit de database.
        public List<TemperatureReading> GetTemperatureHistory()
        {
            return _dbContext.TemperatureReadings.OrderByDescending(t => t.Timestamp).ToList();
        }

        // Instellen van de weermodus via het State Pattern.
        public void SetWeatherMode(IWeatherMode mode)
        {
            _weatherModeContext.SetMode(mode);
        }

        // Ophalen van de huidige weermodus.
        public string GetCurrentWeatherMode()
        {
            return _weatherModeContext.GetMode();
        }
    }
}
