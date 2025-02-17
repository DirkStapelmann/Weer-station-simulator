namespace Weer_station_simulator.Models
{
    // Interface voor het State Pattern: definieert verschillende modi van het weerstation.
    public interface IWeatherMode
    {
        // Geeft de beschrijving van de huidige modus terug.
        string GetMode();
    }
}
