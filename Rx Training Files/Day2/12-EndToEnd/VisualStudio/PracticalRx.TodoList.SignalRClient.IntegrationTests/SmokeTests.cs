using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using PracticalRx.TodoList.Contracts;

namespace PracticalRx.TodoList.SignalRClient.IntegrationTests
{
    [TestFixture]
    public class SmokeTests
    {
        [Test]
        public async void CanAdd()
        {
            var repo = new TaskListSignalRServiceClient();

            var addCmd = new AddTaskCommand
                         {
                             EventId = Guid.NewGuid(),
                             ExpectedVersion = -1,
                             IsCompleted = false,
                             NewTaskId = Guid.NewGuid(),
                             Title = "From Client integration testing"
                         };

           await repo.AddTask(addCmd);
        }

        [Test]
        public void CanGetUpdates()
        {
            var repo = new TaskListSignalRServiceClient();
            var updates = new List<TaskUpdate>();
            repo.TaskUpdates(-1)
                .Subscribe(updates.Add);

            for (int i = 0; i < 10; i++)
            {
                Console.Write(".");
                Thread.Sleep(250);
            }
            Console.WriteLine();


            Assert.AreNotEqual(0, updates.Count);
        }
    }
}
