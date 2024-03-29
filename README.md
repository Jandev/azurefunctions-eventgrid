# Azure Functions Event Grid binding

An easy to use Azure Functions output binding for Azure Event Grid.

## Moving on

When I created this package, there wasn't an Event Grid binding available for Azure Functions. Nowadays, there is one provided to you by the team. I recommend you start using the binding which is supported by the team in favor of this one. [More information can be found in the docs.](https://docs.microsoft.com/en-us/azure/azure-functions/functions-bindings-event-grid?tabs=in-process%2Cextensionv3&pivots=programming-language-csharp).

## Badges

[![Code Smells](https://sonarcloud.io/api/project_badges/measure?project=azurefunctions-eventgrid&metric=code_smells)](https://sonarcloud.io/dashboard?id=azurefunctions-eventgrid)
[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=azurefunctions-eventgrid&metric=sqale_rating)](https://sonarcloud.io/dashboard?id=azurefunctions-eventgrid)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=azurefunctions-eventgrid&metric=alert_status)](https://sonarcloud.io/dashboard?id=azurefunctions-eventgrid)
[![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=azurefunctions-eventgrid&metric=security_rating)](https://sonarcloud.io/dashboard?id=azurefunctions-eventgrid)
[![Technical Debt](https://sonarcloud.io/api/project_badges/measure?project=azurefunctions-eventgrid&metric=sqale_index)](https://sonarcloud.io/dashboard?id=azurefunctions-eventgrid)
[![Vulnerabilities](https://sonarcloud.io/api/project_badges/measure?project=azurefunctions-eventgrid&metric=vulnerabilities)](https://sonarcloud.io/dashboard?id=azurefunctions-eventgrid)

## Download

The compiled version can be downloaded via NuGet (https://www.nuget.org/packages/AzureFunctions.EventGridBinding/), so you can use it in your project.

## Usage

A sample on how you can use the output binding is as follows.

```csharp
[FunctionName("Test")]
public static async Task<IActionResult> Run(
	[HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
	[EventGrid(
		// The endpoint of your Event Grid Topic, this should be specified in your application settings of the Function App
		TopicEndpoint = "EventGridBindingSampleTopicEndpoint",
		// This is the secret key to connect to your Event Grid Topic. To be placed in the application settings.
		TopicKey = "EventGridBindingSampleTopicKey")]
	IAsyncCollector<Event> outputCollector,
	ILogger log)
{
	log.LogInformation("Executing the Test function");

	// Create the actual `Data` object you want to publish to Event Grid
	var customEvent = new MyCustomEvent
	{
		Identifier = 1,
		Name = "Jan",
		Product = "Azure Functions"
	};
	// Specify some meta data of the message you want to publish to Event Grid
	var myTestEvent = new Event
	{
		EventType = nameof(MyCustomEvent),
		Subject = "Jandev/Samples/CustomTestEvent",
		Data = customEvent
	};

	// Add the event to the IAsyncCollector<T> in order to get your event published.
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
```

The publishing of the events will be executed after the Azure Function is finished,
in the `FlushAsync` method of the `IAsyncCollector<T>`.
