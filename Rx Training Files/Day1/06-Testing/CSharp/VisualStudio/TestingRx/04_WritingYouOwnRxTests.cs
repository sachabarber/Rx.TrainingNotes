using System;
using System.Reactive;
using System.Reactive.Linq;
using Microsoft.Reactive.Testing;
using NUnit.Framework;

namespace TestingRx
{
    [TestFixture]
    public class WritingYouOwnRxTests : ReactiveTest
    {
        [Test]
        public void A_sequence_with_an_immediate_value_of_1_and_a_value_of_2_in_2_seconds_then_completes()
        {
            var scheduler = new TestScheduler();
            var observer = scheduler.CreateObserver<int>();

            var source = Observable.Return(1).Merge(Observable.Timer(TimeSpan.FromSeconds(2), scheduler).Select(x => 2));

     

            
            source.Subscribe(observer);
            scheduler.Start();

            //Note that the AssertEqual extension method comes from the Microsoft.Reactive.Testing namespace.
            //Note that the OnNext/OnCompleted factory methods come from ReactiveTest (which this fixture sub-classes). You could also use them statically like ReactiveTest.OnNext(0.Seconds(), 1)
            observer.Messages.AssertEqual(
                OnNext(0.Seconds(), 1),
                OnNext(2.Seconds(), 2),
                OnCompleted<int>(2.Seconds()));
        }





        [Test]
        public void A_sequence_that_produces_a_value_at_5_seconds_then_timesout_after_10_more_seconds()
        {
            var scheduler = new TestScheduler();
            var observer = scheduler.CreateObserver<long>();

            var source = Observable.Timer(TimeSpan.FromSeconds(5), scheduler)
                                   .Concat(Observable.Never<long>())
                                   .Timeout(TimeSpan.FromSeconds(10), scheduler);

            source.Subscribe(observer);
            scheduler.Start();

            observer.Messages.AssertEqual(
                OnNext(5.Seconds(), 0L),
                OnError<long>(15.Seconds(), ex => ex is TimeoutException));
        }

        [Test]
        public void Using_CreateColdObservable_to_generate_sequences()
        {
            var scheduler = new TestScheduler();
            var observer = scheduler.CreateObserver<string>();


            //TODO: Instead of creating the source with standard Rx operators, 
            //create a pre-canned sequence with the Rx testing operators

            //COLD
            //var source = scheduler.CreateColdObservable<string>(
            //    new Recorded<Notification<string>>(5.Seconds(), Notification.CreateOnNext("A")),
            //    new Recorded<Notification<string>>(10.Seconds(), Notification.CreateOnNext("quick")),
            //    new Recorded<Notification<string>>(20.Seconds(), Notification.CreateOnNext("brown")),
            //    new Recorded<Notification<string>>(40.Seconds(), Notification.CreateOnNext("fox")),
            //    new Recorded<Notification<string>>(60.Seconds(), Notification.CreateOnNext("jumps")),
            //    new Recorded<Notification<string>>(180.Seconds(), Notification.CreateOnCompleted<string>()));

            //HOT
            //var source = scheduler.CreateHotObservable<string>(
            //    new Recorded<Notification<string>>(5.Seconds(), Notification.CreateOnNext("A")),
            //    new Recorded<Notification<string>>(10.Seconds(), Notification.CreateOnNext("quick")),
            //    new Recorded<Notification<string>>(20.Seconds(), Notification.CreateOnNext("brown")),
            //    new Recorded<Notification<string>>(40.Seconds(), Notification.CreateOnNext("fox")),
            //    new Recorded<Notification<string>>(60.Seconds(), Notification.CreateOnNext("jumps")),
            //    new Recorded<Notification<string>>(180.Seconds(), Notification.CreateOnCompleted<string>()));

            //shorthand
            var source = scheduler.CreateColdObservable<string>(
              OnNext(5.Seconds(), "A"),
                ReactiveTest.OnNext(10.Seconds(), "quick"),
                ReactiveTest.OnNext(20.Seconds(), "brown"),
                ReactiveTest.OnNext(40.Seconds(), "fox"),
                ReactiveTest.OnNext(1.Minutes(), "jumps"),
                ReactiveTest.OnCompleted<string>(3.Minutes()));



            source.Subscribe(observer);
            scheduler.Start();

            observer.Messages.AssertEqual(
                OnNext(5.Seconds(), "A"),
                OnNext(10.Seconds(), "quick"),
                OnNext(20.Seconds(), "brown"),
                OnNext(40.Seconds(), "fox"),
                OnNext(1.Minutes(), "jumps"),
                OnCompleted<string>(3.Minutes()));
        }
    }
}
