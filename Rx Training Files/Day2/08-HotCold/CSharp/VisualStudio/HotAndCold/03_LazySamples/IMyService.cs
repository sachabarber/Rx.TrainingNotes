using System;
using System.Diagnostics;

namespace HotAndCold._03_LazySamples
{
    public interface IMyService
    {
        IDisposable Connect();

        event EventHandler<DataReceivedEventArgs> DataReceived;
    }
}