using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using EventStore.ClientAPI;
using NUnit.Framework;
using PracticalRx.TodoList.Contracts;

namespace PracticalRx.TodoList.EventStore.IntegrationTests
{
    [TestFixture]
    public class SmokeTest
    {
        private TaskListEventStore _repo;

        [SetUp]
        public async void SetUp()
        {
            var connFactory = new StubEventStoreConnectionFactory();
            var es = new EventStore(connFactory);
            _repo = new TaskListEventStore(es);
            //Ideally,  setup would delete the db/logs directories and then start the ES server.
            //  TearDown would stop the server and delete the db/logs directories
        }

        [Test]//, Timeout(2000)]
        public async void GIVEN_an_empty_stream_WHEN_subscribing_THEN_no_events_are_returned()
        {
            var observer = new List<object>();

            _repo.TaskUpdates(StreamPosition.Start).Subscribe(observer.Add);
            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(200);
            }


            Assert.AreEqual(0, observer.Count);
        }

        [Test]//, Timeout(2000)]
        public async void GIVEN_an_empty_stream_WHEN_Creating_new_task_THEN_TaskAdded_event_is_saved()
        {
            var addTaskCommand1 = new AddTaskCommand
            {
                ExpectedVersion = ExpectedVersion.NoStream,
                EventId = Guid.NewGuid(),
                NewTaskId = Guid.NewGuid(),
                IsCompleted = false,
                Title = "Initial value(from tests)"
            };
            var addTaskCommand2 = new AddTaskCommand
            {
                ExpectedVersion = ExpectedVersion.NoStream + 1,
                EventId = Guid.NewGuid(),
                NewTaskId = Guid.NewGuid(),
                IsCompleted = false,
                Title = "Initial value(from tests)"
            };


            var observer = new List<TaskUpdate>();
            using (_repo.TaskUpdates(-1)
                        .Subscribe(observer.Add))
            {
                await _repo.AddTask(addTaskCommand1);
                await _repo.AddTask(addTaskCommand2);

                Thread.Sleep(2000);
            }


            Assert.AreEqual(addTaskCommand1.EventId, observer[0].EventId);
            Assert.AreEqual(addTaskCommand1.NewTaskId, observer[0].AddedEvent.TaskId);

            Assert.AreEqual(addTaskCommand2.EventId, observer[1].EventId);
            Assert.AreEqual(addTaskCommand2.NewTaskId, observer[1].AddedEvent.TaskId);

        }
    }

    public sealed class StubEventStoreConnectionFactory : IEventStoreConnectionFactory
    {
        public IEventStoreConnection Connect()
        {
            var connectionSettings = ConnectionSettings.Create();
            var endPoint = new IPEndPoint(new IPAddress(new byte[] { 127, 0, 0, 1 }), 1113);
            var conn = EventStoreConnection.Create(connectionSettings, endPoint);
            conn.Connect();
            return conn;
        }
    }
}
