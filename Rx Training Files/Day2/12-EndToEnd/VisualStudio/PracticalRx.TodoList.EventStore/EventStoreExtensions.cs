using System;
using EventStore.ClientAPI;

namespace PracticalRx.TodoList.EventStore
{
    public static class EventStoreExtensions
    {
        public static IObservable<RecordedEvent> GetAllEvents(this IEventStore eventStore, string streamName)
        {
            //Seems pretty hostile that null is used here, not StreamPosition.Start. Potentially worth raising with Greg? -LC
            return eventStore.GetEvents(streamName, null);
        }
    }
}