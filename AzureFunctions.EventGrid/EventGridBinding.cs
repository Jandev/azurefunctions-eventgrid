using Microsoft.Azure.WebJobs.Description;
using Microsoft.Azure.WebJobs.Host.Config;

namespace AzureFunctions.EventGrid
{
    [Extension(nameof(EventGridBinding))]
    public class EventGridBinding : IExtensionConfigProvider
    {
        public void Initialize(ExtensionConfigContext context)
        {
            var ruleCommand = context.AddBindingRule<EventGridAttribute>();
            ruleCommand.BindToCollector(attribute => new EventGridAsyncCollector(attribute));
        }
    }
}
