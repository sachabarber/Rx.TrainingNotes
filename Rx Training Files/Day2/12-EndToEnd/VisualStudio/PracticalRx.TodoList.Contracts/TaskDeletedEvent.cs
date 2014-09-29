using System;

namespace PracticalRx.TodoList.Contracts
{
    public class TaskDeletedEvent
    {
        public Guid TaskId { get; set; }
    }
}