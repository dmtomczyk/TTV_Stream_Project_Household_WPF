using System;
using System.Collections.Generic;
using System.Text;

namespace STR001.Core
{
    internal class DataFunctions
    {

        private static readonly long BaseDateTicks = new DateTime(1900, 1, 1).Ticks;

        public static Guid NewGuidComb()
        {
            byte[] guidArray = Guid.NewGuid().ToByteArray();

            DateTime now = DateTime.UtcNow;

            TimeSpan span = new TimeSpan(now.Ticks - BaseDateTicks);
            TimeSpan time = now.TimeOfDay;

            // Convert to byte array
            byte[] spanArray = BitConverter.GetBytes(span.Days);
            // Note: SQL Server is accurate to 1/300th of a millisecond so we divide by 3.333333
            byte[] timeArray = BitConverter.GetBytes((long)(time.Milliseconds / 3.333333));

            // Reverse bytes to match SQL Server ordering
            Array.Reverse(spanArray);
            Array.Reverse(timeArray);

            // Copy bytes into Guid
            Array.Copy(spanArray, spanArray.Length - 2, guidArray, guidArray.Length - 6, 2);
            Array.Copy(timeArray, timeArray.Length - 4, guidArray, guidArray.Length - 4, 4);

            return new Guid(guidArray);
        }

    }
}
