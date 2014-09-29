using System;
using System.Threading.Tasks;
using EventStore.ClientAPI;

namespace PracticalRx.TodoList.EventStore
{
    public interface IEventStore
    {
        Task SaveEvent(string streamName, int expectedVersion, Guid eventId, string eventType, string jsonData, string jsonMetaData = null);
        IObservable<RecordedEvent> GetNewEvents(string streamName);
        IObservable<RecordedEvent> GetEvents(string streamName, int? fromVersionExclusive);
        Task<int> GetHeadVersion(string streamName);
    }
}