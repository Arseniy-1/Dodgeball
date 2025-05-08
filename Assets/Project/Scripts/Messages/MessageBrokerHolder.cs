using UniRx;

public static class MessageBrokerHolder
{
    public static IMessageBroker GameActions { get; private set; } = new MessageBroker();
}