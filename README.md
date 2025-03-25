# **Project Start**

### netstat -ano | findstr :7113

Doe dit nadat je de C# start om te checken of die aan staat. Start C# project in Visual Studio als je dotnet run probeert doet hij het niet.
je kan ook https://localhost:7113/api/weatherstation/latest openen om te kijken of hij daar loopt.

### `npm start`

Runs the app in the development mode.\
Open [http://localhost:3000](http://localhost:3000) to view it in your browser.

The page will reload when you make changes.\
You may also see any lint errors in the console.

### `npm run build`

Builds the app for production to the `build` folder.\
It correctly bundles React in production mode and optimizes the build for the best performance.

The build is minified and the filenames include the hashes.\
Your app is ready to be deployed!

# **Design Patterns in het Weerstationproject**

## **1. Inleiding**

Dit project maakt gebruik van verschillende **design patterns** om de code **modulair, uitbreidbaar en onderhoudbaar** te houden. Hieronder wordt uitgelegd welke patterns zijn gebruikt, waarom ze zijn gekozen en hoe ze in de code zijn geïmplementeerd.

---

## **2. Overzicht van gebruikte design patterns**

| **Design Pattern**     | **Aantal keer gebruikt** | **Toepassing in de code**                                                                                    |
| ---------------------- | ------------------------ | ------------------------------------------------------------------------------------------------------------ |
| **State Pattern**      | 3x                       | **WeatherModeContext, ISensorState, SensorContext** beheert toestanden zoals dag/nachtmodus en sensorstatus. |
| **Strategy Pattern**   | 2x                       | **WeatherStation & WeatherStationFacade** gebruiken strategieën voor temperatuurconversie.                   |
| **Singleton Pattern**  | 2x                       | **WeatherStation & Program.cs** zorgen ervoor dat bepaalde klassen slechts één keer worden geïnstantieerd.   |
| **Observer Pattern**   | 1x                       | **WeatherStation & IObserver** laten observatoren reageren op temperatuurveranderingen.                      |
| **Facade Pattern**     | 1x                       | **WeatherStationFacade** biedt een vereenvoudigde interface voor complexe logica.                            |
| **Repository Pattern** | 1x                       | **WeatherDbContext & TemperatureReading** beheren databasebewerkingen.                                       |

---

## **3. Uitleg per design pattern**

### **3.1 State Pattern (3x) – Beheren van toestanden**

**Waarom gekozen?**  
Het **State Pattern** wordt gebruikt om dynamisch te wisselen tussen verschillende toestanden zonder complexe `if-else` structuren.

**Waarom dit pattern (en niet iets anders)?**  
Toestanden zoals dag/nachtmodus of sensorstatus kunnen voortdurend veranderen. In plaats van een complexe wirwar van `if-else` of `switch`-statements te onderhouden, biedt het **State Pattern** een **georganiseerde en schaalbare** manier om gedrag per toestand onder te brengen in aparte klassen. Een alternatief, zoals het Strategy Pattern, zou niet voldoende zijn omdat het bij state gaat om **interne toestandsverandering** met blijvende effecten, terwijl strategieën meer **uitwisselbare algoritmes zonder geheugen** zijn.

> **Kortom:** het State Pattern is hier de beste keuze omdat het draait om _gedrag dat afhangt van de interne toestand van een object_ en deze toestand ook _kan veranderen_.

---

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

**Effect:** De modus van het weerstation kan dynamisch veranderen zonder afhankelijk te zijn van `if-else` structuren.

---

### **3.2 Strategy Pattern (2x) – Flexibele temperatuurconversie**

**Waarom gekozen?**  
Het **Strategy Pattern** maakt het mogelijk om **verschillende algoritmen** uitwisselbaar te gebruiken, zoals temperatuurconversies.

**Waarom dit pattern (en niet iets anders)?**  
Er zijn meerdere algoritmen (Celsius, Fahrenheit, Kelvin) die allemaal op dezelfde manier aangeroepen worden, maar verschillend werken. Deze algoritmes kunnen ten opzichte van elkaar gewisseld worden — precies waarvoor het **Strategy Pattern** bedoeld is. Een alternatief zoals het Factory Pattern zou eerder gaan over **objectcreatie**, terwijl het hier juist gaat om **gedragsvariatie op basis van invoer**.

> **Kortom:** het Strategy Pattern past hier perfect omdat je _keuzevrijheid tussen meerdere algoritmes_ wilt zonder `if-else`-ketens en mét behoud van uitbreidbaarheid.

---

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

**Effect:** De applicatie kan eenvoudig **nieuwe conversie-algoritmen** toevoegen zonder bestaande code te wijzigen.

---

### **3.3 Singleton Pattern (2x) – Één instantie van de klasse**

**Waarom gekozen?**  
Het **Singleton Pattern** zorgt ervoor dat er **slechts één instantie** van een klasse wordt gemaakt, wat geheugen bespaart en consistentie garandeert.

**Waarom dit pattern (en niet iets anders)?**  
Voor objecten zoals `WeatherStation`, die _de enige bron van waarheid_ moeten zijn binnen het systeem, is het essentieel dat er slechts één instantie bestaat. In plaats van dit handmatig te beheren (wat foutgevoelig is), zorgt het **Singleton Pattern** automatisch voor **instantiebeheer**. Andere patronen zoals het Repository Pattern of Facade Pattern hebben hier geen controle over het aantal instanties.

> **Kortom:** het Singleton Pattern is gekozen omdat _consistentie en geheugenbeheer_ cruciaal zijn voor centrale componenten zoals het weerstation.

---

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

**Effect:** Voorkomt dat er per ongeluk **meerdere instanties** van WeatherStation worden aangemaakt.

---

### **3.4 Observer Pattern (1x) – Automatische updates bij temperatuurverandering**

**Waarom gekozen?**  
Het **Observer Pattern** wordt gebruikt om meerdere objecten op de hoogte te brengen van veranderingen in een **subject** (WeatherStation).

**Waarom dit pattern (en niet iets anders)?**  
Wanneer meerdere onderdelen van het systeem (bijvoorbeeld UI-componenten, logs, database-updates) op de hoogte moeten worden gebracht van temperatuurveranderingen, is het **Observer Pattern** ideaal. Een alternatief zou zijn om handmatig callbacks aan te roepen, maar dat leidt tot **hoge koppeling** tussen klassen. Observer biedt juist een **losgekoppelde structuur**, wat onderhoud eenvoudiger maakt.

> **Kortom:** het Observer Pattern is gekozen omdat het _loskoppelt_ wie op de hoogte wordt gesteld, wat leidt tot een schaalbaar en uitbreidbaar notificatiesysteem.

---

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

**Effect:** Observatoren worden **automatisch op de hoogte gebracht** zonder directe afhankelijkheden.

---

### **3.5 Facade Pattern (1x) – Vereenvoudigde toegang tot complexe systemen**

**Waarom gekozen?**  
Het **Facade Pattern** verbergt de complexiteit van meerdere klassen achter één **eenvoudige interface**.

**Waarom dit pattern (en niet iets anders)?**  
De Facade biedt een eenvoudige interface voor complexe logica: meerdere klassen (zoals `WeatherStation`, `DbContext`, converters) worden op één plek samengebracht. Je zou dit ook via Dependency Injection kunnen combineren zonder facade, maar dan moet de controller alsnog veel weten over interne structuren. Met de **Facade Pattern** abstraheer je dat allemaal weg. Het Adapter Pattern is bijvoorbeeld meer bedoeld voor **interface-aanpassing tussen twee systemen**, wat hier niet van toepassing is.

> **Kortom:** het Facade Pattern is hier logisch om _complexiteit te verbergen_ en _gebruiksgemak te verhogen_ richting de controller.

---

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

**Effect:** Controllers hoeven **geen complexe logica** meer te kennen, ze roepen alleen **WeatherStationFacade** aan.

---

### **3.6 Repository Pattern (1x) – Gestructureerde database-opslag**

**Waarom gekozen?**  
Het **Repository Pattern** verbergt de database-operaties en voorkomt directe afhankelijkheid van Entity Framework in controllers.

**Waarom dit pattern (en niet iets anders)?**  
De Repository Pattern is gekozen om een **abstractielaag tussen database en businesslogica** te creëren. Zonder dit pattern zou je Entity Framework direct vanuit controllers gebruiken, wat zorgt voor een sterke koppeling. Een alternatief zoals het DAO Pattern lijkt erop, maar richt zich vaker op **enkelvoudige objecten**, terwijl Repository ideaal is voor domeinlogica met query’s, filters en unit-of-work.

> **Kortom:** de Repository Pattern is gekozen om _data-opslag te isoleren van logica_, waardoor testen, onderhoud en vervangbaarheid sterk verbeteren.

---

**Waar toegepast?**

- `WeatherDbContext.cs` beheert de opslag van temperatuurmetingen in de database.

**Codevoorbeeld (WeatherDbContext.cs)**

```csharp
public class WeatherDbContext : DbContext
{
    public DbSet<TemperatureReading> TemperatureReadings { get; set; }
}
```

**Effect:** Controllers hoeven **geen directe SQL-query's** uit te voeren, ze werken met `WeatherDbContext`.

---

## **4. Conclusie**

Dit project maakt **effectief gebruik van 5+ design patterns** om de code **modulair, schaalbaar en onderhoudbaar** te houden. Elk patroon is zorgvuldig gekozen om een specifiek probleem op te lossen, zoals **state management, flexibele strategieën, automatische updates, een enkele instantie en eenvoudiger beheer van complexe systemen**.
