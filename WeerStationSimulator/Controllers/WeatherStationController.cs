using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Weer_station_simulator.Data;
using Weer_station_simulator.Models; // Zorg ervoor dat je de modellen importeert

namespace Weer_station_simulator.Controllers
{
    [ApiController]
    [Route("api/weatherstation")]
    public class WeatherStationController : ControllerBase
    {
        private readonly WeatherDbContext _dbContext;

        public WeatherStationController(WeatherDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("latest")]
        public ActionResult<TemperatureReading> GetLatestTemperature([FromQuery] string unit = "C")
        {
            var latestReading = _dbContext.TemperatureReadings
                .OrderByDescending(t => t.Timestamp)
                .FirstOrDefault();

            if (latestReading == null)
            {
                Console.WriteLine("Geen temperatuurdata beschikbaar.");
                return NotFound("Geen temperatuurdata beschikbaar.");
            }

            // Toepassen van temperatuurconversie
            float convertedTemperature = latestReading.Temperature;

            switch (unit.ToUpper())
            {
                case "F":
                    convertedTemperature = (latestReading.Temperature * 9 / 5) + 32;
                    break;
                case "K":
                    convertedTemperature = latestReading.Temperature + 273.15f;
                    break;
            }

            Console.WriteLine($"Returning latest temperature: {convertedTemperature} {unit}");

            return Ok(new TemperatureReading
            {
                Id = latestReading.Id,
                Temperature = convertedTemperature,
                Unit = unit,
                Timestamp = latestReading.Timestamp
            });
        }


        [HttpPost("generate")]
        public ActionResult GenerateNewTemperature()
        {
            var random = new Random();
            float newTemperature = (float)(random.NextDouble() * 40 - 10);

            var newReading = new TemperatureReading
            {
                Temperature = newTemperature,
                Unit = "C",
                Timestamp = DateTime.UtcNow
            };

            _dbContext.TemperatureReadings.Add(newReading);
            _dbContext.SaveChanges();

            return Ok("Nieuwe temperatuur gegenereerd en opgeslagen.");
        }

        [HttpGet("history")]
        public ActionResult<IEnumerable<TemperatureReading>> GetTemperatureHistory()
        {
            var history = _dbContext.TemperatureReadings
                .OrderByDescending(t => t.Timestamp)
                .Take(10)
                .ToList();

            return Ok(history);
        }
    }
}
