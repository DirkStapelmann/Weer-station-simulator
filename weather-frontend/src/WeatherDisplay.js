import React, { useState, useEffect, useCallback } from "react";

const API_BASE_URL = "https://localhost:7113/api/weatherstation"; // Pas dit aan indien nodig

const WeatherDisplay = () => {
  const [temperature, setTemperature] = useState({ temperature: 0, unit: "C" });
  const [unit, setUnit] = useState("C");

  // Gebruik useCallback om de functie te memoïzeren
  const fetchTemperature = useCallback(async () => {
    try {
      console.log(`Fetching temperature in unit: ${unit}`);
      const response = await fetch(`${API_BASE_URL}/latest?unit=${unit}`);
      const data = await response.json();
      console.log("Received temperature data:", data); // Log ontvangen data
      setTemperature(data);
    } catch (error) {
      console.error("Error fetching temperature:", error);
    }
  }, [unit]);

  const generateNewTemperature = async () => {
    try {
      await fetch(`${API_BASE_URL}/generate`, { method: "POST" });
      await fetchTemperature(); // ✅ Direct temperatuur opnieuw ophalen
    } catch (error) {
      console.error("Error generating temperature:", error);
    }
  };

  useEffect(() => {
    fetchTemperature();
  }, [fetchTemperature]);

  return (
    <div style={{ textAlign: "center", marginTop: "50px" }}>
      <h1>Weerstation</h1>
      <h2>
        Temperatuur:{" "}
        {temperature
          ? `${temperature.temperature}°${temperature.unit}`
          : "Laden..."}
      </h2>
      <button onClick={generateNewTemperature}>
        Nieuwe temperatuur genereren
      </button>
      <br />
      <br />
      <label>Kies eenheid: </label>
      <select value={unit} onChange={(e) => setUnit(e.target.value)}>
        <option value="C">Celsius (°C)</option>
        <option value="F">Fahrenheit (°F)</option>
        <option value="K">Kelvin (K)</option>
      </select>
    </div>
  );
};

export default WeatherDisplay;
