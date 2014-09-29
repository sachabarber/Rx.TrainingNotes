using System;
using System.Diagnostics;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace ObservableSequences
{
    public static class ObsEx
    {
        public static IObservable<T> Log<T>(this IObservable<T> source, string name)
        {
            return Observable.Create<T>(
                o =>
                {
                    Console.WriteLine("{0}.Subscribe()", name);

                    var timer = Stopwatch.StartNew();
                    var timerLog = Disposable.Create(() =>
                                                     {
                                                         timer.Stop();
                                                         Console.WriteLine("{0} took {1}", name, timer.Elapsed);
                                                     });

                    var disposal = Disposable.Create(() => Console.WriteLine("{0}.Dispose()", name));
                    var subscription = source.Subscribe(o);
                    return new CompositeDisposable(disposal, subscription, timerLog);
                });
        }
    }
}