using System.Linq;
using NUnit.Framework;

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

            CollectionAssert.AreEqual(new[] { 'A', 'B', 'C', 'D', 'E' }, actual.ToArray());
        }

        [Test]
        public void TestOnlyEven()
        {
            var actual = Enumerable.Range(0, 5);

            CollectionAssert.AreEqual(new[] { 0, 2, 4 }, actual.ToArray());
        }

        [Test]
        public void TestTakeFirst3()
        {
            var actual = Enumerable.Range(0, 5);

            CollectionAssert.AreEqual(new[] { 0, 1, 2 }, actual.ToArray());
        }

        [Test]
        public void TestSum()
        {
            var actual = Enumerable.Range(0, 5);

            Assert.AreEqual(10, actual);
        }

        [Test]
        public void TestRunningSum()
        {
            var actual = Enumerable.Range(0, 5);

            CollectionAssert.AreEqual(new[] { 0, 1, 3, 6, 10 }, actual.ToArray());
        }
    }
}
