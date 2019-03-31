using System;
using Microsoft.Azure.WebJobs.Description;

namespace AzureFunctions.EventGrid
{
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.ReturnValue)]
    [Binding]
    public class EventGridAttribute : Attribute
    {
        /// <summary>
        /// The Topic endpoint on the Event Grid where you want to send an event to.
        /// This endpoint should be resolved from the configuration.
        /// </summary>
        [AppSetting]
        public string TopicEndpoint { get; set; }

        /// <summary>
        /// The Topic Key which you want to use to send an event to Event Grid.
        /// This key should be resolved from the configuration.
        /// </summary>
        [AppSetting]
        public string TopicKey { get; set; }
    }
}
