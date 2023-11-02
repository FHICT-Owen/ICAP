using System.Text;
using System.Text.Json;
using Azure.Messaging.ServiceBus;

namespace ICAP_ServiceBus
{
    public class BusPublisher : IBusPublisher
    {
        private readonly string _connectionString;

        public BusPublisher(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task SendMessageAsync<T>(T serviceBusMessage, string topicName)
        {
            var client = new ServiceBusClient(_connectionString, new ServiceBusClientOptions
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
