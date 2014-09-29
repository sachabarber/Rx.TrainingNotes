using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using Microsoft.Reactive.Testing;
using NUnit.Framework;

namespace StandAloneExercises.Pricing
{
    /// <summary>
    /// In these tests, implement a solution that will automatically change
    /// to the correct price feed when the user changes price groups.
    /// </summary>
    [TestFixture]
    public class PricingWithChangingPriceGroups : ReactiveTest
    {
        private TestScheduler _testScheduler;
        private ITestableObserver<PriceTick> _observer;
        private IObservable<PriceDto> _groupAPriceFeed;
        private IObservable<PriceDto> _groupBPriceFeed;

        [SetUp]
        public void SetUp()
        {
            _testScheduler = new TestScheduler();
            _observer = _testScheduler.CreateObserver<PriceTick>();
            _groupAPriceFeed = _testScheduler.CreateHotObservable(
                OnNext(0.1.Seconds(), new PriceDto { Price = 100 }),
                OnNext(1.1.Seconds(), new PriceDto { Price = 101 }),
                OnNext(4.0.Seconds(), new PriceDto { Price = 102 }),
                OnNext(4.5.Seconds(), new PriceDto { Price = 103 }),
                OnNext(5.0.Seconds(), new PriceDto { Price = 104 }),
                OnNext(5.1.Seconds(), new PriceDto { Price = 105 }));

            _groupBPriceFeed = _testScheduler.CreateHotObservable(
                OnNext(0.1.Seconds(), new PriceDto { Price = 200 }),
                OnNext(1.1.Seconds(), new PriceDto { Price = 201 }),
                OnNext(2.0.Seconds(), new PriceDto { Price = 202 }),
                OnNext(2.5.Seconds(), new PriceDto { Price = 203 }),
                OnNext(2.9.Seconds(), new PriceDto { Price = 204 }),
                OnNext(3.1.Seconds(), new PriceDto { Price = 205 }),
                OnNext(3.4.Seconds(), new PriceDto { Price = 206 }),
                OnNext(4.0.Seconds(), new PriceDto { Price = 207 }),
                OnNext(4.2.Seconds(), new PriceDto { Price = 208 }));
        }

        [Test]
        public void Given_a_users_PriceGroupFeed_Then_subscribe_to_correct_priceFeed()
        {
            //In this test, take the single value sequence 'priceGroupFeed' and use the
            //value to get a sequences of prices from the 'GetPriceByGroup(string)' method.
            var priceGroupFeed = Observable.Return("PriceGroupA");

            //TODO: Implementation goes here
            var actual = priceGroupFeed.SelectMany(x => GetPriceByGroup(x)
                            .Select(y => new PriceTick() {Price = y.Price}));

            actual.Subscribe(_observer);
            _testScheduler.Start();

            _observer.Messages.AssertEqual(
                OnNext(0.1.Seconds(), new PriceTick { Price = 100 }),
                OnNext(1.1.Seconds(), new PriceTick { Price = 101, }),
                OnNext(4.0.Seconds(), new PriceTick { Price = 102 }),
                OnNext(4.5.Seconds(), new PriceTick { Price = 103 }),
                OnNext(5.0.Seconds(), new PriceTick { Price = 104, }),
                OnNext(5.1.Seconds(), new PriceTick { Price = 105 }));
        }

        [Test]
        public void Given_a_users_changing_PriceGroupFeed_Then_subscribe_to_correct_priceFeed()
        {
            //In this test, take the multi-value sequence 'priceGroupFeed' and use the
            //value to get a sequences of prices from the 'GetPriceByGroup(string)' method.
            //When a new priceGroup is yielded, then you should no longer be subscribed to
            //prices from the previous priceGroup.

            var priceGroupFeed = _testScheduler.CreateHotObservable(
                OnNext(0.05.Seconds(), "PriceGroupA"),
                OnNext(1.0.Seconds(), "PriceGroupB"),
                OnNext(2.1.Seconds(), "None"),
                OnNext(2.5.Seconds(), "PriceGroupB"),
                OnNext(3.0.Seconds(), "PriceGroupA"));

            //TODO: Implementation goes here
            var actual = priceGroupFeed.Select(x => GetPriceByGroup(x)).Switch()
                                .Select(y => new PriceTick() { Price = y.Price });

            actual.Subscribe(_observer);
            _testScheduler.Start();

            _observer.Messages.AssertEqual(
                OnNext(0.1.Seconds(), new PriceTick { Price = 100 }),
                OnNext(1.1.Seconds(), new PriceTick { Price = 201, }),
                OnNext(2.0.Seconds(), new PriceTick { Price = 202 }),
                OnNext(2.9.Seconds(), new PriceTick { Price = 204 }),
                OnNext(4.0.Seconds(), new PriceTick { Price = 102 }),
                OnNext(4.5.Seconds(), new PriceTick { Price = 103 }),
                OnNext(5.0.Seconds(), new PriceTick { Price = 104, }),
                OnNext(5.1.Seconds(), new PriceTick { Price = 105 }));
        }

        private IObservable<PriceDto> GetPriceByGroup(string groupName)
        {
            if (groupName == "PriceGroupA")
                return _groupAPriceFeed;
            if (groupName == "PriceGroupB")
                return _groupBPriceFeed;

            return Observable.Never<PriceDto>();
        }
    }
}