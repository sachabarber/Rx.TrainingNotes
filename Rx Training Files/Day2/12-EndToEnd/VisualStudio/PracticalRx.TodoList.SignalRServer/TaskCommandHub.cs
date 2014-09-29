using System;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using PracticalRx.TodoList.Contracts;
using PracticalRx.TodoList.EventStore;

namespace PracticalRx.TodoList.SignalRServer
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
            try
            {
                await _taskListEventStore.AddTask(addTaskCommand);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public async Task UpdateTask(UpdateTaskCommand updateTaskCommand)
        {
            try
            {
                await _taskListEventStore.UpdateTask(updateTaskCommand);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public async Task DeleteTask(DeleteTaskCommand deleteTaskCommand)
        {
            try
            {
                await _taskListEventStore.DeleteTaskList(deleteTaskCommand);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}