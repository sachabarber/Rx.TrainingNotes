using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace PracticalRx.TodoList.Contracts
{
    public static class ObservableExtensions
    {
        private static int _idx;
        public static IObservable<T> Trace<T>(this IObservable<T> source, string name)
        {
            return Observable.Create<T>(o =>
                {
                    var prefix = string.Format("{0:d4}.{1}", _idx++, name);
                    Console.WriteLine("{0}.Subscribe()", prefix);
                    return Observable.Using(() => Disposable.Create(() => Console.WriteLine("{0}.Dispose()", prefix)),
                        _ => source.Do(
                            x => Console.WriteLine("{0}.OnNext({1})", prefix, x),
                            ex => Console.WriteLine("{0}.OnError({1})", prefix, ex),
                            () => Console.WriteLine("{0}.OnCompleted()", prefix))
                        )
                        .Subscribe(o);
                });
        }

    }
}