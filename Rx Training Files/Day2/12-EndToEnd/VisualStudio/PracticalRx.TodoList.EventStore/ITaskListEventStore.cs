using System;
using System.Threading.Tasks;
using PracticalRx.TodoList.Contracts;

namespace PracticalRx.TodoList.EventStore
{
    public interface ITaskListEventStore
    {
        Task AddTask(AddTaskCommand addTaskCommand);

        Task UpdateTask(UpdateTaskCommand updateTaskCommand);

        Task DeleteTaskList(DeleteTaskCommand deleteTaskCommand);

        Task<int> GetHeadVersion();

        IObservable<TaskUpdate> TaskUpdates(int fromVersionExclusive);
    }
}