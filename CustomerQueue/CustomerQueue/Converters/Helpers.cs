using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerQueue.Converters
{
    public static class Helpers
    {
        public static int MinuteDifference(DateTime newerTime, DateTime olderTime)
        {
            return (int)TimeSpan.FromTicks(newerTime.Ticks - olderTime.Ticks).TotalMinutes;
        }
    }
}
