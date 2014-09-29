using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using Microsoft.Reactive.Testing;
using NUnit.Framework;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using CollectionAssert = Microsoft.VisualStudio.TestTools.UnitTesting.CollectionAssert;
using System;

namespace ObservableSequences
{
    [TestFixture]
    public class DeclarativeQueries
    {
        [Test]
        public async void TestAdd5()
        {
            var query = Observable.Range(0, 5)
                                  .Select(i => i + 5);
            
            var actual = await query.ToArray().SingleAsync();

            CollectionAssert.AreEqual(new[] { 5, 6, 7, 8, 9 }, actual);
        }

        [Test]
        public async void TestToChar()
        {
            var query = Observable.Range(0, 5).Select(x =>  Convert.ToChar((int)'A' + x));

            var actual = await query.ToArray().SingleAsync();

            CollectionAssert.AreEqual(new[] {'A', 'B', 'C', 'D', 'E'}, actual);
        }

        [Test]
        public async void TestOnlyEven()
        {
            var query = Observable.Range(0, 5).Where(x => x == 0 || x % 2 == 0);

            var actual = await query.ToArray().SingleAsync();

            CollectionAssert.AreEqual(new[] {0, 2, 4}, actual);
        }

        [Test]
        public async void TestTakeFirst3()
        {
            var query = Observable.Range(0, 5).Take(3);

            var actual = await query.ToArray().SingleAsync();

            CollectionAssert.AreEqual(new[] {0, 1, 2}, actual);
        }

        [Test]
        public async void TestSum()
        {
            var query = Observable.Range(0, 5).Sum();

            var actual = await query;

            Assert.AreEqual(10, actual);
        }

        [Test]
        public async void TestRunningSum()
        {
            var query = Observable.Range(0, 5);

            var actual = await query.ToArray().SingleAsync();

            CollectionAssert.AreEqual(new[] {0, 1, 3, 6, 10}, actual.ToArray());
        }

        public static ITestableObserver<T> CreateObserver<T>()
        {
            return new TestScheduler().CreateObserver<T>();
        }

        public static List<T> MessageValues<T>(ITestableObserver<T> source)
        {
            return source.Messages
                         .Where(m => m.Value.Kind == NotificationKind.OnNext)
                         .Select(m => m.Value.Value).ToList();
        }
    }
}
