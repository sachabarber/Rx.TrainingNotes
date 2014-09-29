using System;

namespace PracticalRx.TodoList.Contracts
{
    public class TaskAddedEvent
    {
        public Guid TaskId { get; set; }
        public string Title { get; set; }
    }
}