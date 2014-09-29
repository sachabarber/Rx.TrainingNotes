using System;
using System.Threading.Tasks;

namespace PracticalRx.Client.Common
{
    public interface ITaskListServiceClient
    {
        Task UpdateTaskList(UpdateTaskCommand updateTaskCommand);

        IObservable<TaskUpdate> TaskUpdates();
    }
}