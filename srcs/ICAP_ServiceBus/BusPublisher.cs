using System.Text;
using System.Text.Json;
using Microsoft.Azure.ServiceBus;

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
            var queueClient = new QueueClient(_connectionString, topicName);
            var messageBody = JsonSerializer.Serialize(serviceBusMessage);
            var message = new Message(Encoding.UTF8.GetBytes(messageBody));

            await queueClient.SendAsync(message);
        }
    }
}
