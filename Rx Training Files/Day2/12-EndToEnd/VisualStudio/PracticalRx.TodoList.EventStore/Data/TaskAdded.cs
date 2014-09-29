using System;

namespace PracticalRx.TodoList.EventStore.Data
{
    public class TaskAdded
    {
        public Guid TaskId { get; set; }
        public string Title { get; set; }
    }
}
