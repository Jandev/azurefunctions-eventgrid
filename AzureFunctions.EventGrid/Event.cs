namespace AzureFunctions.EventGrid
{
    /// <summary>
    /// The details of your specific event which you want to send to Azure Event Grid
    /// </summary>
    public class Event
    {
        /// <summary>
        /// The subject you want to use for the event.
        /// </summary>
        /// <example>
        /// MyNamespace/SomeDomain/SomeLogic/ObjectType
        /// </example>
        public string Subject { get; set; }

        /// <summary>
        /// The type of event which is sent.
        /// </summary>
        /// <remarks>
        /// This should be the <code>nameof(YourObject)</code>.
        /// </remarks>
        public string EventType { get; set; }

        /// <summary>
        /// The object (event) you want to send to Event Grid.
        /// </summary>
        /// <remarks>
        /// Make sure this object can be serialized to JSON.
        /// </remarks>
        public object Data { get; set; }

        /// <summary>
        /// The data version for the event.
        /// </summary>
        /// <remarks>Default is 2.0.</remarks>
        public string DataVersion { get; set; } = "2.0";
    }
}