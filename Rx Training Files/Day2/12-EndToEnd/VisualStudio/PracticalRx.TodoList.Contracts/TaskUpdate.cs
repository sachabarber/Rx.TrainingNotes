using System;

namespace PracticalRx.TodoList.Contracts
{
    public class TaskUpdate
    {
        public int Version { get; set; }
        public Guid EventId { get; set; }
        

        public TaskAddedEvent AddedEvent { get; set; }
        public TaskUpdatedEvent UpdatedEvent { get; set; }
        public TaskDeletedEvent DeletedEvent { get; set; }
    }
}