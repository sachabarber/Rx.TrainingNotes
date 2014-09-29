using EventStore.ClientAPI;

namespace PracticalRx.TodoList.EventStore
{
    public interface IEventStoreConnectionFactory
    {
        IEventStoreConnection Connect();
    }
}