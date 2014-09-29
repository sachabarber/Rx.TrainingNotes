using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
using PracticalRx.TodoList.Contracts;

namespace PracticalRx.TodoList.SignalRClient
{
    public class TaskListSignalRServiceClient : ITaskListServiceClient
    {
        //private const string ServerAddress = "http://localhost:8080";
        private const string ServerAddress = "http://localhost:58978";
        private const string TaskCommandHubName = "TaskCommandHub";
        private const string TaskQueryHubName = "TaskQueryHub";


        public async Task AddTask(AddTaskCommand addTaskCommand)
        {
            using (var hubConnection = CreateHubConnection())
            {
                var proxy = hubConnection.CreateHubProxy(TaskCommandHubName);
                await hubConnection.Start();
                await proxy.Invoke("AddTask", addTaskCommand);
            }
        }

        public async Task UpdateTask(UpdateTaskCommand updateTaskCommand)
        {
            using (var hubConnection = CreateHubConnection())
            {
                var proxy = hubConnection.CreateHubProxy(TaskCommandHubName);
                await hubConnection.Start();
                await proxy.Invoke("UpdateTask", updateTaskCommand);
            }
        }

        public async Task DeleteTask(DeleteTaskCommand deleteTaskCommand)
        {
            using (var hubConnection = CreateHubConnection())
            {
                var proxy = hubConnection.CreateHubProxy(TaskCommandHubName);
                await hubConnection.Start();
                await proxy.Invoke("DeleteTask", deleteTaskCommand);
            }
        }

        public async Task<int> HeadVersion()
        {
            using (var hubConnection = CreateHubConnection())
            {
                var proxy = hubConnection.CreateHubProxy(TaskQueryHubName);
                await hubConnection.Start();
                return await proxy.Invoke<int>("GetHeadVersion");
            }
        }

        public IObservable<TaskUpdate> TaskUpdates(int fromVersionExclusive)
        {
            return Observable.Create<TaskUpdate>(async observer =>
            {
                var subscription = Disposable.Empty;
                var hubConnection = CreateHubConnection();
                
                try
                {
                    var proxy = hubConnection.CreateHubProxy(TaskQueryHubName);
                    await hubConnection.Start();

                    subscription = proxy.Observe("TaskUpdates")
                                        .SelectMany(updates => updates)
                                        .Select(jToken => jToken.ToObject<TaskUpdate>())
                                        .Subscribe(observer);

                    await proxy.Invoke("GetTaskUpdatesFrom", fromVersionExclusive);
                }
                catch (Exception e)
                {
                    observer.OnError(e);
                }

                return new CompositeDisposable(subscription, hubConnection);
            });
        }

        private static HubConnection CreateHubConnection()
        {
            var hubConnection = new HubConnection(ServerAddress);
            //hubConnection.TraceLevel = TraceLevels.All;
            //hubConnection.TraceWriter = Console.Out;
            return hubConnection;
        }
    }
}
