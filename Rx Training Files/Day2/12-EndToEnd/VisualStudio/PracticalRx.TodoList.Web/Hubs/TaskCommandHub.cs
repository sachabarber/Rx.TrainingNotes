using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using PracticalRx.TodoList.Contracts;
using PracticalRx.TodoList.EventStore;

namespace PracticalRx.TodoList.Web.Hubs
{
    public class TaskCommandHub : Hub
    {
        private readonly ITaskListEventStore _taskListEventStore;

        //LAZY IoC
        public TaskCommandHub()
            : this(new TaskListEventStore())
        {

        }
        public TaskCommandHub(ITaskListEventStore taskListEventStore)
        {
            _taskListEventStore = taskListEventStore;
        }

        public async Task AddTask(AddTaskCommand addTaskCommand)
        {
            await _taskListEventStore.AddTask(addTaskCommand);
        }

        public async Task UpdateTask(UpdateTaskCommand updateTaskCommand)
        {
            await _taskListEventStore.UpdateTask(updateTaskCommand);
        }

        public async Task DeleteTask(DeleteTaskCommand deleteTaskCommand)
        {
            await _taskListEventStore.DeleteTaskList(deleteTaskCommand);
        }
    }
}