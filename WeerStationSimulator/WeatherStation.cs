// Observer Pattern - Interface voor temperatuur updates
using System;
using System.Collections.Generic;

public interface IObserver
{
    void Update(float temperature);
}

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

// Context klasse die een conversiestrategie gebruikt
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

public class WeatherStation
{
    private static WeatherStation? _instance;
    private static readonly object _lock = new object();
    private List<IObserver> observers = new List<IObserver>();
    private Random random = new Random();
    private float latestTemperature;
    private TemperatureContext temperatureContext;

    private WeatherStation()
    {
        latestTemperature = (float)(random.NextDouble() * 40 - 10);
        temperatureContext = new TemperatureContext(new CelsiusConverter()); // Default naar Celsius
    }

    public static WeatherStation GetInstance()
    {
        lock (_lock)
        {
            if (_instance == null)
                _instance = new WeatherStation();
        }
        return _instance;
    }

    public void AddObserver(IObserver observer)
    {
        observers.Add(observer);
    }

    public void RemoveObserver(IObserver observer)
    {
        observers.Remove(observer);
    }

    public void NotifyObservers()
    {
        foreach (var observer in observers)
        {
            observer.Update(latestTemperature);
        }
    }

    public void GenerateTemperature()
    {
        latestTemperature = (float)(random.NextDouble() * 40 - 10);
        NotifyObservers();
    }

    public void SetTemperatureUnit(ITemperatureConverter converter)
    {
        temperatureContext.SetConverter(converter);
    }

    public float GetConvertedTemperature()
    {
        return temperatureContext.GetConvertedTemperature(latestTemperature);
    }
}