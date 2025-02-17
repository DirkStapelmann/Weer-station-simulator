namespace WeerStationSimulator.Models
{
    // Contextklasse voor het State Pattern: beheert de huidige toestand van de sensor.
    public class SensorContext
    {
        private ISensorState _state; // Huidige status van de sensor

        public SensorContext()
        {
            // Standaard status is 'Actief'
            _state = new ActiveState();
        }

        // Wijzigt de huidige status van de sensor naar een nieuwe toestand.
        public void SetState(ISensorState newState)
        {
            _state = newState;
        }

        // Haalt de statusbeschrijving van de huidige toestand op.
        public string GetStatus()
        {
            return _state.GetStatus();
        }
    }
}
