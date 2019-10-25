using System;
using System.Linq;
using System.Threading.Tasks;

namespace Mobile.IoTMonitor
{
    interface IMeasurementService
    {
        IObservable<Measurement> NewMeasurement { get; }

        Task Connect();
        Task Disconnect();
        void UpdateColor(MyColor myColor);
    }
}