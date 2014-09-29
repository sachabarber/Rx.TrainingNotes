using System;
using System.Diagnostics;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace DuplexWcfChat
{
    public static class WcfObservableExtensions
    {
        /// <summary>
        /// The IDisposable implementation in the Channel proxies is delegates to Close(). 
        /// Close can throw as it is not always valid to call.
        /// This methods attempts to protect the consumer so they can always call Dispose safely.
        /// </summary>
        /// <typeparam name="TChannel"></typeparam>
        /// <param name="channel"></param>
        /// <returns></returns>
        public static IDisposable CreateSafeDisposal<TChannel>(this TChannel channel) where TChannel : IChannel, IDisposable
        {
            return Disposable.Create(() =>
                                     {
                                         try
                                         {
                                             try
                                             {
                                                 //Console.WriteLine("Starting safe disposal of channel : {0}", channel.State);
                                                 if (channel.State != CommunicationState.Faulted)
                                                 {
                                                     //Console.WriteLine("Closing channel");
                                                     channel.Close(TimeSpan.FromSeconds(20));
                                                 }
                                             }
                                             finally
                                             {
                                                 if (channel.State != CommunicationState.Closed)
                                                 {
                                                     //Console.WriteLine("Aborting channel");
                                                     channel.Abort();
                                                 }
                                             }
                                         }
                                         catch (Exception e)
                                         {
                                             //Console.WriteLine("SafeDisposal of channel threw : {0}", e);
                                         }
                                     });
        }

        public static IObservable<TChannel> OpenSequence<TChannel>(this TChannel channel) where TChannel : IChannel
        {
            var opened = Observable.FromEventPattern(h => channel.Opened += h, h => channel.Opened -= h)
                //.Log("Channel.Opened")
                                   .Take(1)
                                   .Select(_ => channel);

            var alreadyOpen = Observable.Return(channel)
                //.Log("RawChannel", x=>x.State.ToString())
                                        .Where(c => c.State == CommunicationState.Opened);

            return Observable.Merge(opened, alreadyOpen)
                             .Take(1);
        }

        public static IObservable<TChannel> TerminationSequence<TChannel>(this TChannel channel) where TChannel : IChannel
        {
            return Observable.Create<TChannel>(o =>
                                               {
                                                   //When Channel closes, then complete the sequence.
                                                   var closed = Observable.FromEventPattern(h => channel.Closed += h, h => channel.Closed -= h)
                                                                          .Log("Channel.Closed")
                                                                          .Take(1)
                                                                          .IgnoreElements()
                                                                          .Cast<TChannel>();

                                                   //When Channel faults, then error the sequence.
                                                   var faulted = Observable.FromEventPattern(h => channel.Faulted += h, h => channel.Faulted -= h)
                                                                           .Log("Channel.Faulted")
                                                                           .SelectMany(ea => Observable.Throw<TChannel>(new ChannelTerminatedException()))
                                                                           .Take(1);

                                                   //If channel is already in terminal state, then propagate that.
                                                   if (channel.State == CommunicationState.Closed)
                                                       o.OnCompleted();
                                                   if (channel.State == CommunicationState.Faulted)
                                                       o.OnError(new ChannelTerminatedException());

                                                   return Observable.Merge(closed, faulted)
                                                                    .Subscribe(o);
                                               });
        }


        [System.Diagnostics.DebuggerStepThrough]
        public static IObservable<T> Log<T>(this IObservable<T> source, string name)
        {
            return source.Log(name, x => x.ToString());
        }

        public static IObservable<T> Log<T>(this IObservable<T> source, string name, Func<T, string> selector)
        {
            return source;
            //return Observable.Using(
            //    () => new Timer(name),
            //    timer => Observable.Create<T>(
            //        o =>
            //        {
            //            Console.WriteLine("{1:o} {0}.Subscribe()", name, DateTime.Now);
            //            var subscription = source
            //                .Do(
            //                    i => Console.WriteLine("{2:o} {0}.OnNext({1})", name, selector(i), DateTime.Now),
            //                    ex => Console.WriteLine("{2:o} {0}.OnError({1})", name, ex, DateTime.Now),
            //                    () => Console.WriteLine("{1:o} {0}.OnCompleted()", name, DateTime.Now))
            //                .Subscribe(o);
            //            var disposal = Disposable.Create(() => Console.WriteLine("{1:o} {0}.Dispose()", name, DateTime.Now));
            //            return new CompositeDisposable(subscription, disposal);
            //        })
            //    );
        }

        private sealed class Timer : IDisposable
        {
            private readonly string _name;
            private readonly Stopwatch _stopwatch;

            public Timer(string name)
            {
                _name = name;
                _stopwatch = Stopwatch.StartNew();
            }

            public void Dispose()
            {
                _stopwatch.Stop();
                Console.WriteLine("{0} took {1}", _name, _stopwatch.Elapsed);
            }
        }
    }
}