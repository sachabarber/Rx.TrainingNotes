using System;

namespace PracticalRx.TodoList.Contracts
{
    public class AddTaskCommand
    {
        public int ExpectedVersion { get; set; }
        public Guid EventId { get; set; }
        public Guid NewTaskId { get; set; }
        public string Title { get; set; }
        public bool IsCompleted { get; set; }
    }
}