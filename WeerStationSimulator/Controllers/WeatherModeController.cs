using Microsoft.AspNetCore.Mvc;
using Weer_station_simulator.Models;

namespace Weer_station_simulator.Controllers
{
    // Deze controller behandelt de API-endpoints voor het wijzigen en ophalen van de modus van het weerstation.
    // De controller is gekoppeld aan de route "api/weatherstation".
    [Route("api/weatherstation")]
    [ApiController]
    public class WeatherModeController : ControllerBase
    {
        private readonly WeatherModeContext _modeContext;

        // Constructor gebruikt Dependency Injection om de WeatherModeContext-instantie binnen te halen.
        // Dit maakt het eenvoudiger om de code te testen en voorkomt directe afhankelijkheid van de implementatie.
        public WeatherModeController(WeatherModeContext modeContext)
        {
            _modeContext = modeContext;
        }

        // Haalt de huidige modus van het weerstation op via een GET-request naar "api/weatherstation/mode".
        [HttpGet("mode")]
        public IActionResult GetMode()
        {
            // Stuurt een JSON-object terug met de huidige modus, waardoor de client weet welke modus actief is.
            return Ok(new { mode = _modeContext.GetMode() });
        }

        // Stelt de modus van het weerstation in via een POST-request naar "api/weatherstation/mode".
        [HttpPost("mode")]
        public IActionResult SetMode([FromBody] ModeRequest request)
        {
            // Valideert de input: alleen "Night" en "Day" worden geaccepteerd.
            if (request.Mode != "Night" && request.Mode != "Day")
            {
                return BadRequest("Ongeldige modus. Gebruik 'Night' of 'Day'.");
            }

            // Stelt de juiste modus in, afhankelijk van de ontvangen waarde.
            // Dit maakt gebruik van polymorfisme door NightMode en DayMode als objecten te behandelen.
            _modeContext.SetMode(request.Mode == "Night" ? new NightMode() : new DayMode());

            // Stuurt de nieuwe modus terug ter bevestiging.
            return Ok(new { mode = _modeContext.GetMode() });
        }
    }

    // Klasse voor het ontvangen van JSON-body requests voor de SetMode-functie.
    public class ModeRequest
    {
        // Eigenschap Mode met een standaardwaarde om null-waardes te voorkomen.
        public string Mode { get; set; } = string.Empty;
    }
}
