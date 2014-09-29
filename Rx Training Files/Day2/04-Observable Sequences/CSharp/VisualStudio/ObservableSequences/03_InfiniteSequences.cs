using System;
using System.Reactive.Linq;
using Microsoft.Reactive.Testing;
using NUnit.Framework;

namespace ObservableSequences
{
    [TestFixture]
    public class InfiniteSequences : ReactiveTest
    {
        public IObservable<T> Generate<T>(T seed, Func<T, T> increment)
        {
            //TODO:
            throw new NotImplementedException("Implement me");
        }


        [Test]
        public void TestGenerate()
        {
            var observer = new TestScheduler().CreateObserver<int>();

            var query = Generate(1, i => i * 2).Take(10);
            query.Subscribe(observer);

            //CollectionAssert.AreEqual(new[] { 1, 2, 4, 8, 16, 32, 64, 128, 256, 512 }, actual);
            observer.Messages.AssertEqual(
                OnNext(0, 1),
                OnNext(0, 2),
                OnNext(0, 4),
                OnNext(0, 8),
                OnNext(0, 16),
                OnNext(0, 32),
                OnNext(0, 64),
                OnNext(0, 128),
                OnNext(0, 256),
                OnNext(0, 512),
                OnCompleted<int>(0));
        }

        [Test]
        public void TestSequence_before_error()
        {
            var observer = new TestScheduler().CreateObserver<int>();
            var query = Generate(4, i => i - 1).Select(i => 100 / i)
                                              .Take(4);

            query.Subscribe(observer);

            //CollectionAssert.AreEqual(expected, actual);
            observer.Messages.AssertEqual(
                OnNext(0, 25),
                OnNext(0, 33),
                OnNext(0, 50),
                OnNext(0, 100),
                OnCompleted<int>(0));
        }

        [Test]
        public void TestErrorSequences()
        {
            var observer = new TestScheduler().CreateObserver<int>();
            var query = Generate(4, i => i - 1).Select(i => 100 / i)
                                               .Take(5);

            query.Subscribe(observer);

            observer.Messages.AssertEqual(
               OnNext(0, 25),
               OnNext(0, 33),
               OnNext(0, 50),
               OnNext(0, 100),
               OnError<int>(0, ex => ex is DivideByZeroException));
        }
    }
}
