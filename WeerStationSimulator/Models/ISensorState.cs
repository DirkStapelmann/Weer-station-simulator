namespace WeerStationSimulator.Models
{
    // Interface voor het State Pattern: definieert verschillende toestanden van de sensor.
    public interface ISensorState
    {
        string GetStatus();
    }

    // Concreet onderdeel van het State Pattern: de sensor is actief en functioneert normaal.
    public class ActiveState : ISensorState
    {
        public string GetStatus() => "Sensor is actief";
    }

    // Concreet onderdeel van het State Pattern: de sensor heeft een storing.
    public class ErrorState : ISensorState
    {
        public string GetStatus() => "Sensor heeft een storing";
    }

    // Concreet onderdeel van het State Pattern: de sensor is offline en verzendt geen data.
    public class OfflineState : ISensorState
    {
        public string GetStatus() => "Sensor is offline";
    }
}
