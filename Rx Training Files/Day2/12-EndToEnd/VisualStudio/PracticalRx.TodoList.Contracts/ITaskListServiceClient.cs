using System;
using System.Threading.Tasks;

namespace PracticalRx.TodoList.Contracts
{
    public interface ITaskListServiceClient
    {
        Task AddTask(AddTaskCommand addTaskCommand);

        Task UpdateTask(UpdateTaskCommand updateTaskCommand);

        Task DeleteTask(DeleteTaskCommand deleteTaskCommand);

        Task<int> HeadVersion();

        IObservable<TaskUpdate> TaskUpdates(int fromVersionExclusive);
    }
}