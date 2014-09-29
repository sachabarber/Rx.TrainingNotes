using System;
using System.Reactive.Linq;
using NUnit.Framework;

namespace ObservableSequences
{
    [TestFixture]
    public class ProjectingWithSelect
    {
        private IObservable<string> _sourceSequence;

        [SetUp]
        public void SetUp()
        {
            _sourceSequence = new[] {"One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten"}.ToObservable();
        }

        [Test]
        public async void Using_Select_to_project_to_another_type()
        {
            //Takes the first character of each word
            var query = _sourceSequence.Select(word => word[0]);

            var actual = await query.ToArray().SingleAsync();

            CollectionAssert.AreEqual(new[] { 'O', 'T', 'T', 'F', 'F', 'S' , 'S', 'E', 'N', 'T'}, actual);
        }

        [Test]
        public async void Using_SelectMany_to_flatten_a_sequence()
        {
            //If a string is actually an array of chars then we can use SelectMany to flatten the sequence of char sequences to just a sequence of chars.
            var query = _sourceSequence.SelectMany(word => word);

            var actual = await query.ToArray().SingleAsync();

            CollectionAssert.AreEqual(new[] { 
                'O', 'n', 'e', 
                'T', 'w', 'o', 
                'T', 'h', 'r', 'e', 'e', 
                'F', 'o', 'u', 'r', 
                'F', 'i', 'v', 'e', 
                'S', 'i', 'x', 
                'S', 'e', 'v', 'e', 'n', 
                'E', 'i', 'g', 'h', 't', 
                'N', 'i', 'n', 'e', 
                'T', 'e', 'n'
                }, actual);
        }

        [Test]
        public async void Using_Select_to_get_index()
        {
            //Filter out every other word, by using Select to extract the index of each element
            var query = _sourceSequence.Select((word,idx)=> new{word,idx})
                                       .Where(x=>x.idx%2==0)
                                       .Select(x=>x.word);

            var actual = await query.ToArray().SingleAsync();
            CollectionAssert.AreEqual(
                new[] {"One", "Three", "Five", "Seven", "Nine"},
                actual);
        }
    }
}