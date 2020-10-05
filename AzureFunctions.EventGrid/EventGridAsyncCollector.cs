using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Newtonsoft.Json;

namespace AzureFunctions.EventGrid
{
    public class EventGridAsyncCollector : IAsyncCollector<Event>
    {
        private readonly string topicEndPoint;       
        private readonly string topicKey; 

        private readonly IList<Event> eventCollection;
        private readonly EventGridAttribute attribute;
        
        private HttpClient httpClient;


        public EventGridAsyncCollector(EventGridAttribute attribute)
        {
            this.eventCollection = new List<Event>();
            this.topicEndPoint = attribute.TopicEndpoint;
            this.topicKey = attribute.TopicKey;
            this.attribute = attribute;

        }

        /// <summary>
        /// Adds the specified <paramref name="item"/> to a private collection.
        /// </summary>
        /// <param name="item">The event details</param>
        /// <param name="cancellationToken">Cancellation token isn't used.</param>
        /// <returns><see cref="Task.CompletedTask"/>.</returns>
        public Task AddAsync(Event item, CancellationToken cancellationToken = new CancellationToken())
        {
            if (httpClient == null)
            {
                httpClient = attribute.HttpClient ?? new HttpClient();
            }

            this.eventCollection.Add(item);

            return Task.CompletedTask;
        }

        /// <summary>
        /// Will invoke the actual publishing of the collected events to Azure Event Grid.
        /// </summary>
        /// <param name="cancellationToken">Is used when publishing to Azure Event Grid.</param>
        public async Task FlushAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var eventGridEventCollection = CreateEventGridEventCollection();
            if (eventGridEventCollection.Any())
            {
                HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(eventGridEventCollection), Encoding.UTF8, "application/json");
                httpContent.Headers.Add("aeg-sas-key", topicKey);
                await httpClient.PostAsync(topicEndPoint, httpContent, cancellationToken);
            }
        }

        private IList<EventGridModel> CreateEventGridEventCollection()
        {
            var eventGridEventCollection = new List<EventGridModel>();
            foreach (var @event in eventCollection)
            {

                var eventGridEvent = new EventGridModel(
                    id: Guid.NewGuid().ToString("N"),
                    subject: @event.Subject,
                    dataVersion: @event.DataVersion,
                    eventType: @event.EventType,
                    data: @event.Data,
                    eventTime: DateTime.UtcNow
                );
                eventGridEventCollection.Add(eventGridEvent);
            }

            return eventGridEventCollection;
        }
    }
}