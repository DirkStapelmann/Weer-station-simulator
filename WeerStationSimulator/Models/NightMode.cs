namespace Weer_station_simulator.Models
{
    public class NightMode : IWeatherMode
    {
        public string GetMode() => "Nachtmodus: Donkere UI en minder updates.";
    }

}
