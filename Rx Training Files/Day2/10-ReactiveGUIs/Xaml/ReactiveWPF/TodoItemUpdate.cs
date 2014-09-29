using System;

namespace ReactiveWPF
{
    public class TodoItemUpdate
    {
        private readonly Guid _id;
        private readonly string _title;
        private readonly bool _isCompleted;
        private readonly bool _isDeleted;

        public TodoItemUpdate(Guid id, string title, bool isCompleted, bool isDeleted)
        {
            _id = id;
            _title = title;
            _isCompleted = isCompleted;
            _isDeleted = isDeleted;
        }

        public Guid Id { get { return _id; } }

        public string Title { get { return _title; } }

        public bool IsCompleted { get { return _isCompleted; } }

        public bool IsDeleted { get { return _isDeleted; } }
    }
}