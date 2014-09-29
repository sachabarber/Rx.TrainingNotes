using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace IntroToSequences
{
    [TestFixture]
    public class InfiniteSequences
    {
        [Test]
        public void TestAllPositiveIntegers()
        {
            var actual = AllPositiveIntegers().Take(10).ToArray();

            CollectionAssert.AreEqual(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, actual);
        }

        public IEnumerable<int> AllPositiveIntegers()
        {
            var i = 1;
            while (true)
            {
                yield return i++;
            }
        }

        [Test]
        public void TestGenerate()
        {
            var actual = Generate(1, i => i * 2).Take(10).ToArray();
            CollectionAssert.AreEqual(new[] { 1, 2, 4, 8, 16, 32, 64, 128, 256, 512 }, actual);
        }

        public IEnumerable<T> Generate<T>(T seed, Func<T, T> increment)
        {
            yield return seed;
            while(true)
            {
                seed = increment(seed);
                yield return seed;
            }
        }

        [Test]
        public void TestSequence_before_error()
        {
            var expected = new[] { 25, 33, 50, 100 };
            var actual = Generate(4, i => i - 1).Select(i => 100 / i)
                                              .Take(4)
                                              .ToArray();

            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestErrorSequences()
        {
            Assert.Throws<DivideByZeroException>(
                () => Generate(4, i => i - 1).Select(i => 100 / i)
                                            .Take(5)
                                            .ToArray());
        }

    }
}
