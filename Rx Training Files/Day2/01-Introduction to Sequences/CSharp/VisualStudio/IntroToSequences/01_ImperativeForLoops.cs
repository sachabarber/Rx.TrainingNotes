﻿using System;
using NUnit.Framework;

namespace IntroToSequences
{
    [TestFixture]
    public class ImperativeForLoops
    {
        private int[] _source;

        [SetUp]
        public void Setup()
        {
            _source = new int[5] { 0, 1, 2, 3, 4 };
        }

        [Test]
        public void TestArrayAdd5()
        {
            var actual = ArrayAdd5(_source);
            CollectionAssert.AreEqual(new[] { 5, 6, 7, 8, 9 }, actual);
        }

        public int[] ArrayAdd5(int[] source)
        {
            //Take the sequence [0,1,2,3,4]
            // and create the sequence [5,6,7,8,9]
            var output = new int[source.Length];
            for (int i = 0; i < source.Length; i++)
            {
                output[i] = source[i] + 5;
            }
            return output;
        }
        
        [Test]
        public void TestArrayIntToArrayChar()
        {
            var actual = ArrayIntToArrayChar(_source);
            CollectionAssert.AreEqual(new[] { 'A', 'B', 'C', 'D', 'E' }, actual);
        }

        public char[] ArrayIntToArrayChar(int[] source)
        {
            //Take the sequence [0,1,2,3,4]
            // and create the sequence [A,B,C,D,E]
            var output = new char[source.Length];
            for (int i = 0; i < source.Length; i++)
            {
                output[i] = default(char); //TODO: Correct this line
            }
            return output;
        }

        [Test]
        public void TestArrayIntToArrayEvenInt()
        {
            var actual = ArrayIntToArrayEvenInt(_source);
            CollectionAssert.AreEqual(new[] { 0, 2, 4 }, actual);
        }

        public int[] ArrayIntToArrayEvenInt(int[] source)
        {
            //Take the sequence [0,1,2,3,4]
            // and create the sequence [0,2,4]

            for (int i = 0; i < source.Length; i++)
            {

            }

            return default(int[]);
        }

        [Test]
        public void TestArrayFirst3()
        {
            var actual = TakeFirst3(_source);
            CollectionAssert.AreEqual(new[] { 0, 1, 2 }, actual);
        }

        public int[] TakeFirst3(int[] source)
        {
            //Take the sequence [0,1,2,3,4]
            // and create the sequence [0,1,2]
            return default(int[]);
        }

        [Test]
        public void TestArrayIntToSum()
        {
            var actual = ArrayIntToSum(_source);
            Assert.AreEqual(10, actual);
        }

        public int ArrayIntToSum(int[] source)
        {
            //Take the sequence [0,1,2,3,4]
            // and return the value 10

            for (int i = 0; i < source.Length; i++)
            {

            }
            return default(int);
        }

        [Test]
        public void TestArrayIntToRunningSum()
        {
            var actual = ArrayIntToRunningSum(_source);
            CollectionAssert.AreEqual(new[] { 0, 1, 3, 6, 10 }, actual);
        }

        public int[] ArrayIntToRunningSum(int[] source)
        {
            //Take the sequence [0,1,2,3,4]
            // and return the value c

            for (int i = 0; i < source.Length; i++)
            {

            }
            return default(int[]);
        }
    }
}
