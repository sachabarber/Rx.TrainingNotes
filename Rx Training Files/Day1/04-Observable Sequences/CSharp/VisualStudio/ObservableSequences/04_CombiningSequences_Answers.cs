using System;
using System.Reactive.Linq;
using Microsoft.Reactive.Testing;
using NUnit.Framework;

namespace ObservableSequences
{
    [TestFixture]
    public class CombiningSequences_Answers : ReactiveTest
    {
        [Test]
        public void Test_Concatenating_two_sequences()
        {
            var observer = new TestScheduler().CreateObserver<int>();

            var s1 = Observable.Range(0, 3);
            var s2 = Observable.Range(7, 2);

            var query = s1.Concat(s2);

            query.Subscribe(observer);

            observer.Messages.AssertEqual(
                OnNext(0, 0),
                OnNext(0, 1),
                OnNext(0, 2),
                OnNext(0, 7),
                OnNext(0, 8),
                OnCompleted<int>(0));
        }

        [Test]
        public void Test_Concatenating_three_sequences()
        {
            var observer = new TestScheduler().CreateObserver<int>();

            var s1 = Observable.Range(0, 3);
            var s2 = Observable.Range(10, 3);
            var s3 = Observable.Range(20, 3);

            var query = s1.Concat(s2).Concat(s3);

            query.Subscribe(observer);

            observer.Messages.AssertEqual(
                OnNext(0, 0),
                OnNext(0, 1),
                OnNext(0, 2),
                OnNext(0, 10),
                OnNext(0, 11),
                OnNext(0, 12),
                OnNext(0, 20),
                OnNext(0, 21),
                OnNext(0, 22),
                OnCompleted<int>(0));
        }

        [Test]
        public void Test_Zipping_two_size_equal_sequences()
        {
            var observer = new TestScheduler().CreateObserver<Tuple<int, char>>();
            var s1 = Observable.Range(0, 5);
            var s2 = Observable.Range(0, 5).Select(i => (char)(i + 65));

            var query = s1.Zip(s2, Tuple.Create);

            query.Subscribe(observer);

            observer.Messages.AssertEqual(
                OnNext(0, Tuple.Create(0, 'A')),
                OnNext(0, Tuple.Create(1, 'B')),
                OnNext(0, Tuple.Create(2, 'C')),
                OnNext(0, Tuple.Create(3, 'D')),
                OnNext(0, Tuple.Create(4, 'E')),
                OnCompleted<Tuple<int, char>>(0));
        }

        [Test]
        public void Test_CartesianProduct()
        {
            var scheduler = new TestScheduler();
            var observer = scheduler.CreateObserver<Tuple<int, char>>();
            var numbers = Observable.Interval(TimeSpan.FromMilliseconds(50), scheduler).Take(3).Select(i => (int)(i + 1));//Observable.Range(1, 3);
            var letters = new[] { 'x', 'y', 'z' }.ToObservable(scheduler);

            var query = numbers.SelectMany(num => letters, Tuple.Create);

            query.Subscribe(observer);
            scheduler.Start();

            observer.Messages.AssertEqual(
                OnNext(500001, Tuple.Create(1, 'x')),
                OnNext(500002, Tuple.Create(1, 'y')),
                OnNext(500003, Tuple.Create(1, 'z')),
                OnNext(1000001, Tuple.Create(2, 'x')),
                OnNext(1000002, Tuple.Create(2, 'y')),
                OnNext(1000003, Tuple.Create(2, 'z')),
                OnNext(1500001, Tuple.Create(3, 'x')),
                OnNext(1500002, Tuple.Create(3, 'y')),
                OnNext(1500003, Tuple.Create(3, 'z')),
                OnCompleted<Tuple<int, char>>(1500004));
        }

        [Test]
        public void Test_CartesianProduct_Query_Comprehension_syntax()
        {
            var scheduler = new TestScheduler();
            var observer = scheduler.CreateObserver<Tuple<int, char>>();
            var numbers = Observable.Interval(TimeSpan.FromMilliseconds(50), scheduler).Take(3).Select(i => (int)(i + 1));//Observable.Range(1, 3);
            var letters = new[] { 'x', 'y', 'z' }.ToObservable(scheduler);

            var query = from num in numbers
                        from letter in letters
                        select Tuple.Create(num, letter);

            query.Subscribe(observer);
            scheduler.Start();

            observer.Messages.AssertEqual(
                OnNext(500001, Tuple.Create(1, 'x')),
                OnNext(500002, Tuple.Create(1, 'y')),
                OnNext(500003, Tuple.Create(1, 'z')),
                OnNext(1000001, Tuple.Create(2, 'x')),
                OnNext(1000002, Tuple.Create(2, 'y')),
                OnNext(1000003, Tuple.Create(2, 'z')),
                OnNext(1500001, Tuple.Create(3, 'x')),
                OnNext(1500002, Tuple.Create(3, 'y')),
                OnNext(1500003, Tuple.Create(3, 'z')),
                OnCompleted<Tuple<int, char>>(1500004));
        }

        [Test]
        public void Test_Sequence_Join_QCS()
        {
            var observer = new TestScheduler().CreateObserver<Tuple<string, string>>();
            var s1 = new[]{
		        Tuple.Create(0,"zero"),
		        Tuple.Create(1,"one"),
		        Tuple.Create(2,"two"),
		        Tuple.Create(3,"three")
	        }.ToObservable();
            var s2 = new[] {
		        Tuple.Create(1,"uno"),
		        Tuple.Create(2,"dos"),
		        Tuple.Create(3,"tres"),
		        Tuple.Create(4,"cuatro"),
	        }.ToObservable();

            var query = from eng in s1
                        from esp in s2
                        where eng.Item1 == esp.Item1
                        select Tuple.Create(eng.Item2, esp.Item2);

            query.Subscribe(observer);

            observer.Messages.AssertEqual(
                    OnNext(0, Tuple.Create("one", "uno")),
                    OnNext(0, Tuple.Create("two", "dos")),
                    OnNext(0, Tuple.Create("three", "tres")),
                    OnCompleted<Tuple<string, string>>(0));
        }

        [Test]
        public async void Test_FastBySlow_join()
        {

            var fast = Observable.Range(1, 3);
            var slow = SlowIntegers().Take(3);

            var actual = from x in fast
                         from y in slow
                         select new { fast = x, slow = y };

            await actual.Log("Fast by slow");
        }

        [Test]
        public async void Test_SlowByFast_join()
        {
            var fast = Observable.Range(1, 3);
            var slow = SlowIntegers().Take(3);

            var actual = from x in slow
                         from y in fast
                         //orderby y 	//Not supported.
                         select new { fast = y, slow = x };

            await actual.Log("Slow by fast");
        }

        private IObservable<int> SlowIntegers()
        {
            return Observable.Interval(TimeSpan.FromMilliseconds(200)).Select(l => (int)l);
        }
    }
}
