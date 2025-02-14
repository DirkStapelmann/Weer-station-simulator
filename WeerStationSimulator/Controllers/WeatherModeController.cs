using Microsoft.AspNetCore.Mvc;
using Weer_station_simulator.Models;

namespace Weer_station_simulator.Controllers
{
    [Route("api/weatherstation")]
    [ApiController]
    public class WeatherModeController : ControllerBase
    {
        private readonly WeatherModeContext _modeContext;

        public WeatherModeController()
        {
            _modeContext = new WeatherModeContext(new DayMode()); // Standaard naar dagmodus
        }

        [HttpGet("mode")]
        public IActionResult GetMode()
        {
            return Ok(new { mode = _modeContext.GetMode() });
        }

        [HttpPost("mode")]
        public IActionResult SetMode([FromBody] ModeRequest request)
        {
            if (request.Mode == "Night")
                _modeContext.SetMode(new NightMode());
            else
                _modeContext.SetMode(new DayMode());

            return Ok(new { mode = _modeContext.GetMode() });
        }
    }

    public class ModeRequest
    {
        public string Mode { get; set; } = string.Empty; // ✅ Standaardwaarde voorkomt waarschuwing
    }
}
