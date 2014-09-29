using System;
using System.IO;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using EventStore.ClientAPI.Common.Utils;
using Newtonsoft.Json;
using PracticalRx.TodoList.Contracts;
using PracticalRx.TodoList.EventStore.Data;

namespace PracticalRx.TodoList.EventStore
{
    public sealed class TaskListEventStore : ITaskListEventStore
    {
        private const string TaskListStreamName = "TaskList";
        private readonly IEventStore _eventStore;

        //Poor man's IoC
        public TaskListEventStore()
            : this(new EventStore())
        {
        }
        public TaskListEventStore(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public Task AddTask(AddTaskCommand addTaskCommand)
        {
            Console.WriteLine("Received AddCommand {0}", addTaskCommand.ToJson());
            if(addTaskCommand.NewTaskId == Guid.Empty)
                throw new InvalidDataException("TaskId of 00000000-0000-0000-0000-000000000000 is not valid.");
            var newTask = new TaskAdded
                          {
                              TaskId = addTaskCommand.NewTaskId,
                              Title = addTaskCommand.Title,
                          };
            return _eventStore.SaveEvent(TaskListStreamName, addTaskCommand.ExpectedVersion, addTaskCommand.EventId, EventTypes.TaskAdded, newTask.ToJson());
        }

        public Task UpdateTask(UpdateTaskCommand updateTaskCommand)
        {
            Console.WriteLine("Received UpdateCommand {0}", updateTaskCommand.ToJson());
            var update = new TaskUpdated
                         {
                             TaskId = updateTaskCommand.TaskId,
                             Title = updateTaskCommand.Title,
                             IsCompleted = updateTaskCommand.IsCompleted,
                         };
            return _eventStore.SaveEvent(TaskListStreamName, updateTaskCommand.ExpectedVersion, updateTaskCommand.EventId, EventTypes.TaskUpdated, update.ToJson());
        }

        public Task DeleteTaskList(DeleteTaskCommand deleteTaskCommand)
        {
            Console.WriteLine("Received DeleteCommand {0}", deleteTaskCommand.ToJson());
            var taskDeleted = new TaskDeleted { TaskId = deleteTaskCommand.TaskId };
            return _eventStore.SaveEvent(TaskListStreamName, deleteTaskCommand.ExpectedVersion, deleteTaskCommand.EventId, EventTypes.TaskDeleted, taskDeleted.ToJson());
        }

        public async Task<int> GetHeadVersion()
        {
            return await _eventStore.GetHeadVersion(TaskListStreamName);
        }

        public IObservable<TaskUpdate> TaskUpdates(int fromVersionExclusive)
        {
            return _eventStore.GetEvents(TaskListStreamName, fromVersionExclusive < 0 ? (int?)null : fromVersionExclusive)
                .Select(Translate);
        }

        private static TaskUpdate Translate(RecordedEvent recordedEvent)
        {
            var result = new TaskUpdate
                         {
                             Version = recordedEvent.EventNumber,
                             EventId = recordedEvent.EventId,
                         };
            var json = Encoding.UTF8.GetString(recordedEvent.Data);

            switch (recordedEvent.EventType)
            {
                case EventTypes.TaskAdded:
                    result.AddedEvent = TranslateTaskAdded(json);
                    break;
                case EventTypes.TaskUpdated:
                    result.UpdatedEvent = TranslateTaskUpdated(json);
                    break;
                case EventTypes.TaskDeleted:
                    result.DeletedEvent = TranslateTaskDeleted(json);
                    break;
            }
            return result;
        }

        private static TaskAddedEvent TranslateTaskAdded(string json)
        {
            var data = JsonConvert.DeserializeObject<TaskAdded>(json);
            return new TaskAddedEvent
            {
                TaskId = data.TaskId,
                Title = data.Title,
            };
        }

        private static TaskUpdatedEvent TranslateTaskUpdated(string json)
        {
            var data = JsonConvert.DeserializeObject<TaskUpdated>(json);
            return new TaskUpdatedEvent
            {
                TaskId = data.TaskId,
                Title = data.Title,
                IsCompleted = data.IsCompleted
            };
        }

        private static TaskDeletedEvent TranslateTaskDeleted(string json)
        {
            var data = JsonConvert.DeserializeObject<TaskDeleted>(json);
            return new TaskDeletedEvent { TaskId = data.TaskId };
        }

        private static class EventTypes
        {
            public const string TaskAdded = "TaskAdded";
            public const string TaskUpdated = "TaskUpdated";
            public const string TaskDeleted = "TaskDeleted";
        }
    }
}