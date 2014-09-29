using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace IntroToSequences
{
    [TestFixture]
    public class CombiningSequences
    {
        [Test]
        public void Test_Concatenating_two_sequences()
        {
            var s1 = Enumerable.Range(0, 3);
            var s2 = Enumerable.Range(7, 2);

            var actual = s1.Concat(s2);

            CollectionAssert.AreEqual(new[]{0,1,2,7,8}, actual.ToArray());
        }

        [Test]
        public void Test_Concatenating_three_sequences()
        {
            var s1 = Enumerable.Range(0, 3);
            var s2 = Enumerable.Range(10, 3);
            var s3 = Enumerable.Range(20, 3);

            var actual = s1.Concat(s2).Concat(s3);

            CollectionAssert.AreEqual(new[] { 0, 1, 2, 10, 11, 12, 20, 21, 22 }, actual.ToArray());
        }

        [Test]
        public void Test_Zipping_two_size_equal_sequences()
        {
            var s1 = Enumerable.Range(0, 5);
            var s2 = Enumerable.Range(0, 5).Select(i => (char)(i + 65));

            var actual = s1.Zip(s2, Tuple.Create);

            CollectionAssert.AreEqual(new[]
                {
                    Tuple.Create(0,'A'),
                    Tuple.Create(1,'B'),
                    Tuple.Create(2,'C'),
                    Tuple.Create(3,'D'),
                    Tuple.Create(4,'E')
                }, actual.ToArray());
        }

        [Test]
        public void Test_CartesianProduct()
        {
            var numbers = Enumerable.Range(1, 3);
            var letters = new[] { 'x', 'y', 'z' };

            var actual = numbers.SelectMany(num => letters, Tuple.Create);

            CollectionAssert.AreEqual(new[] {
			        Tuple.Create(1,'x'),
			        Tuple.Create(1,'y'),
			        Tuple.Create(1,'z'),
			        Tuple.Create(2,'x'),
			        Tuple.Create(2,'y'),
			        Tuple.Create(2,'z'),
			        Tuple.Create(3,'x'),
			        Tuple.Create(3,'y'),
			        Tuple.Create(3,'z')
		        }, actual.ToArray());
        }

        [Test]
        public void Test_CartesianProduct_Query_Comprehension_syntax()
        {
            var numbers = Enumerable.Range(1, 3);
            var letters = new[] { 'x', 'y', 'z' };

            var actual = from num in numbers
                         from letter in letters
                         select Tuple.Create(num, letter);

            CollectionAssert.AreEqual(new[] {
			        Tuple.Create(1,'x'),
			        Tuple.Create(1,'y'),
			        Tuple.Create(1,'z'),
			        Tuple.Create(2,'x'),
			        Tuple.Create(2,'y'),
			        Tuple.Create(2,'z'),
			        Tuple.Create(3,'x'),
			        Tuple.Create(3,'y'),
			        Tuple.Create(3,'z')
		        }, actual.ToArray());
        }

        [Test]
        public void Test_Sequence_Join()
        {
            var s1 = new[]{
		        Tuple.Create(0,"zero"),
		        Tuple.Create(1,"one"),
		        Tuple.Create(2,"two"),
		        Tuple.Create(3,"three"),
	        };

            var s2 = new[] {
		        Tuple.Create(1,"uno"),
		        Tuple.Create(2,"dos"),
		        Tuple.Create(3,"tres"),
		        Tuple.Create(4,"cuatro"),
	        };

            var actual = s1.Join(s2, eng => eng.Item1, esp => esp.Item1, (eng, esp) => Tuple.Create(eng.Item2, esp.Item2));

            CollectionAssert.AreEqual(
                new[] {
			        Tuple.Create("one","uno"),
			        Tuple.Create("two","dos"),
			        Tuple.Create("three","tres")
		        },
                actual.ToArray());
        }

        [Test]
        public void Test_Sequence_Join_QCS()
        {
            var s1 = new[]{
		        Tuple.Create(0,"zero"),
		        Tuple.Create(1,"one"),
		        Tuple.Create(2,"two"),
		        Tuple.Create(3,"three")
	        };
            var s2 = new[] {
		        Tuple.Create(1,"uno"),
		        Tuple.Create(2,"dos"),
		        Tuple.Create(3,"tres"),
		        Tuple.Create(4,"cuatro")
	        };

            var actual = from eng in s1
                         from esp in s2
                         where eng.Item1 == esp.Item1
                         select Tuple.Create(eng.Item2, esp.Item2);

            CollectionAssert.AreEqual(
                new[] {
			        Tuple.Create("one","uno"),
			        Tuple.Create("two","dos"),
			        Tuple.Create("three","tres")
		        },
                actual.ToArray());
        }

        [Test, Timeout(1000)]
        public void Test_FastBySlow_join()
        {
            var fast = Enumerable.Range(1, 3);
            var slow = SlowIntegers().Take(3);

            var actual = from x in slow
                         from y in fast
                         orderby y
                         select new { fast = y, slow = x };

            CollectionAssert.AreEqual(
                new[] {
			        new { fast = 1, slow = 1 },
			        new { fast = 1, slow = 2 },
			        new { fast = 1, slow = 3 },
			        new { fast = 2, slow = 1 },
			        new { fast = 2, slow = 2 },
			        new { fast = 2, slow = 3 },
			        new { fast = 3, slow = 1 },
			        new { fast = 3, slow = 2 },
			        new { fast = 3, slow = 3 }
		        },
                actual.ToArray());
        }


        private static IEnumerable<int> SlowIntegers()
        {
            var i = 1;
            while (true)
            {
                Thread.Sleep(200);
                yield return i++;
            }
        }
    }
}