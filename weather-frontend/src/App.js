import React, { useState, useEffect } from "react";
import WeatherDisplay from "./WeatherDisplay";
import TemperatureHistory from "./TemperatureHistory";
import TemperatureChart from "./TemperatureChart";

const API_BASE_URL = "https://localhost:7113/api/weatherstation"; // Pas dit aan indien nodig

function App() {
  const [mode, setMode] = useState("Day");

  useEffect(() => {
    // Haal huidige modus op van de backend
    fetch(`${API_BASE_URL}/mode`)
      .then((res) => res.json())
      .then((data) => {
        setMode(data.mode);
        document.body.classList.remove("night-mode", "day-mode");
        document.body.classList.add(
          data.mode === "Night" ? "night-mode" : "day-mode"
        );
      })
      .catch((err) => console.error("Fout bij ophalen modus:", err));
  }, []);

  const toggleMode = async () => {
    const newMode = mode === "Day" ? "Night" : "Day";

    // ✅ Zet direct de nieuwe mode in de state voordat de fetch is afgerond
    setMode(newMode);
    document.body.classList.remove("night-mode", "day-mode");
    document.body.classList.add(
      newMode === "Night" ? "night-mode" : "day-mode"
    );

    try {
      // ✅ Verstuur de nieuwe mode naar de backend
      const response = await fetch(`${API_BASE_URL}/mode`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ mode: newMode }),
      });

      if (!response.ok) {
        throw new Error("Fout bij updaten modus.");
      }

      console.log("Modus succesvol bijgewerkt naar:", newMode);
    } catch (error) {
      console.error("Fout bij updaten modus:", error);
    }
  };

  console.log("Huidige modus:", mode);

  return (
    <div className="App">
      <header>
        <h1>Weerstation</h1>
        <button onClick={toggleMode}>
          Schakel naar {mode === "Day" ? "Nachtmodus" : "Dagmodus"}
        </button>
      </header>

      <main>
        <WeatherDisplay />
        <div className="container">
          <div className="table-container">
            <TemperatureHistory />
          </div>
          <div className="chart-container">
            <TemperatureChart />
          </div>
        </div>
      </main>
    </div>
  );
}

export default App;
