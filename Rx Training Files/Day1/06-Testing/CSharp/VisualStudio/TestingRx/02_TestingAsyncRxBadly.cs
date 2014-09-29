using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading;
using NUnit.Framework;

namespace TestingRx
{
    [TestFixture]
    public class TestingAsyncRxBadly
    {
        //Note that this test actually takes >2seconds to run
        [Test, Timeout(2500)]
        public void Testing_Timer_the_slow_way()
        {
            var source = Observable.Timer(TimeSpan.FromSeconds(2));

            var actualValues = new List<long>();
            var hasCompleted = false;

            source.Subscribe(actualValues.Add, () => hasCompleted = true);
         
            Thread.Sleep(2100);
            CollectionAssert.AreEqual(new[]{0}, actualValues);
            Assert.That(hasCompleted);
        }

        //Note again, that this test actually takes >2seconds to run. What if we had to test 15/30/90second timeouts?
        [Test, Timeout(2500)]
        public void Testing_timeout_the_slow_way()
        {
            var source = Observable.Never<long>()
                                   .Timeout(TimeSpan.FromSeconds(2));

            var actualValues = new List<long>();
            Exception exception = null;
            var hasCompleted = false;

            source.Subscribe(actualValues.Add, ex=>exception=ex, () => hasCompleted = true);

            Thread.Sleep(2100);
            CollectionAssert.IsEmpty(actualValues);
            Assert.IsNotNull(exception);
            Assert.IsInstanceOf<TimeoutException>(exception);
            Assert.IsFalse(hasCompleted);
        }
    }
}
