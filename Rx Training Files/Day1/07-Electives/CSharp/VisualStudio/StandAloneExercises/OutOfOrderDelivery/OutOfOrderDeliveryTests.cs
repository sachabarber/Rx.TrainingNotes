using System.Threading.Tasks;
using Microsoft.Reactive.Testing;
using NUnit.Framework;

namespace StandAloneExercises.OutOfOrderDelivery
{
    [TestFixture]
    public class OutOfOrderDeliveryTests
    {
        [Test, Timeout(1000)]
        public void TestMethod1()
        {
            var testScheduler = new TestScheduler();
            var service = new Service(new Network(testScheduler));

            var responseA = service.Request("A");
            var responseB = service.Request("B");
            var responseC = service.Request("C");
            var responseD = service.Request("D");

            //Run in virtual time :-)
            testScheduler.Start();
            Task.WaitAll(responseA, responseB, responseC);

            Assert.AreEqual("Message A", responseA.Result);
            Assert.AreEqual("Message B", responseB.Result);
            Assert.AreEqual("Message C", responseC.Result);
            Assert.AreEqual("Message D", responseD.Result);
        }
    }
}