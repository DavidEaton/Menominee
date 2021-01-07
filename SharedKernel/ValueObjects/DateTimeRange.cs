using System;
using System.Collections.Generic;

namespace SharedKernel.ValueObjects
{
    public class DateTimeRange : ValueObject
    {
        public static readonly string DateTimeRangeInvalidMessage = "End date cannot occur before Start date";
        public DateTime Start { get; }
        public DateTime End { get; }

        public DateTimeRange(DateTime start, DateTime end)
        {
            try
            {
                Guard.ForPrecedesDate(start, end, "start");
            }
            catch (Exception)
            {
                throw new ArgumentException(DateTimeRangeInvalidMessage);
            }

            Start = start;
            End = end;
        }

        public DateTimeRange(DateTime start, TimeSpan duration) : this(start, start.Add(duration))
        {
        }

        public int DurationInMinutes()
        {
            return (End - Start).Minutes;
        }

        public DateTimeRange NewEnd(DateTime newEnd)
        {
            return new DateTimeRange(Start, newEnd);
        }
        public DateTimeRange NewDuration(TimeSpan newDuration)
        {
            return new DateTimeRange(Start, newDuration);
        }
        public DateTimeRange NewStart(DateTime newStart)
        {
            return new DateTimeRange(newStart, End);
        }

        public static DateTimeRange CreateOneDayRange(DateTime start)
        {
            return new DateTimeRange(start, start.AddDays(1));
        }

        public static DateTimeRange CreateOneWeekRange(DateTime start)
        {
            return new DateTimeRange(start, start.AddDays(7));
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return End;
            yield return Start;
        }

        #region ORM

        // EF requires an empty constructor
        protected DateTimeRange() { }

        #endregion
    }
}
