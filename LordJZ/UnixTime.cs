using System;

namespace LordJZ
{
    public static class UnixTime
    {
        public static readonly DateTime EpochUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static readonly DateTime Epoch = TimeZone.CurrentTimeZone.ToLocalTime(EpochUtc);

        public static long ToUnixTime(this DateTime dateTime)
        {
            return (long)(dateTime.ToUniversalTime() - Epoch).TotalSeconds;
        }

        public static DateTime FromNumber(long unixTime)
        {
            return Epoch.AddSeconds(unixTime);
        }
    }
}
