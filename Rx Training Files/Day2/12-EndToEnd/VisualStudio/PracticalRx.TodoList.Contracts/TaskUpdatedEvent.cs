using System;

namespace PracticalRx.TodoList.Contracts
{
    public class TaskUpdatedEvent
    {
        public Guid TaskId { get; set; }
        public string Title { get; set; }
        public bool IsCompleted { get; set; }
    }
}