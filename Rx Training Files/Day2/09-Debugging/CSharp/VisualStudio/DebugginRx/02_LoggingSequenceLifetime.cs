using System;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Moq;
using NUnit.Framework;

namespace DebugginRx
{
    public static class ObservableLoggingExtensions
    {
        //TODO: Correct the implementation
        //[System.Diagnostics.DebuggerStepThrough]
        public static IObservable<T> Log<T>(this IObservable<T> source, ILogger logger, string name)
        {
            //return Observable.Create<T>(observer =>
            //           source.Materialize().Subscribe(notification =>
            //           {

            //               switch (notification.Kind)
            //               {
            //                   case NotificationKind.OnError:
            //                       logger.Log(string.Format("{0}.OnError(System.Exception: {1})", name, 
            //                           notification.Exception.Message));
            //                       break;
            //                   case NotificationKind.OnCompleted:
            //                       logger.Log(string.Format("{0}.{1}",name, notification.ToString()));
            //                       break;
            //                   default:
            //                       logger.Log(string.Format("{0}.{1}", name, notification.ToString()));
            //                       break;

            //               }


            //               notification.Accept(observer);
            //           }));



            return Observable.Create<T>(observer =>
            {
                logger.Log(string.Format("{0}.Subscribe()", name));
                var timer = new Timer(logger, name);

                //NOTE : See notes below
                //Timer timer = null;
                var obsSubsDisp = source.Subscribe(
                    (x) =>
                    {
                        //NOTE : See notes below
                        //if (timer == null)
                        //{
                        //    timer = new Timer(logger, name);
                        //}

                        logger.Log(string.Format("{0}.OnNext({1})", name, x.ToString()));
                        observer.OnNext(x);
                    },
                    (ex) =>
                    {
                        logger.Log(string.Format("{0}.OnError({1}: {2})", name, ex.GetType().ToString(), ex.Message));
                        observer.OnError(ex);
                    },
                    () =>
                    {
                        logger.Log(string.Format("{0}.OnCompleted()", name));
                        observer.OnCompleted();
                    }
                    );

                var loggingActionDisp = Disposable.Create(() =>
                {
                    logger.Log(string.Format("{0}.Dispose()",name));
                        
                });

                //compositeDisposable takes a copy, so beware, this is why the timer.Dispose() wasn't called when 
                //setup in the Subscribe(...) method. Essentially it was the initial null value of the timer
                return new CompositeDisposable(obsSubsDisp, loggingActionDisp, timer);
            });
        }

        public static IDisposable Time(this ILogger logger, string name)
        {
            return new Timer(logger, name);
        }

        private sealed class Timer : IDisposable
        {
            private readonly ILogger _logger;
            private readonly string _name;
            private readonly Stopwatch _stopwatch;

            public Timer(ILogger logger, string name)
            {
                _logger = logger;
                _name = name;
                _stopwatch = Stopwatch.StartNew();
            }

            public void Dispose()
            {
                _stopwatch.Stop();
                _logger.Log(string.Format("{0} took {1}", _name, _stopwatch.Elapsed));
            }
        }
    }

    [TestFixture]
    public class LoggingSequenceLifetime
    {
        //Don't modify me. Update the implementation of ObservableLoggingExtensions.Log(..) to satisfy the test
        [Test]
        public void GIVEN_a_logged_observable_sequence_WHEN_values_are_produced_THEN_values_are_logged()
        {
            var source = Observable.Range(0, 5);
            var logger = new Mock<ILogger>();

            //Act
            source
                .Log(logger.Object, "MySeq")
                .Subscribe();

            //Assert
            logger.Verify(l => l.Log("MySeq.OnNext(0)"));
            logger.Verify(l => l.Log("MySeq.OnNext(1)"));
            logger.Verify(l => l.Log("MySeq.OnNext(2)"));
            logger.Verify(l => l.Log("MySeq.OnNext(3)"));
            logger.Verify(l => l.Log("MySeq.OnNext(4)"));
        }

        //Don't modify me. Update the implementation of ObservableLoggingExtensions.Log(..) to satisfy the test
        [Test]
        public void GIVEN_a_logged_observable_sequence_WHEN_sequence_completes_THEN_completion_is_logged()
        {
            var source = Observable.Range(0, 5);
            var logger = new Mock<ILogger>();

            //Act
            source
                .Log(logger.Object, "MySeq")
                .Subscribe();

            //Assert
            logger.Verify(l => l.Log("MySeq.OnCompleted()"));
        }

        //Don't modify me. Update the implementation of ObservableLoggingExtensions.Log(..) to satisfy the test
        [Test]
        public void GIVEN_a_logged_observable_sequence_WHEN_sequence_errors_THEN_error_is_logged()
        {
            var source = Observable.Range(0, 5).Concat(Observable.Throw<int>(new Exception("Fail!")));
            var logger = new Mock<ILogger>();

            //Act
            source
                .Log(logger.Object, "MySeq")
                .Subscribe(x => { }, ex => { });

            //Assert
            logger.Verify(l => l.Log("MySeq.OnError(System.Exception: Fail!)"));
        }

        //Don't modify me. Update the implementation of ObservableLoggingExtensions.Log(..) to satisfy the test
        [Test]
        public void GIVEN_a_logged_observable_sequence_WHEN_subscription_is_made_THEN_subscription_is_logged()
        {
            var source = Observable.Range(0, 5);
            var logger = new Mock<ILogger>();

            //Act
            source
                .Log(logger.Object, "MySeq")
                .Subscribe();

            //Assert
            logger.Verify(l => l.Log("MySeq.Subscribe()"));
        }

        //Don't modify me. Update the implementation of ObservableLoggingExtensions.Log(..) to satisfy the test
        [Test]
        public void GIVEN_a_logged_observable_sequence_WHEN_subscription_is_disposed_THEN_disposal_is_logged()
        {
            var source = Observable.Range(0, 5);
            var logger = new Mock<ILogger>();

            //Act
            source
                .Log(logger.Object, "MySeq")
                .Subscribe()
                .Dispose();

            //Assert
            logger.Verify(l => l.Log("MySeq.Dispose()"));
        }

        //Don't modify me. Update the implementation of ObservableLoggingExtensions.Log(..) to satisfy the test
        [Test]
        public void GIVEN_a_logged_observable_sequence_WHEN_subscription_is_disposed_THEN_time_of_subscription_is_logged()
        {
            var source = Observable.Range(0, 5);
            var logger = new Mock<ILogger>();

            //Act
            source
                .Log(logger.Object, "MySeq")
                .Subscribe()
                .Dispose();

            //Assert
            logger.Verify(l => l.Log(It.Is<string>(value => value.StartsWith("MySeq took "))));
        }

    }
}