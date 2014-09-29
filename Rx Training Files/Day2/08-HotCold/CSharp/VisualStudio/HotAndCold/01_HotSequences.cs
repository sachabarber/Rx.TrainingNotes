using System;
using System.ComponentModel;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using NUnit.Framework;

namespace HotAndCold
{
    [TestFixture]
    public class HotSequences
    {
        [Test]
        public void Events_are_hot()
        {
            var viewModel = new MyViewModel();

            viewModel.PropertyChanged += (s, e) => Console.WriteLine("First event handler : '{0}'", e.PropertyName);
            viewModel.Name = "Alex";

            //This event handler has registered too late. It has missed the change event for viewModel.Name = "Alex"
            viewModel.PropertyChanged += (s, e) => Console.WriteLine("Second event handler : '{0}'", e.PropertyName);
            viewModel.Age = 21;
        }
        [Test]
        public void Events_as_ObservableSequences_are_still_hot()
        {
            var viewModel = new MyViewModel();
            var source = Observable.FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(
                        h => viewModel.PropertyChanged += h,
                        h => viewModel.PropertyChanged -= h);

            source.Subscribe(propName=> Console.WriteLine("First event handler : '{0}'", propName));
            viewModel.Name = "Alex";

            //This event handler has registered too late. It has missed the change event for viewModel.Name = "Alex"
            source.Subscribe(propName => Console.WriteLine("Second event handler : '{0}'", propName));
            viewModel.Age = 21;
        }

        [Test]
        public void Subject_is_hot()
        {
            var source = new Subject<int>();

            source.Subscribe(x => Console.WriteLine("First event handler : '{0}'", x));
            source.OnNext(1);

            //This subscription is too late to see the value 1
            source.Subscribe(x => Console.WriteLine("Second event handler : '{0}'", x));
            source.OnNext(2);
        }
    }
}