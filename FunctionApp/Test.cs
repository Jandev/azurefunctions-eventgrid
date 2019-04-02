using System.IO;
using System.Threading.Tasks;
using AzureFunctions.EventGrid;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FunctionApp
{
    public static class Test
    {
        [FunctionName("Test")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            [EventGrid(
                TopicEndpoint = "EventGridBindingSampleTopicEndpoint", 
                TopicKey = "EventGridBindingSampleTopicKey")] 
            IAsyncCollector<Event> outputCollector,
            ILogger log)
        {
            log.LogInformation("Executing the Test function");
            
            var customEvent = new MyCustomEvent
            {
                Identifier = 1,
                Name = "Jan",
                Product = "Azure Functions"
            };
            var myTestEvent = new Event
            {
                EventType = nameof(MyCustomEvent),
                Subject = "Jandev/Samples/CustomTestEvent",
                Data = customEvent
            };
            await outputCollector.AddAsync(myTestEvent);

            log.LogInformation("Executed the Test function");

            return new OkObjectResult($"Sending {customEvent.Identifier}.");
        }

        private class MyCustomEvent
        {
            public int Identifier { get; set; }
            public string Name { get; set; }
            public string Product { get; set; }
        }
    }
}
