using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace TestingRx
{



    public static class RxTestingExtensions
    {
        public static long Minutes(this int minutes)
        {
            return TimeSpan.FromMinutes(minutes).Ticks;
        }
        public static long Minutes(this double minutes)
        {
            return TimeSpan.FromMinutes(minutes).Ticks;
        }
        public static long Seconds(this int seconds)
        {
            return TimeSpan.FromSeconds(seconds).Ticks;
        }
        public static long Seconds(this double seconds)
        {
            return TimeSpan.FromSeconds(seconds).Ticks;
        }
        public static long Milliseconds(this int milliseconds)
        {
            return TimeSpan.FromMilliseconds(milliseconds).Ticks;
        }
        public static long Milliseconds(this double milliseconds)
        {
            return TimeSpan.FromMilliseconds(milliseconds).Ticks;
        }
    }
}