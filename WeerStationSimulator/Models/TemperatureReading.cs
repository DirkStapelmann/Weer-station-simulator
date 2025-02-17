using System;
using System.ComponentModel.DataAnnotations;

namespace Weer_station_simulator.Models
{
    // Modelklasse die een temperatuurmeting vertegenwoordigt.
    // Wordt gebruikt voor opslag in de database via Entity Framework.
    public class TemperatureReading
    {
        [Key] // Primaire sleutel voor database-identificatie.
        public int Id { get; set; }

        // De temperatuurwaarde in de opgegeven eenheid.
        public float Temperature { get; set; }

        // De meeteenheid (Celsius, Fahrenheit, Kelvin), standaard "C".
        public string Unit { get; set; } = "C";

        // Tijdsregistratie van de meting, standaard UTC.
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
