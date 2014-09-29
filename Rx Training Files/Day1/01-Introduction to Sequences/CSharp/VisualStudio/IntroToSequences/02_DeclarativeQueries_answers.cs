using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IntroToSequences
{
    [TestClass]
    public class DeclarativeQueries
    {
        [TestMethod]
        public void TestAdd5()
        {
            var actual = Enumerable.Range(0, 5)
                                   .Select(i => i + 5);

            CollectionAssert.AreEqual(new[] { 5, 6, 7, 8, 9 }, actual.ToArray());
        }

        [TestMethod]
        public void TestToChar()
        {
            var actual = Enumerable.Range(0, 5).Select(i=>(char)(i+65));

            CollectionAssert.AreEqual(new[] { 'A', 'B', 'C', 'D', 'E' }, actual.ToArray());
        }

        [TestMethod]
        public void TestOnlyEven()
        {
            var actual = Enumerable.Range(0, 5).Where(i=>i%2==0);

            CollectionAssert.AreEqual(new[] { 0, 2, 4 }, actual.ToArray());
        }

        [Test]
        public void TestTakeFirst3()
        {
            var actual = Enumerable.Range(0, 5).Take(3);

            CollectionAssert.AreEqual(new[] { 0, 1, 2 }, actual.ToArray());
        }

        [TestMethod]
        public void TestSum()
        {
            var actual = Enumerable.Range(0, 5).Sum();

            Assert.AreEqual(10, actual);
        }

        [TestMethod]
        public void TestRunningSum()
        {
            var actual = Enumerable.Range(0, 5).Scan(0, (accumulator, currentValue)=>accumulator+currentValue);

            CollectionAssert.AreEqual(new[] { 0, 1, 3, 6, 10 }, actual.ToArray());
        }
    }
}
