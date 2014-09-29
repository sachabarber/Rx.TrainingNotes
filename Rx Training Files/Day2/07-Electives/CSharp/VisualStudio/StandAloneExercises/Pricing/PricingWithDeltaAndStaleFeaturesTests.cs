using System;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Text;
using System.Collections.Generic;
using Microsoft.Reactive.Testing;
using NUnit.Framework;

namespace StandAloneExercises.Pricing
{
    /// <summary>
    /// In these tests, show that you can convert a basic price feed into one 
    /// that calculates the delta and will automatically go stale after a 
    /// specified period.
    /// </summary>
    [TestFixture]
    public class PricingWithDeltaAndStaleFeaturesTests : ReactiveTest
    {
        private TestScheduler _testScheduler;
        private ITestableObserver<PriceTick> _observer;
        private static readonly TimeSpan StaleTimeout = TimeSpan.FromSeconds(2);
        private IObservable<PriceDto> _priceFeed;

        [SetUp]
        public void SetUp()
        {
            _testScheduler = new TestScheduler();
            _observer = _testScheduler.CreateObserver<PriceTick>();
            _priceFeed = _testScheduler.CreateHotObservable(
                OnNext(0.1.Seconds(), new PriceDto { Price = 100 }),
                OnNext(1.1.Seconds(), new PriceDto { Price = 110 }),
                OnNext(4.0.Seconds(), new PriceDto { Price = 105 }),
                OnNext(4.5.Seconds(), new PriceDto { Price = 101 }),
                OnNext(5.0.Seconds(), new PriceDto { Price = 99 }),
                OnNext(5.1.Seconds(), new PriceDto { Price = 103 }));
        }

        [Test]
        public void PriceFeedIsEnrichedWithDeltaFromPreviousPrice()
        {
            //TODO: Put implementation here
            var actual = Observable.Empty<PriceTick>();

            actual.Subscribe(_observer);
            _testScheduler.Start();

            _observer.Messages.AssertEqual(
                OnNext(0.1.Seconds(), new PriceTick { Price = 100, Delta = 0 }),
                OnNext(1.1.Seconds(), new PriceTick { Price = 110, Delta = 10 }),
                OnNext(4.0.Seconds(), new PriceTick { Price = 105, Delta = -5 }),
                OnNext(4.5.Seconds(), new PriceTick { Price = 101, Delta = -4 }),
                OnNext(5.0.Seconds(), new PriceTick { Price = 099, Delta = -2 }),
                OnNext(5.1.Seconds(), new PriceTick { Price = 103, Delta = 4 }));
        }

        [Test]
        public void PriceFeedGoesStaleAfter2Seconds()
        {
            //In this test, you will need to inject a stale price into the sequence when there 
            //is no price produced for the given 'StaleTimeout' (2 seconds).
            //Take care to compare the input sequence times and the expected sequence times
            //(there is an item at 3.1 seconds in the expected that is not in the source sequence)

            //TODO: Put implementation here
            var actual = Observable.Empty<PriceTick>();

            actual.Subscribe(_observer);
            _testScheduler.Start();

            _observer.Messages.AssertEqual(
                OnNext(0.1.Seconds(), new PriceTick { Price = 100, IsValid = true, }),
                OnNext(1.1.Seconds(), new PriceTick { Price = 110, IsValid = true, }),
                OnNext(3.1.Seconds(), new PriceTick { Price = 110, IsValid = false, }),
                OnNext(4.0.Seconds(), new PriceTick { Price = 105, IsValid = true, }),
                OnNext(4.5.Seconds(), new PriceTick { Price = 101, IsValid = true, }),
                OnNext(5.0.Seconds(), new PriceTick { Price = 99, IsValid = true, }),
                OnNext(5.1.Seconds(), new PriceTick { Price = 103, IsValid = true, }),
                OnNext(7.1.Seconds(), new PriceTick { Price = 103, IsValid = false, }));
        }

        [Test]
        public void PriceFeedWithDeltaAndStaleFeatures()
        {
            //TODO: Put implementation here
            var actual = Observable.Empty<PriceTick>();

            actual.Subscribe(_observer);
            _testScheduler.Start();

            _observer.Messages.AssertEqual(
                OnNext(0.1.Seconds(), new PriceTick { Price = 100, Delta = 0, IsValid = true, }),
                OnNext(1.1.Seconds(), new PriceTick { Price = 110, Delta = 10, IsValid = true, }),
                OnNext(3.1.Seconds(), new PriceTick { Price = 110, Delta = 0, IsValid = false, }),
                OnNext(4.0.Seconds(), new PriceTick { Price = 105, Delta = -5, IsValid = true, }),
                OnNext(4.5.Seconds(), new PriceTick { Price = 101, Delta = -4, IsValid = true, }),
                OnNext(5.0.Seconds(), new PriceTick { Price = 99, Delta = -2, IsValid = true, }),
                OnNext(5.1.Seconds(), new PriceTick { Price = 103, Delta = 4, IsValid = true, }),
                OnNext(7.1.Seconds(), new PriceTick { Price = 103, Delta = 0, IsValid = false, }));
        }
    }
}
