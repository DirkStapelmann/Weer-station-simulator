namespace Weer_station_simulator.Models
{
    // Concreet onderdeel van het State Pattern: vertegenwoordigt de nachtmodus van het weerstation.
    public class NightMode : IWeatherMode
    {
        // Geeft een beschrijving van de actieve modus.
        public string GetMode() => "Nachtmodus: Donkere UI en minder updates.";
    }
}
