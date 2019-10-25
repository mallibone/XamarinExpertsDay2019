using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Reactive.Subjects;
using System.Reactive.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.IO;

namespace Mobile.IoTMonitor
{
    class MeasurementService : IMeasurementService
    {
        private HubConnection _connection;
        private readonly HttpClient _httpClient;
        private const string backendUrl = "https://iotdemofunction.azurewebsites.net/api/";
        //private const string backendUrl = "http://localhost:7071/api/";
        //public event EventHandler<Measurement> NewMeasurement;
        private Subject<Measurement> _newMeasurement = new Subject<Measurement>();

        public MeasurementService()
        {
            _connection = new HubConnectionBuilder()
                .WithUrl(backendUrl)
                .Build();
            _httpClient = new HttpClient();
        }

        public IObservable<Measurement> NewMeasurement => _newMeasurement.AsObservable();

        public async Task Connect()
        {
            if (_connection.State == HubConnectionState.Connected) return;

            _connection.On<string>("measurement", (messageString) =>
            {
                var messages = JsonConvert.DeserializeObject<IEnumerable<Measurement>>(messageString);
                foreach (var message in messages)
                {
                    _newMeasurement.OnNext(message);
                }
                Debug.WriteLine(messageString);
            });

            try
            {
                await _connection.StartAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public async Task Disconnect()
        {
            await _connection.DisposeAsync();

            _connection = new HubConnectionBuilder()
                .WithUrl(backendUrl)
                .Build();
        }

        public async void UpdateColor(MyColor myColor)
        {
            var serializedPayload = JsonConvert.SerializeObject(myColor);

            await _httpClient.PostAsync(Path.Combine(backendUrl, "UpdateColor"), new StringContent(serializedPayload));
        }
    }
}
