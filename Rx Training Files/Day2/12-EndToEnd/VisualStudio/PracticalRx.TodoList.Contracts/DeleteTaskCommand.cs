using System;

namespace PracticalRx.TodoList.Contracts
{
    public class DeleteTaskCommand
    {
        public int ExpectedVersion { get; set; }
        public Guid EventId { get; set; }
        public Guid TaskId { get; set; }
    }
}