using System;

namespace CustomerVehicleManagement.Core.Model
{
    public class DateTimeRange
    {
        public DateTimeRange(DateTime from, DateTime thru)
        {
            From = from;
            Thru = thru;
        }
        public DateTime From { get; private set; }
        public DateTime Thru { get; private set; }
    }
}
