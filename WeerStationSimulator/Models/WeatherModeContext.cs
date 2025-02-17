namespace Weer_station_simulator.Models
{
    // Contextklasse voor het State Pattern: beheert de huidige modus van het weerstation.
    public class WeatherModeContext
    {
        private IWeatherMode _mode; // Huidige modus van het weerstation

        // Constructor initialiseert de modus (bijvoorbeeld DayMode of NightMode).
        public WeatherModeContext(IWeatherMode mode)
        {
            _mode = mode;
        }

        // Wijzigt de huidige modus van het weerstation.
        public void SetMode(IWeatherMode mode)
        {
            _mode = mode;
        }

        // Geeft de beschrijving van de huidige modus terug.
        public string GetMode()
        {
            return _mode.GetMode();
        }
    }
}
