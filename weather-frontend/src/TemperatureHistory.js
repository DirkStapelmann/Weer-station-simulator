import React, { useEffect, useState } from "react";

const API_URL = "https://localhost:7113/api/weatherstation/history"; // ✅ Pas aan als je een andere poort gebruikt

const TemperatureHistory = () => {
  const [history, setHistory] = useState([]);

  useEffect(() => {
    fetch(API_URL)
      .then((response) => response.json())
      .then((data) => setHistory(data))
      .catch((error) => console.error("Error fetching history:", error));
  }, []);

  return (
    <div>
      <h2>Historische Metingen</h2>
      <table border="1">
        <thead>
          <tr>
            <th>Temperatuur (°C)</th>
            <th>Eenheid</th>
            <th>Tijdstip</th>
          </tr>
        </thead>
        <tbody>
          {history.map((entry, index) => (
            <tr key={index}>
              <td>{entry.temperature}</td>
              <td>{entry.unit}</td>
              <td>{new Date(entry.timestamp).toLocaleString()}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default TemperatureHistory;
