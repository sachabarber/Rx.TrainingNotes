using System;

namespace PracticalRx.TodoList.EventStore.Data
{
    public class TaskUpdated
    {
        public Guid TaskId { get; set; }
        public string Title { get; set; }
        public bool IsCompleted { get; set; }
    }
}