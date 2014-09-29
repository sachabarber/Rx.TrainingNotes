using System;
using System.Data;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using EventStore.ClientAPI.Exceptions;
using EventStore.ClientAPI.Messages;

namespace PracticalRx.TodoList.EventStore
{
    public sealed class EventStore : IEventStore
    {
        private readonly IEventStoreConnectionFactory _connectionFactory;

        //Poor man's IoC
        public EventStore() : this(new EventStoreConnectionFactory())
        {
        }
        public EventStore(IEventStoreConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task DeleteStream(string streamName)
        {
            using (var conn = _connectionFactory.Connect())
            {
                await conn.DeleteStreamAsync(streamName, ExpectedVersion.Any);
            }
        }

        public IObservable<RecordedEvent> GetNewEvents(string streamName)
        {
            return Observable.Create<RecordedEvent>(o =>
                {
                    var conn = _connectionFactory.Connect();
                    var subscription = conn.SubscribeToStream(streamName, false, (_, resolvedEvent) => o.OnNext(resolvedEvent.OriginalEvent));
                    return new CompositeDisposable(subscription, conn);
                });
        }

        public IObservable<RecordedEvent> GetEvents(string streamName, int? fromVersionExclusive)
        {
            return Observable.Create<RecordedEvent>(o =>
                {
                    var conn = _connectionFactory.Connect();
                    var subscription = conn.SubscribeToStreamFrom(streamName, fromVersionExclusive, true, (_, resolvedEvent) => o.OnNext(resolvedEvent.OriginalEvent));
                    return new CompositeDisposable(Disposable.Create(() => subscription.Stop(TimeSpan.FromSeconds(2))), conn);
                });
        }

        public async Task<int> GetHeadVersion(string streamName)
        {
            using (var conn = _connectionFactory.Connect())
            {
                var slice = await conn.ReadStreamEventsBackwardAsync(streamName, StreamPosition.End, 1, false);
                if (slice.Status == SliceReadStatus.Success && slice.Events.Length == 1)
                {
                    return slice.Events[0].OriginalEvent.EventNumber;
                }
                if (slice.Status == SliceReadStatus.StreamNotFound)
                {
                    return -1;
                }             
                throw new StreamDeletedException(streamName);
            }
        }

        public async Task SaveEvent(string streamName, int expectedVersion, Guid eventId, string eventType, string jsonData, string jsonMetaData = null)
        {
            var payload = Encoding.UTF8.GetBytes(jsonData);
            var metadata = jsonMetaData == null ? null : Encoding.UTF8.GetBytes(jsonMetaData);
            using (var conn = _connectionFactory.Connect())
            {
                await conn.AppendToStreamAsync(streamName, expectedVersion, new EventData(eventId, eventType, true, payload, metadata));
            }
        }
    }
}