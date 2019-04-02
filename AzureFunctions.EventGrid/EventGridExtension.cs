using System;
using Microsoft.Azure.WebJobs;

namespace AzureFunctions.EventGrid
{
    public static class EventGridExtension
    {
        public static IWebJobsBuilder AddEventGridBinding(this IWebJobsBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.AddExtension<EventGridBinding>();
            return builder;
        }
    }
}
