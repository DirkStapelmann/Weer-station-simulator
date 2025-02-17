using WeerStationSimulator.Models;

// Strategy Pattern - Interface voor temperatuurconversie
public interface ITemperatureConverter
{
    float ConvertTemperature(float temperature);
}

// Concrete strategieën voor verschillende temperatuurconversies
public class CelsiusConverter : ITemperatureConverter
{
    public float ConvertTemperature(float temperature) => temperature;
}

public class FahrenheitConverter : ITemperatureConverter
{
    public float ConvertTemperature(float temperature) => (temperature * 9 / 5) + 32;
}

public class KelvinConverter : ITemperatureConverter
{
    public float ConvertTemperature(float temperature) => temperature + 273.15f;
}

// Context klasse die een conversiestrategie gebruikt (Strategy Pattern)
public class TemperatureContext
{
    private ITemperatureConverter _converter;

    public TemperatureContext(ITemperatureConverter converter)
    {
        _converter = converter;
    }

    public void SetConverter(ITemperatureConverter converter)
    {
        _converter = converter;
    }

    public float GetConvertedTemperature(float temperature)
    {
        return _converter.ConvertTemperature(temperature);
    }
}

// Singleton Pattern: Zorgt ervoor dat er slechts één instantie van WeatherStation bestaat
public class WeatherStation
{
    private static WeatherStation? _instance;
    private static readonly object _lock = new object();
    private List<IObserver> observers = new List<IObserver>(); // Observer Pattern
    private Random random = new Random();
    private float latestTemperature;
    private TemperatureContext temperatureContext;

    // Privé constructor voorkomt directe instantie van de klasse
    private WeatherStation()
    {
        latestTemperature = (float)(random.NextDouble() * 40 - 10);
        temperatureContext = new TemperatureContext(new CelsiusConverter()); // Default naar Celsius
    }

    // Singleton Pattern: Geeft de enige instantie van WeatherStation terug
    public static WeatherStation GetInstance()
    {
        lock (_lock)
        {
            if (_instance == null)
                _instance = new WeatherStation();
        }
        return _instance;
    }

    // Observer Pattern: Toevoegen van een observer die op temperatuurwijzigingen reageert
    public void AddObserver(IObserver observer)
    {
        observers.Add(observer);
    }

    // Observer Pattern: Verwijderen van een observer
    public void RemoveObserver(IObserver observer)
    {
        observers.Remove(observer);
    }

    // Observer Pattern: Notificeert alle geregistreerde observers bij een wijziging
    public void NotifyObservers()
    {
        foreach (var observer in observers)
        {
            observer.Update(latestTemperature);
        }
    }

    // Genereert een willekeurige temperatuur en informeert de observers
    public void GenerateTemperature()
    {
        latestTemperature = (float)(random.NextDouble() * 40 - 10);
        NotifyObservers();
    }

    // Strategy Pattern: Wijzigt de temperatuurconversiestrategie
    public void SetTemperatureUnit(ITemperatureConverter converter)
    {
        temperatureContext.SetConverter(converter);
    }

    // Strategy Pattern: Geeft de laatst gemeten temperatuur terug in de gekozen eenheid
    public float GetConvertedTemperature()
    {
        return temperatureContext.GetConvertedTemperature(latestTemperature);
    }
}
