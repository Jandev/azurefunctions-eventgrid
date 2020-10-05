using System;

namespace AzureFunctions.EventGrid
{
    class EventGridModel :Event
    {
        //
        // Summary:
        //     Gets or sets an unique identifier for the event.
        public string Id { get; set; }

        //
        // Summary:
        //     Gets or sets the time (in UTC) the event was generated.
        public DateTime EventTime { get; set; }

        public EventGridModel(string id,string subject, string dataVersion, string eventType, object data,DateTime eventTime)
        {
            this.Id = id;
            this.Subject = subject;
            this.DataVersion = dataVersion;
            this.EventType = eventType;
            this.Data = data;
            this.EventTime = eventTime;
        }
    }
}
