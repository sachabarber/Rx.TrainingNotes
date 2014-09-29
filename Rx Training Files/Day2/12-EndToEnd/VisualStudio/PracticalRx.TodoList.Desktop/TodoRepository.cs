using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using PracticalRx.TodoList.Contracts;

namespace PracticalRx.TodoList.Desktop
{
    public class TodoRepository : ITodoRepository, IDisposable
    {
        private readonly ITaskListServiceClient _serviceClient;
        private readonly IConnectableObservable<TaskUpdate> _updates;
        private readonly IDisposable _updateSubscription;
        private int _currentVersion = -1;

        public TodoRepository(ITaskListServiceClient serviceClient)
        {
            _serviceClient = serviceClient;
            _updates = _serviceClient.TaskUpdates(-1)
                .Do(x => _currentVersion = x.Version)
                .Replay();
            //TODO: Favor the use of of AutoConnect/LazyConnect -LC
            _updateSubscription = _updates.Connect();
        }

        public async Task AddItem(TodoViewModel item)
        {
            var addCmd = new AddTaskCommand
                         {
                             EventId = Guid.NewGuid(),
                             NewTaskId = item.Id,
                             ExpectedVersion = _currentVersion,
                             IsCompleted = item.IsCompleted,
                             Title = item.Title
                         };
            await _serviceClient.AddTask(addCmd);
        }

        public async Task UpdateItem(TodoViewModel item)
        {
            var updateCmd = new UpdateTaskCommand
            {
                EventId = Guid.NewGuid(),
                TaskId = item.Id,
                ExpectedVersion = _currentVersion,
                IsCompleted = item.IsCompleted,
                Title = item.Title
            };
            await _serviceClient.UpdateTask(updateCmd);
        }

        public async Task RemoveItem(TodoViewModel item)
        {
            var deleteCmd = new DeleteTaskCommand
            {
                EventId = Guid.NewGuid(),
                TaskId = item.Id,
                ExpectedVersion = _currentVersion,
            };
            await _serviceClient.DeleteTask(deleteCmd);
        }

        public Task<int> HeadVersion()
        {
            return _serviceClient.HeadVersion();
        }

        public IObservable<TaskUpdate> Updates { get { return _updates; } }

        public void Dispose()
        {
            _updateSubscription.Dispose();
        }
    }
}