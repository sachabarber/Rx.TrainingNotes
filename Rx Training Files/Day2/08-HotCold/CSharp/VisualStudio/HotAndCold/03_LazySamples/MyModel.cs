using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace HotAndCold._03_LazySamples
{
    class MyModel
    {
        private readonly IMyService _service;

        public MyModel(IMyService service)
        {
            _service = service;
        }

        //TODO: Correct this implementation to be idiomatic Rx (lazy, not leaky etc..)
        public IObservable<string> GetData()
        {
            //var connection = _service.Connect();
            //return Observable.FromEventPattern<DataReceivedEventArgs>(
            //    h => _service.DataReceived += h,
            //    h => _service.DataReceived -= h)
            //    .Select(e => e.EventArgs.Data);


            return Observable.Create<string>(
               (observer) =>
               {
                   var serviceSub = Observable.FromEventPattern<DataReceivedEventArgs>(
                       h => _service.DataReceived += h,
                       h => _service.DataReceived -= h)
                       .Select(e => e.EventArgs.Data).Subscribe(observer);

                   //make sure this happens after we subscribe as if we don't we could miss the raising of the event
                   //to begin with
                   var connection = _service.Connect();

                   return new CompositeDisposable(serviceSub, connection);
               });

        }
    }
}
