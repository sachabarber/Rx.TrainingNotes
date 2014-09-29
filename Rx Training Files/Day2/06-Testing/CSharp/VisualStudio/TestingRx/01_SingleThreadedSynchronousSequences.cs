using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using NUnit.Framework;

namespace TestingRx
{
    [TestFixture]
    public class SingleThreadedSynchronousSequences
    {
        [Test]
        public void TestingReturn()
        {
            var expected = 42;
            var actualValues = new List<int>();

            var source = Observable.Return(expected);

            source.Subscribe(actualValues.Add);

            CollectionAssert.AreEqual(new[]{expected}, actualValues);
        }

        [Test]
        public void TestingCreate()
        {
            var expected = 42;
            var actualValues = new List<int>();
            var source = Observable.Create<int>(o =>
                                                {
                                                    o.OnNext(42); 
                                                    return Disposable.Empty;
                                                });

            source.Subscribe(actualValues.Add);

            CollectionAssert.AreEqual(new[] { expected }, actualValues);
        }

        [Test]
        public void TestingCreateAgain()
        {
            var expected = 42;
            var actualValues = new List<int>();
            var source = Observable.Create<int>(o =>
            {
                o.OnNext(42);
                o.OnCompleted();
                return Disposable.Empty;
            });

            source.Subscribe(actualValues.Add);

            //Note that the assert is the same, but this version completes.
            CollectionAssert.AreEqual(new[] { expected }, actualValues);
        }

        [Test]
        public void TestingCreateAgainWithOnComplete()
        {
            var expected = 42;
            var actualValues = new List<int>();
            var source = Observable.Create<int>(o =>
            {
                o.OnNext(42);
                o.OnCompleted();
                return Disposable.Empty;
            });
            var hasCompleted = false;
            source.Subscribe(actualValues.Add, ()=>hasCompleted=true);

            //Note that the assert is the same, but this version completes.
            CollectionAssert.AreEqual(new[] { expected }, actualValues);
            //Here we have the extra flag to check for completion.
            Assert.That(hasCompleted);
        }
    }
}
