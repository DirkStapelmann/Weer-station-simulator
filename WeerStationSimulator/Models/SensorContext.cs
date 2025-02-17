namespace WeerStationSimulator.Models
{
    public class SensorContext
    {
        private ISensorState _state;

        public SensorContext()
        {
            // Standaard status is 'Actief'
            _state = new ActiveState();
        }

        public void SetState(ISensorState newState)
        {
            _state = newState;
        }

        public string GetStatus()
        {
            return _state.GetStatus();
        }
    }
}