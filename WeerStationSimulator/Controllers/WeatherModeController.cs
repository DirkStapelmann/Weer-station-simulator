using Microsoft.AspNetCore.Mvc;
using Weer_station_simulator.Models;

namespace Weer_station_simulator.Controllers
{
    [Route("api/weatherstation")]
    [ApiController]
    public class WeatherModeController : ControllerBase
    {
        private readonly WeatherModeContext _modeContext;

        // Gebruik Dependency Injection voor WeatherModeContext
        public WeatherModeController(WeatherModeContext modeContext)
        {
            _modeContext = modeContext;
        }

        [HttpGet("mode")]
        public IActionResult GetMode()
        {
            return Ok(new { mode = _modeContext.GetMode() });
        }

        [HttpPost("mode")]
        public IActionResult SetMode([FromBody] ModeRequest request)
        {
            if (request.Mode != "Night" && request.Mode != "Day")
            {
                return BadRequest("Ongeldige modus. Gebruik 'Night' of 'Day'.");
            }

            _modeContext.SetMode(request.Mode == "Night" ? new NightMode() : new DayMode());

            return Ok(new { mode = _modeContext.GetMode() });
        }
    }

    public class ModeRequest
    {
        public string Mode { get; set; } = string.Empty; // Standaardwaarde voorkomt waarschuwing
    }
}
