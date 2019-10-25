using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Console.IoTMonitor
{
    class Program
    {
        static async Task Main(string[] args)
        {
            const string backendUrl = "https://iotdemofunction.azurewebsites.net/api/";
            //const string backendUrl = "http://localhost:7071/api/";

            // Connect to SignalR Service
            var connect = new HubConnectionBuilder()
                .WithUrl(backendUrl)
                .Build();

            // Attach Message handler
            connect.On<string>("measurement", (messageString) =>
            {
                var message = JsonConvert.DeserializeObject<IEnumerable<Measurement>>(messageString);
                System.Console.WriteLine("Message Received: " + messageString);
            });

            // Start Monitoring
            await connect.StartAsync();

            // User input handling

            System.Console.WriteLine("...");
            System.Console.ReadLine();
            await connect.DisposeAsync();
        }
    }

    public class Measurement
    {
        public float Temperature { get; set; }
        public string TemperatureUnit { get; set; }
        public float Humidity { get; set; }
        public string HumidityUnit { get; set; }
        public float Pressure { get; set; }
        public string PressureUnit { get; set; }
    }

}
