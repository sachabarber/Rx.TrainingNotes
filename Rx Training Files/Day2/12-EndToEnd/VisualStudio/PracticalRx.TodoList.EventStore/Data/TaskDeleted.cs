using System;

namespace PracticalRx.TodoList.EventStore.Data
{
    public class TaskDeleted
    {
        public Guid TaskId { get; set; }
    }
}