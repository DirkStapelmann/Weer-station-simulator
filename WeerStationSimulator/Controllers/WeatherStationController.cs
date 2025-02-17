using Microsoft.AspNetCore.Mvc;
using Weer_station_simulator.Models;

namespace Weer_station_simulator.Controllers
{
    [ApiController]
    [Route("api/weatherstation")]
    public class WeatherStationController : ControllerBase
    {
        private readonly WeatherStationFacade _weatherStationFacade;

        public WeatherStationController(WeatherStationFacade weatherStationFacade)
        {
            _weatherStationFacade = weatherStationFacade;
        }

        [HttpGet("latest")]
        public ActionResult<TemperatureReading> GetLatestTemperature([FromQuery] string unit = "C")
        {
            var latestReading = _weatherStationFacade.GetTemperatureHistory().FirstOrDefault();

            if (latestReading == null)
            {
                return NotFound("Geen temperatuurdata beschikbaar.");
            }

            // Gebruik TemperatureContext voor conversie
            ITemperatureConverter converter = unit.ToUpper() switch
            {
                "F" => new FahrenheitConverter(),
                "K" => new KelvinConverter(),
                _ => new CelsiusConverter()
            };

            var context = new TemperatureContext(converter);
            float convertedTemperature = context.GetConvertedTemperature(latestReading.Temperature);

            return Ok(new TemperatureReading
            {
                Id = latestReading.Id,
                Temperature = convertedTemperature,
                Unit = unit.ToUpper(),
                Timestamp = latestReading.Timestamp
            });
        }

        [HttpPost("generate")]
        public ActionResult GenerateNewTemperature([FromQuery] string unit = "C")
        {
            // Controleer of de eenheid geldig is
            if (unit.ToUpper() != "C" && unit.ToUpper() != "F" && unit.ToUpper() != "K")
            {
                return BadRequest("Ongeldige eenheid. Gebruik 'C', 'F' of 'K'.");
            }

            // Roep de facade aan om de temperatuur te genereren en op te slaan
            _weatherStationFacade.GenerateAndSaveTemperature(unit);

            return Ok($"Nieuwe temperatuur gegenereerd en opgeslagen in {unit.ToUpper()}.");
        }

        [HttpGet("history")]
        public ActionResult<IEnumerable<TemperatureReading>> GetTemperatureHistory([FromQuery] int limit = 10)
        {
            limit = Math.Clamp(limit, 1, 100); // Limiet tussen 1 en 100 om overbelasting te voorkomen

            var history = _weatherStationFacade.GetTemperatureHistory()
                .Take(limit)
                .ToList();

            return Ok(history);
        }

        [HttpGet("sensor/status")]
        public ActionResult<string> GetSensorStatus()
        {
            return Ok(_weatherStationFacade.GetSensorStatus());
        }

        [HttpPost("sensor/status")]
        public ActionResult SetSensorStatus([FromBody] SensorStatusRequest request)
        {
            try
            {
                _weatherStationFacade.SetSensorStatus(request.Status);
                return Ok($"Sensorstatus gewijzigd naar: {request.Status}");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Hulpklasse voor JSON-requestbody
        public class SensorStatusRequest
        {
            public string Status { get; set; } = "active";
        }
    }
}
