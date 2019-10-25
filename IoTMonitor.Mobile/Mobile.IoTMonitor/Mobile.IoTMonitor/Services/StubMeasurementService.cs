using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using System.Timers;

namespace Mobile.IoTMonitor
{
    class StubMeasurementService : IMeasurementService, IDisposable
    {
        private Timer _timer;
        private const string dummyData = "[{\"temperature\":24.7,\"temperature_unit\":\"C\",\"humidity\":32.5,\"humidity_unit\":\"%\",\"pressure\":975.4,\"pressure_unit\":\"psig\"},{\"temperature\":25.0,\"temperature_unit\":\"C\",\"humidity\":34.5,\"humidity_unit\":\"%\",\"pressure\":975.5,\"pressure_unit\":\"psig\"},{\"temperature\":25.2,\"temperature_unit\":\"C\",\"humidity\":35.5,\"humidity_unit\":\"%\",\"pressure\":975.3,\"pressure_unit\":\"psig\"},{\"temperature\":26.0,\"temperature_unit\":\"C\",\"humidity\":30.5,\"humidity_unit\":\"%\",\"pressure\":975.5,\"pressure_unit\":\"psig\"},{\"temperature\":25.5,\"temperature_unit\":\"C\",\"humidity\":31.5,\"humidity_unit\":\"%\",\"pressure\":975.6,\"pressure_unit\":\"psig\"}]";
        private Lazy<IEnumerable<Measurement>> lazyDummyMeasurements = new Lazy<IEnumerable<Measurement>>(() => JsonConvert.DeserializeObject<IEnumerable<Measurement>>(dummyData));
        private IEnumerable<Measurement> DummyMeasurements => lazyDummyMeasurements.Value;
        private Subject<Measurement> _newMeasurement = new Subject<Measurement>();

        private int dummyIndex = 0;

        public IObservable<Measurement> NewMeasurement => _newMeasurement.AsObservable();

        public StubMeasurementService()
        {
            _timer = new Timer
            {
                AutoReset = true,
                Interval = TimeSpan.FromSeconds(5).TotalMilliseconds,
                Enabled = false
            };
            _timer.Elapsed += RaiseNewMeasurement;
        }

        private void RaiseNewMeasurement(object sender, ElapsedEventArgs e)
        {
            _newMeasurement.OnNext(DummyMeasurements.ElementAt(dummyIndex));
            dummyIndex = (dummyIndex + 1) % DummyMeasurements.Count();
        }

        public Task Connect()
        {
            _timer.Enabled = true;
            return Task.CompletedTask;
        }

        public Task Disconnect()
        {
            _timer.Enabled = false;
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer.Elapsed -= RaiseNewMeasurement;
            _timer.Dispose();
        }

        public void UpdateColor(MyColor myColor)
        {
            
        }
    }
}