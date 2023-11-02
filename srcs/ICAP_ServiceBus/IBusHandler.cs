namespace ICAP_ServiceBus;

public interface IBusHandler
{
    Task CreateBusHandler<T>(string topicName, string subscriptionName, Func<T, Task> onMessageProcessed);
}