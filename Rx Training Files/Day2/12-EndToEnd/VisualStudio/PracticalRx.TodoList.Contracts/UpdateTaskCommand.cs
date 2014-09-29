using System;

namespace PracticalRx.TodoList.Contracts
{
    public class UpdateTaskCommand
    {
        public int ExpectedVersion { get; set; }
        public Guid EventId { get; set; }
        public Guid TaskId { get; set; }
        public string Title { get; set; }
        public bool IsCompleted { get; set; }
    }
}
