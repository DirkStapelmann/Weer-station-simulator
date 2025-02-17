namespace WeerStationSimulator.Models
{
    // Interface voor het Observer Pattern.
    // Objecten die deze interface implementeren kunnen worden 'geïnformeerd' over temperatuurupdates.
    public interface IObserver
    {
        // Wordt aangeroepen wanneer de temperatuur verandert.
        void Update(float temperature);
    }
}
