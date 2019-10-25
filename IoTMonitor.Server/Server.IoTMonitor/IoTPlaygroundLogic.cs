using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Net.Http;

namespace Server.IoTMonitor
{
    public static class IoTPlaygroundLogic
    {
        [FunctionName("ProcessIoTMeasurement")]
        public static async Task<HttpResponseMessage> ProcessIoTMeasurement(
            [HttpTrigger(AuthorizationLevel.Anonymous)]HttpRequestMessage req,
            [SignalR(HubName = "IoTPlayground")] IAsyncCollector<SignalRMessage> signalRMessages,
            ILogger log)
        {
            var payload = await req.Content.ReadAsStringAsync();
            log.LogInformation($"function processed a message: {payload}");
            await signalRMessages.AddAsync(
                new SignalRMessage
                {
                    Target = "measurement",
                    Arguments = new[] { payload }
                });

            return req.CreateResponse(HttpStatusCode.OK);
    }

        // SignalR Registration
        [FunctionName("Negotiate")]
        public static SignalRConnectionInfo Negotiate(
            [HttpTrigger(AuthorizationLevel.Anonymous)]HttpRequest req,
            [SignalRConnectionInfo(HubName = "IoTPlayground")] SignalRConnectionInfo connectionInfo)
        {
            // connectionInfo contains an access key token with a name identifier claim set to the authenticated user
            return connectionInfo;
        }
    }
}