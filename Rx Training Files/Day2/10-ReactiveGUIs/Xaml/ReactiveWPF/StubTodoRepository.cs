using System;
using System.Reactive.Subjects;

namespace ReactiveWPF
{
    public class StubTodoRepository : ITodoRepository, IDisposable
    {
        private readonly ReplaySubject<TodoItemUpdate> _updates = new ReplaySubject<TodoItemUpdate>(2);

        public StubTodoRepository()
        {
            _updates.OnNext(new TodoItemUpdate(Guid.NewGuid(), "One", false, false));
            _updates.OnNext(new TodoItemUpdate(Guid.NewGuid(), "Two", false, false));
        }

        public void SaveItem(TodoItemViewModel item)
        {
            var update = new TodoItemUpdate(item.Id, item.Title, item.IsCompleted, false);
            _updates.OnNext(update);
        }

        public void RemoveItem(TodoItemViewModel item)
        {
            var update = new TodoItemUpdate(item.Id, item.Title, item.IsCompleted, true);
            _updates.OnNext(update);
        }

        public IObservable<TodoItemUpdate> Updates { get { return _updates; } }

        public void Dispose()
        {
            _updates.Dispose();
        }
    }
}