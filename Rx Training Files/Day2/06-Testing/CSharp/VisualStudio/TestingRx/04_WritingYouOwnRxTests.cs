using System;
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

            //TODO: You need to replace Observable.Never<int>() with your query to make the unit test pass.
            var source = Observable.Never<int>();
            
            source.Subscribe(observer);
            scheduler.Start();

            //Note that the AssertEqual extension method comes from the Microsoft.Reactive.Testing namespace.
            //Note that the OnNext/OnCompleted factory methods come from ReactiveTest (which this fixture sub-classes). You could also use them statically like ReactiveTest.OnNext(0.Seconds(), 1)
            observer.Messages.AssertEqual(
                OnNext(0.Seconds(), 1),
                OnNext(2.Seconds(), 2),
                OnCompleted<int>(2.Seconds()));
        }


        //TODO: Introduce the concept of the cold subscription +1 cost
        //  var source = Observable.Timer(TimeSpan.Zero, TimeSpan.FromSeconds(2), scheduler).Select(i => (int)i + 1).Take(2);   //[OnNext(1)@1, OnNext(2)@20000001, OnCompleted()@20000001]
        //  vs
        //  var source = Observable.Return(1).Concat(Observable.Return(2).Delay(TimeSpan.FromSeconds(2), scheduler));           //[OnNext(1)@0, OnNext(2)@20000000, OnCompleted()@20000001]
        //  vs
        //  var source = Observable.Return(1).Concat(Observable.Timer(TimeSpan.FromSeconds(2), scheduler).Select(_ => 2));      //[OnNext(1)@0, OnNext(2)@20000000, OnCompleted()@20000000]



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


            //TODO: Instead of creating the source with standard Rx operators, create a pre-canned sequence with the Rx testing operators
            IObservable<string> source = null;
            

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
