using System.Net;
using EventStore.ClientAPI;

namespace PracticalRx.TodoList.EventStore
{
    //Could get from config, if this was a real app that we wanted to be able modify without a recompile.
    public sealed class EventStoreConnectionFactory : IEventStoreConnectionFactory
    {
        private static readonly IPAddress Address = new IPAddress(new byte[] { 127, 0, 0, 1 });
        private const int Port = 1113;

        public IEventStoreConnection Connect()
        {
            var connectionSettings = ConnectionSettings.Create();
            var endPoint = new IPEndPoint(Address, Port);
            var connection = EventStoreConnection.Create(connectionSettings, endPoint);
            connection.Connect();
            return connection;
        }
    }
}
