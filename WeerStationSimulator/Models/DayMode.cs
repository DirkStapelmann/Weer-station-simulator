namespace Weer_station_simulator.Models
{
    // Concreet onderdeel van het State Pattern: vertegenwoordigt de dagmodus van het weerstation.
    public class DayMode : IWeatherMode
    {
        // Geeft een beschrijving van de actieve modus.
        public string GetMode() => "Dagmodus: Lichte UI en real-time updates.";
    }
}
