using AzureFunctions.EventGrid;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;

[assembly: WebJobsStartup(typeof(EventGridStartup))]
namespace AzureFunctions.EventGrid
{
    public class EventGridStartup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            builder.AddEventGridBinding();
        }
    }
}
