using System;

namespace ReactiveWPF
{
    public interface ITodoRepository
    {
        void SaveItem(TodoItemViewModel item);
        void RemoveItem(TodoItemViewModel item);
        IObservable<TodoItemUpdate> Updates { get; } 
    }
}