namespace WeerStationSimulator.Models
{
    // Interface voor sensor status
    public interface ISensorState
    {
        string GetStatus();
    }

    // Sensor is actief en functioneert correct
    public class ActiveState : ISensorState
    {
        public string GetStatus() => "Sensor is actief";
    }

    // Sensor heeft een storing
    public class ErrorState : ISensorState
    {
        public string GetStatus() => "Sensor heeft een storing";
    }

    // Sensor is offline en verzendt geen data
    public class OfflineState : ISensorState
    {
        public string GetStatus() => "Sensor is offline";
    }
}