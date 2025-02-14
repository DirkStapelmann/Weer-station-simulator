namespace WeerStationSimulator
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public interface IObserver
    {
        void Update(float temperature);
    }

    public class WeatherStation
    {
        private static WeatherStation _instance;
        private static readonly object _lock = new object();
        private List<IObserver> observers = new List<IObserver>();
        private Random random = new Random();
        private float latestTemperature;

        private WeatherStation() { }

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

        public float GetLatestTemperature()
        {
            return latestTemperature;
        }
    }

    public class ConsoleDisplay : IObserver
    {
        public void Update(float temperature)
        {
            Console.WriteLine($"[Console] Huidige temperatuur: {temperature}°C");
        }
    }

    public class FileLogger : IObserver
    {
        private string filePath = "temperature_log.txt";

        public void Update(float temperature)
        {
            File.AppendAllText(filePath, $"[{DateTime.Now}] Temp: {temperature}°C\n");
        }
    }

}
