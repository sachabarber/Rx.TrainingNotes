using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading;
using Microsoft.Reactive.Testing;
using NUnit.Framework;

namespace HotAndCold
{
    [TestFixture]
    public class ColdSequences : ReactiveTest
    {
        [Test]
        public void Interval_appears_hot()
        {
            var expected = new[] {0L, 1L, 2L};
            var listA = new List<long>();
            var listB = new List<long>();

            var source = Observable.Interval(TimeSpan.FromMilliseconds(10)).Take(3);
            source.Subscribe(listA.Add);
            source.Subscribe(listB.Add);

            Thread.Sleep(1000);

            CollectionAssert.AreEqual(expected, listA);
            CollectionAssert.AreEqual(expected, listB);

        }
        [Test]
        public void Interval_is_not_hot()
        {
            var expected = new[] { 0L, 1L, 2L };
            var listA = new List<long>();
            var listB = new List<long>();

            var source = Observable.Interval(TimeSpan.FromMilliseconds(10)).Take(3);
            source.Subscribe(listA.Add);

            Thread.Sleep(1000);
            
            //If Interval was hot, then we would not get any values in this subscription.
            source.Subscribe(listB.Add);
            Thread.Sleep(1000);

            CollectionAssert.AreEqual(expected, listA);
            CollectionAssert.AreEqual(expected, listB);
        }

        [Test]
        public void Share_to_make_hot()
        {
            var testScheduler = new TestScheduler();
            var actualA = testScheduler.CreateObserver<long>();
            var actualB = testScheduler.CreateObserver<long>();

            //was cold, wont work
            //var source = Observable.Interval(TimeSpan.FromSeconds(1), testScheduler)
            //    .Take(3);

            //NEW : make it hot by publishing, should work
            var source = Observable.Interval(TimeSpan.FromSeconds(1), testScheduler)
                .Take(3).Publish();

            
            source.Subscribe(actualA);
            testScheduler.AdvanceBy(1.5.Seconds());
            //If Interval was hot, then we would not get any values in this subscription.
            source.Subscribe(actualB);

            //NEW : this is needed after publish and subscribes
            source.Connect();

            testScheduler.Start();

            actualA.Messages.AssertEqual(
                OnNext(1.Seconds(), 0L),
                OnNext(2.Seconds(), 1L),
                OnNext(3.Seconds(), 2L),
                OnCompleted<long>(3.Seconds()));
            actualB.Messages.AssertEqual(
                OnNext(2.Seconds(), 1L),
                OnNext(3.Seconds(), 2L),
                OnCompleted<long>(3.Seconds()));
        }
    }
}
