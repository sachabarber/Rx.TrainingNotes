using System;

namespace HotAndCold
{
    public static class TestSchedulerTimeExtensions
    {
        public static long Seconds(this double seconds)
        {
            return TimeSpan.FromSeconds(seconds).Ticks;
        }

        public static long Seconds(this int seconds)
        {
            return TimeSpan.FromSeconds(seconds).Ticks;
        }
    }
}