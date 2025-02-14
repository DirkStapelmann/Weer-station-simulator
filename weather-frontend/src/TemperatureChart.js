import React, { useEffect, useState, useRef } from "react";
import {
  Chart as ChartJS,
  CategoryScale,
  LinearScale,
  LineElement,
  PointElement,
  Title,
  Tooltip,
  Legend,
  LineController,
} from "chart.js";
import { Line } from "react-chartjs-2";

// Register chart.js components
ChartJS.register(
  CategoryScale,
  LinearScale,
  LineElement,
  PointElement, // Register PointElement
  LineController,
  Title,
  Tooltip,
  Legend
);

const API_BASE_URL = "https://localhost:7113/api/weatherstation";

const TemperatureChart = () => {
  const [history, setHistory] = useState([]);
  const chartRef = useRef(null);

  useEffect(() => {
    // Fetch temperature history
    fetch(`${API_BASE_URL}/history`)
      .then((res) => res.json())
      .then((data) => {
        console.log("Ontvangen temperatuurhistorie:", data);
        setHistory(data);
      })
      .catch((err) =>
        console.error("Fout bij ophalen temperatuurhistorie:", err)
      );

    // Capture the chartRef value in a variable to avoid React ref value changes during cleanup
    const currentChartRef = chartRef.current;

    // Cleanup function to destroy chart before re-rendering
    return () => {
      if (currentChartRef && currentChartRef.chartInstance) {
        currentChartRef.chartInstance.destroy();
      }
    };
  }, []);

  // Prepare chart data
  const chartData = {
    labels: history.map((entry) =>
      new Date(entry.timestamp).toLocaleTimeString()
    ),
    datasets: [
      {
        label: "Temperatuur (°C)",
        data: history.map((entry) => entry.temperature),
        borderColor: "rgb(75, 192, 192)",
        backgroundColor: "rgba(75, 192, 192, 0.2)",
        tension: 0.4,
      },
      {
        label: "Temperatuur (°F)",
        data: history.map((entry) => (entry.temperature * 9) / 5 + 32),
        borderColor: "rgb(255, 99, 132)",
        backgroundColor: "rgba(255, 99, 132, 0.2)",
        tension: 0.4,
      },
      {
        label: "Temperatuur (K)",
        data: history.map((entry) => entry.temperature + 273.15),
        borderColor: "rgb(54, 162, 235)",
        backgroundColor: "rgba(54, 162, 235, 0.2)",
        tension: 0.4,
      },
    ],
  };

  // Chart options
  const chartOptions = {
    responsive: true,
    maintainAspectRatio: false,
    plugins: {
      legend: {
        position: "top",
      },
    },
    scales: {
      x: {
        title: {
          display: true,
          text: "Tijdstip",
        },
      },
      y: {
        title: {
          display: true,
          text: "Temperatuur",
        },
      },
    },
  };

  return (
    <div className="chart-container">
      <h2>Temperatuur Historie</h2>
      <Line
        ref={chartRef} // Reference to the chart instance
        data={chartData}
        options={chartOptions}
      />
    </div>
  );
};

export default TemperatureChart;
