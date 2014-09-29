using System;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using PracticalRx.TodoList.EventStore;

namespace PracticalRx.TodoList.Web.Hubs
{
    public class TaskQueryHub : Hub
    {
        private readonly ITaskListEventStore _taskListEventStore;
        private readonly SingleAssignmentDisposable _updatesSubscription = new SingleAssignmentDisposable();

        //LAZY IoC
        public TaskQueryHub() : this(new TaskListEventStore())
        {
            
        }
        public TaskQueryHub(ITaskListEventStore taskListEventStore)
        {
            _taskListEventStore = taskListEventStore;
        }

        public Task<int> GetHeadVersion()
        {
            return _taskListEventStore.GetHeadVersion();
        }

        public void GetTaskUpdatesFrom(int fromVersionExclusive)
        {
            _updatesSubscription.Disposable = _taskListEventStore
                .TaskUpdates(fromVersionExclusive)
                .Subscribe(x => Clients.Caller.TaskUpdates(x));
        }

        public override Task OnDisconnected()
        {
            _updatesSubscription.Dispose();
            return base.OnDisconnected();
        }        
    }
}