using System;
using System.ComponentModel.DataAnnotations;

namespace Weer_station_simulator.Models  // ✅ Zorg dat deze namespace correct is!
{
    public class TemperatureReading
    {
        [Key]
        public int Id { get; set; }

        public float Temperature { get; set; }

        public string Unit { get; set; } = "C";

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
