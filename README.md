Hier is de volledige documentatie over de **design patterns** in jouw project, gepresenteerd in **Canvas-formaat**. 📜✨  

---

# **📌 Design Patterns in het Weerstationproject**  

## **1. Inleiding**  
Dit project maakt gebruik van verschillende **design patterns** om de code **modulair, uitbreidbaar en onderhoudbaar** te houden. Hieronder wordt uitgelegd welke patterns zijn gebruikt, waarom ze zijn gekozen en hoe ze in de code zijn geïmplementeerd.  

---

## **2. Overzicht van gebruikte design patterns**  

| **Design Pattern**  | **Aantal keer gebruikt** | **Toepassing in de code** |
|---------------------|----------------------|--------------------------------|
| **State Pattern** 🏁 | 3x  | **WeatherModeContext, ISensorState, SensorContext** beheert toestanden zoals dag/nachtmodus en sensorstatus. |
| **Strategy Pattern** 🎯 | 2x  | **WeatherStation & WeatherStationFacade** gebruiken strategieën voor temperatuurconversie. |
| **Singleton Pattern** 🔁 | 2x  | **WeatherStation & Program.cs** zorgen ervoor dat bepaalde klassen slechts één keer worden geïnstantieerd. |
| **Observer Pattern** 👀 | 1x  | **WeatherStation & IObserver** laten observatoren reageren op temperatuurveranderingen. |
| **Facade Pattern** 🏛️ | 1x  | **WeatherStationFacade** biedt een vereenvoudigde interface voor complexe logica. |
| **Repository Pattern** 💾 | 1x  | **WeatherDbContext & TemperatureReading** beheren databasebewerkingen. |

---

## **3. Uitleg per design pattern**  

### **📌 3.1 State Pattern (3x) – Beheren van toestanden**  
**Waarom gekozen?**  
Het **State Pattern** wordt gebruikt om dynamisch te wisselen tussen verschillende toestanden zonder complexe `if-else` structuren.  

**Waar toegepast?**  
- `WeatherModeContext` (Dagmodus/Nachtmodus)  
- `SensorContext` (Sensorstatus: actief, fout, offline)  

**Codevoorbeeld (WeatherModeContext.cs)**  
```csharp
public class WeatherModeContext
{
    private IWeatherMode _mode;

    public WeatherModeContext(IWeatherMode mode)
    {
        _mode = mode;
    }

    public void SetMode(IWeatherMode mode)
    {
        _mode = mode;
    }

    public string GetMode()
    {
        return _mode.GetMode();
    }
}
```
📌 **Effect:** De modus van het weerstation kan dynamisch veranderen zonder afhankelijk te zijn van `if-else` structuren.  

---

### **📌 3.2 Strategy Pattern (2x) – Flexibele temperatuurconversie**  
**Waarom gekozen?**  
Het **Strategy Pattern** maakt het mogelijk om **verschillende algoritmen** uitwisselbaar te gebruiken, zoals temperatuurconversies.  

**Waar toegepast?**  
- `WeatherStation.cs` voor conversies tussen **Celsius, Fahrenheit en Kelvin**  
- `WeatherStationFacade.cs` kiest de juiste strategie op basis van gebruikersinvoer  

**Codevoorbeeld (TemperatureContext.cs)**  
```csharp
public interface ITemperatureConverter
{
    float ConvertTemperature(float temperature);
}

public class CelsiusConverter : ITemperatureConverter
{
    public float ConvertTemperature(float temperature) => temperature;
}

public class FahrenheitConverter : ITemperatureConverter
{
    public float ConvertTemperature(float temperature) => (temperature * 9 / 5) + 32;
}

public class TemperatureContext
{
    private ITemperatureConverter _converter;

    public TemperatureContext(ITemperatureConverter converter)
    {
        _converter = converter;
    }

    public float GetConvertedTemperature(float temperature)
    {
        return _converter.ConvertTemperature(temperature);
    }
}
```
📌 **Effect:** De applicatie kan eenvoudig **nieuwe conversie-algoritmen** toevoegen zonder bestaande code te wijzigen.  

