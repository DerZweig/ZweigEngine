namespace ZweigEngine.Common.Services.Messages;

public interface IMessageBus
{
    IDisposable Subscribe<THandler>(THandler handler) where THandler : class;
    void Publish<THandler>(Action<THandler> message) where THandler : class;
}