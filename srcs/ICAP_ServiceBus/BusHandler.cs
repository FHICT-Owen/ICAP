using System.Text;
using System.Text.Json;
using Azure.Messaging.ServiceBus;

namespace ICAP_ServiceBus
{
    public class BusHandler : IBusHandler
    {
        private readonly ServiceBusClient _client;
        public BusHandler(string connectionString)
        {
            _client = new ServiceBusClient(connectionString);
        }

        public async Task CreateBusHandler<T>(string topicName, string subscriptionName, Func<T, Task> onMessageProcessed)
        {
            var processor = _client.CreateProcessor(topicName, subscriptionName, new ServiceBusProcessorOptions
            {
                AutoCompleteMessages = false,
                MaxConcurrentCalls = 5,
                ReceiveMode = ServiceBusReceiveMode.PeekLock
            });

            processor.ProcessMessageAsync += MessageHandler;
            processor.ProcessErrorAsync += ErrorHandler;
            
            await processor.StartProcessingAsync();

            async Task MessageHandler(ProcessMessageEventArgs args)
            {
                var body = Encoding.UTF8.GetString(args.Message.Body);
                var message = JsonSerializer.Deserialize<T>(body);
                if (message == null)
                {
                    await args.DeadLetterMessageAsync(args.Message, "DeserializationFailed", "Could not deserialize message to type " + typeof(T).Name);
                    return;
                }

                await onMessageProcessed(message);
                await args.CompleteMessageAsync(args.Message);
            }
        }

        private static Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine($"Message handler encountered an exception {args.Exception}.");
            Console.WriteLine("Exception context for troubleshooting:");
            Console.WriteLine($"- Source: {args.ErrorSource}");
            Console.WriteLine($"- Entity Path: {args.EntityPath}");
            return Task.CompletedTask;
        }

    }
}
