using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;

namespace Menominee.Common.ValueObjects
{
    public class DateTimeRange : AppValueObject
    {
        public static readonly string EndBeforeStartMessage = "End date cannot occur before Start date";
        public DateTime Start { get; private set; }
        public DateTime End { get; private set; }

        private DateTimeRange(DateTime start, DateTime end)
        {
            Start = start;
            End = end;
        }

        public static Result<DateTimeRange> Create(DateTime start, DateTime end)
        {
            if (start >= end)
                return Result.Failure<DateTimeRange>(EndBeforeStartMessage);

            return Result.Success(new DateTimeRange(start, end));
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
            if (Start >= newEnd)
                throw new ArgumentOutOfRangeException(EndBeforeStartMessage);

            return new DateTimeRange(Start, newEnd);
        }

        public DateTimeRange NewDuration(TimeSpan newDuration)
        {
            return new DateTimeRange(Start, newDuration);
        }

        public DateTimeRange NewStart(DateTime newStart)
        {
            if (newStart >= End)
                throw new ArgumentOutOfRangeException(EndBeforeStartMessage);

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
