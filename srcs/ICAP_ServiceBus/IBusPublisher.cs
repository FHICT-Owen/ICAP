namespace ICAP_ServiceBus;

public interface IBusPublisher
{
    Task SendMessageAsync<T>(T serviceBusMessage, string topicName);
}