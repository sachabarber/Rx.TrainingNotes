using System;
using System.Threading.Tasks;
using PracticalRx.TodoList.Contracts;

namespace PracticalRx.TodoList.Desktop
{
    public interface ITodoRepository
    {
        Task AddItem(TodoViewModel item);
        Task UpdateItem(TodoViewModel item);
        Task RemoveItem(TodoViewModel item);
        Task<int> HeadVersion();
        IObservable<TaskUpdate> Updates { get; }
    }
}