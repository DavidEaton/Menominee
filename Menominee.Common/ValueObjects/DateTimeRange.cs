using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;

namespace Menominee.Common.ValueObjects
{
    public class DateTimeRange : AppValueObject
    {
        public static readonly string RequiredMessage = $"Please include all required items.";
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
            return start >= end
                ? Result.Failure<DateTimeRange>(EndBeforeStartMessage)
                : Result.Success(new DateTimeRange(start, end));
        }

        public DateTimeRange(DateTime start, TimeSpan duration) : this(start, start.Add(duration))
        {
        }

        public Result<int> DurationInMinutes()
        {
            return Result.Success((End - Start).Minutes);
        }

        public Result<DateTimeRange> NewEnd(DateTime newEnd)
        {
            return Start >= newEnd
                ? Result.Failure<DateTimeRange>(RequiredMessage)
                : Result.Success(new DateTimeRange(Start, newEnd));
        }

        public Result<DateTimeRange> NewDuration(TimeSpan newDuration)
        {
            return Result.Success(new DateTimeRange(Start, newDuration));
        }

        public Result<DateTimeRange> NewStart(DateTime newStart)
        {
            return newStart >= End
                ? Result.Failure<DateTimeRange>(RequiredMessage)
                : Result.Success(new DateTimeRange(newStart, End));
        }

        public static Result<DateTimeRange> CreateOneDayRange(DateTime start)
        {
            return Result.Success(new DateTimeRange(start, start.AddDays(1)));
        }

        public static Result<DateTimeRange> CreateOneWeekRange(DateTime start)
        {
            return Result.Success(new DateTimeRange(start, start.AddDays(7)));
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
