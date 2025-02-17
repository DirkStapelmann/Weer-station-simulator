using Microsoft.AspNetCore.Mvc;
using Weer_station_simulator.Models;

namespace Weer_station_simulator.Controllers
{
    // Controller voor het beheren van temperatuurmetingen en sensorstatus via API-endpoints.
    [ApiController]
    [Route("api/weatherstation")]
    public class WeatherStationController : ControllerBase
    {
        private readonly WeatherStationFacade _weatherStationFacade;

        // Gebruik van het Facade Pattern om interactie met complexe logica te vereenvoudigen.
        public WeatherStationController(WeatherStationFacade weatherStationFacade)
        {
            _weatherStationFacade = weatherStationFacade;
        }

        // Haalt de meest recente temperatuurmeting op en converteert deze naar de gewenste eenheid.
        [HttpGet("latest")]
        public ActionResult<TemperatureReading> GetLatestTemperature([FromQuery] string unit = "C")
        {
            var latestReading = _weatherStationFacade.GetTemperatureHistory().FirstOrDefault();

            if (latestReading == null)
            {
                return NotFound("Geen temperatuurdata beschikbaar.");
            }

            // Strategy Pattern: Kiest een converterstrategie op basis van de opgegeven eenheid.
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

        // Genereert en slaat een nieuwe temperatuurmeting op via de Facade.
        [HttpPost("generate")]
        public ActionResult GenerateNewTemperature([FromQuery] string unit = "C")
        {
            if (unit.ToUpper() != "C" && unit.ToUpper() != "F" && unit.ToUpper() != "K")
            {
                return BadRequest("Ongeldige eenheid. Gebruik 'C', 'F' of 'K'.");
            }

            _weatherStationFacade.GenerateAndSaveTemperature(unit);
            return Ok($"Nieuwe temperatuur gegenereerd en opgeslagen in {unit.ToUpper()}.");
        }

        // Haalt de temperatuurhistorie op met een limiet om overbelasting te voorkomen.
        [HttpGet("history")]
        public ActionResult<IEnumerable<TemperatureReading>> GetTemperatureHistory([FromQuery] int limit = 10)
        {
            limit = Math.Clamp(limit, 1, 100);

            var history = _weatherStationFacade.GetTemperatureHistory()
                .Take(limit)
                .ToList();

            return Ok(history);
        }

        // Haalt de status van de temperatuursensor op.
        [HttpGet("sensor/status")]
        public ActionResult<string> GetSensorStatus()
        {
            return Ok(_weatherStationFacade.GetSensorStatus());
        }

        // Wijzigt de sensorstatus via een JSON-request.
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

        // Hulpklasse voor het ontvangen van sensorstatus-updates via JSON.
        public class SensorStatusRequest
        {
            public string Status { get; set; } = "active"; // Standaardwaarde voorkomt null-errors.
        }
    }
}
