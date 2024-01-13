using System.Text;
using System.Text.Json;
using Azure.Messaging.ServiceBus;

namespace ICAP_ServiceBus
{
    public class BusPublisher(string connectionString) : IBusPublisher
    {
        public async Task SendMessageAsync<T>(T serviceBusMessage, string topicName)
        {
            var client = new ServiceBusClient(connectionString, new ServiceBusClientOptions
            {
                ConnectionIdleTimeout = TimeSpan.FromMinutes(5),
            });
            var sender = client.CreateSender(topicName);
            var messageBody = JsonSerializer.Serialize(serviceBusMessage);
            var message = new ServiceBusMessage(Encoding.UTF8.GetBytes(messageBody));
            await sender.SendMessageAsync(message);
        }
    }
}
