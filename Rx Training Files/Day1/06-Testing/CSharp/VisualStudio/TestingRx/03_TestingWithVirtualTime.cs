using System;
using System.Reactive.Linq;
using Microsoft.Reactive.Testing;
using NUnit.Framework;

namespace TestingRx
{
    //Note that these tests should all run very fast i.e. ~10ms each. Compare this to the +2sec it takes to run them previously.
    //  This method allows your unit test to scale to hundred and thousands of unit test and still have them all run in a few seconds.
    [TestFixture]
    public class TestingWithVirtualTime : ReactiveTest
    {
        [Test]
        public void Testing_Timer_with_test_scheduler_and_ReactiveTest()
        {
            var scheduler = new TestScheduler();
            var observer = scheduler.CreateObserver<long>();
            var source = Observable.Timer(TimeSpan.FromSeconds(2), scheduler);

            source.Subscribe(observer);
            scheduler.Start();

            observer.Messages.AssertEqual(
                OnNext<long>(2.Seconds(), 0),
                OnCompleted<long>(2.Seconds()));
        }

        [Test]
        public void Testing_timeout_with_test_scheduler_and_ReactiveTest()
        {
            var scheduler = new TestScheduler();
            var observer = scheduler.CreateObserver<long>();
            var source = Observable.Never<long>()
                                   .Timeout(TimeSpan.FromSeconds(2), scheduler);

            source.Subscribe(observer);
            scheduler.Start();

            observer.Messages.AssertEqual(
                OnError<long>(2.Seconds(), ex => ex is TimeoutException));
        }
    }
}
