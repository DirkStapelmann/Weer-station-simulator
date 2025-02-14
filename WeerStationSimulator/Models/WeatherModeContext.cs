namespace Weer_station_simulator.Models
{
    public class WeatherModeContext
    {
        private IWeatherMode _mode;

        public WeatherModeContext(IWeatherMode mode)
        {
            _mode = mode;
        }

        public void SetMode(IWeatherMode mode)
        {
            _mode = mode;
        }

        public string GetMode()
        {
            return _mode.GetMode();
        }
    }

}
