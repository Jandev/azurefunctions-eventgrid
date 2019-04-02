using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.EventGrid;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs;

namespace AzureFunctions.EventGrid
{
    public class EventGridAsyncCollector : IAsyncCollector<Event>
    {
        private readonly string topicHostname;
        
        private readonly IList<Event> eventCollection;
        private readonly TopicCredentials topicCredentials;

        public EventGridAsyncCollector(EventGridAttribute attribute)
        {
            this.eventCollection = new List<Event>();
            string topicEndpoint = attribute.TopicEndpoint;
            string topicKey = attribute.TopicKey;

            this.topicHostname = new Uri(topicEndpoint).Host;
            this.topicCredentials = new TopicCredentials(topicKey);
        }

        /// <summary>
        /// Adds the specified <paramref name="item"/> to a private collection.
        /// </summary>
        /// <param name="item">The event details</param>
        /// <param name="cancellationToken">Cancellation token isn't used.</param>
        /// <returns><see cref="Task.CompletedTask"/>.</returns>
        public Task AddAsync(Event item, CancellationToken cancellationToken = new CancellationToken())
        {
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
                using (var eventGridClient = new EventGridClient(topicCredentials))
                {
                    await eventGridClient.PublishEventsAsync(
                        topicHostname,
                        eventGridEventCollection,
                        cancellationToken);
                }
            }
        }

        private IList<EventGridEvent> CreateEventGridEventCollection()
        {
            var eventGridEventCollection = new List<EventGridEvent>();
            foreach (var @event in eventCollection)
            {
                var eventGridEvent = new EventGridEvent(
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