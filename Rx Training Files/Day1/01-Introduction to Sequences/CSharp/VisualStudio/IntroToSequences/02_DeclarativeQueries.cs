using System.Linq;
using NUnit.Framework;
using System;

namespace IntroToSequences
{
    [TestFixture]
    public class DeclarativeQueries
    {
        [Test]
        public void TestAdd5()
        {
            var actual = Enumerable.Range(0, 5)
                                   .Select(i => i + 5);

            CollectionAssert.AreEqual(new[] { 5, 6, 7, 8, 9 }, actual.ToArray());
        }

        [Test]
        public void TestToChar()
        {
            var actual = Enumerable.Range(0, 5);


            var final = actual.Select(x => Convert.ToChar((int)'A' + x));

            CollectionAssert.AreEqual(new[] { 'A', 'B', 'C', 'D', 'E' }, final.ToArray());
        }

        [Test]
        public void TestOnlyEven()
        {
            var actual = Enumerable.Range(0, 5);
            var final = actual.Where(x => x == 0 || x%2 ==0);
            CollectionAssert.AreEqual(new[] { 0, 2, 4 }, final.ToArray());
        }

        [Test]
        public void TestTakeFirst3()
        {
            var actual = Enumerable.Range(0, 5);
            var top3 = actual.Take(3);
            CollectionAssert.AreEqual(new[] { 0, 1, 2 }, top3.ToArray());
        }

        [Test]
        public void TestSum()
        {
            var actual = Enumerable.Range(0, 5);
            var finalSum = actual.Sum();
            Assert.AreEqual(10, finalSum);
        }

        [Test]
        public void TestRunningSum()
        {
            var actual = Enumerable.Range(0, 5);
            var result = actual.Scan(0, (x, y) => x + y);
            CollectionAssert.AreEqual(new[] { 0, 1, 3, 6, 10 }, result.ToArray());
        }
    }
}
