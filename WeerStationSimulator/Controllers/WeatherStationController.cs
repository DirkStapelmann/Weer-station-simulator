using Microsoft.AspNetCore.Mvc;

namespace WeerStationSimulator.Controllers
{
    [ApiController]
    [Route("api/weatherstation")]
    public class WeatherStationController : ControllerBase
    {
        private readonly WeatherStation _weatherStation = WeatherStation.GetInstance();

        [HttpGet("latest")] // Haalt de laatste temperatuur op
        public ActionResult<float> GetLatestTemperature()
        {
            return Ok(_weatherStation.GetLatestTemperature());
        }

        [HttpPost("generate")] // Genereert een nieuwe temperatuurwaarde
        public IActionResult GenerateNewTemperature()
        {
            _weatherStation.GenerateTemperature();
            return Ok("Nieuwe temperatuur gegenereerd");
        }
    }

}