---

### **📌 3.3 Singleton Pattern (2x) – Één instantie van de klasse**  
**Waarom gekozen?**  
Het **Singleton Pattern** zorgt ervoor dat er **slechts één instantie** van een klasse wordt gemaakt, wat geheugen bespaart en consistentie garandeert.  

**Waar toegepast?**  
- `WeatherStation.cs` zorgt ervoor dat er maar **één** weerstation is.  
- `Program.cs` maakt **WeatherModeContext** als singleton beschikbaar.  

**Codevoorbeeld (WeatherStation.cs)**  
```csharp
public class WeatherStation
{
    private static WeatherStation? _instance;
    private static readonly object _lock = new object();

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
}
```
📌 **Effect:** Voorkomt dat er per ongeluk **meerdere instanties** van WeatherStation worden aangemaakt.  

---

### **📌 3.4 Observer Pattern (1x) – Automatische updates bij temperatuurverandering**  
**Waarom gekozen?**  
Het **Observer Pattern** wordt gebruikt om meerdere objecten op de hoogte te brengen van veranderingen in een **subject** (WeatherStation).  

**Waar toegepast?**  
- `WeatherStation.cs` bevat een lijst met observatoren die een update krijgen bij **temperatuurveranderingen**.  

**Codevoorbeeld (WeatherStation.cs)**  
```csharp
public class WeatherStation
{
    private List<IObserver> observers = new List<IObserver>();

    public void AddObserver(IObserver observer)
    {
        observers.Add(observer);
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
        latestTemperature = new Random().Next(0, 40);
        NotifyObservers();
    }
}
```
📌 **Effect:** Observatoren worden **automatisch op de hoogte gebracht** zonder directe afhankelijkheden.  

---

### **📌 3.5 Facade Pattern (1x) – Vereenvoudigde toegang tot complexe systemen**  
**Waarom gekozen?**  
Het **Facade Pattern** verbergt de complexiteit van meerdere klassen achter één **eenvoudige interface**.  

**Waar toegepast?**  
- `WeatherStationFacade.cs` maakt het makkelijker om de sensor, database en temperatuurgegevens te beheren.  

**Codevoorbeeld (WeatherStationFacade.cs)**  
```csharp
public class WeatherStationFacade
{
    private WeatherStation _weatherStation;
    private WeatherDbContext _dbContext;

    public WeatherStationFacade(WeatherDbContext dbContext)
    {
        _weatherStation = WeatherStation.GetInstance();
        _dbContext = dbContext;
    }

    public float GetLatestTemperature(ITemperatureConverter converter)
    {
        _weatherStation.SetTemperatureUnit(converter);
        return _weatherStation.GetConvertedTemperature();
    }
}
```
📌 **Effect:** Controllers hoeven **geen complexe logica** meer te kennen, ze roepen alleen **WeatherStationFacade** aan.  

---

### **📌 3.6 Repository Pattern (1x) – Gestructureerde database-opslag**  
**Waarom gekozen?**  
Het **Repository Pattern** verbergt de database-operaties en voorkomt directe afhankelijkheid van Entity Framework in controllers.  

**Waar toegepast?**  
- `WeatherDbContext.cs` beheert de opslag van temperatuurmetingen in de database.  

**Codevoorbeeld (WeatherDbContext.cs)**  
```csharp
public class WeatherDbContext : DbContext
{
    public DbSet<TemperatureReading> TemperatureReadings { get; set; }
}
```
📌 **Effect:** Controllers hoeven **geen directe SQL-query's** uit te voeren, ze werken met `WeatherDbContext`.  

---

## **4. Conclusie**  
Dit project maakt **effectief gebruik van 5+ design patterns** om de code **modulair, schaalbaar en onderhoudbaar** te houden. Elk patroon is zorgvuldig gekozen om een specifiek probleem op te lossen, zoals **state management, flexibele strategieën, automatische updates, een enkele instantie en eenvoudiger beheer van complexe systemen**.  

Wil je nog een **diagram of extra uitleg**? 🚀
