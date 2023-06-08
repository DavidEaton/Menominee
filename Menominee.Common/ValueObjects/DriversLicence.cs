using CSharpFunctionalExtensions;
using Menominee.Common.Enums;
using System;
using System.Collections.Generic;

namespace Menominee.Common.ValueObjects
{
    public class DriversLicense : AppValueObject
    {
        public static readonly int MinimumLength = 3;
        public static readonly int MaximumLength = 255;
        public static readonly string UnderMinimumLengthMessage = $"Drivers License cannot be less than {MinimumLength} character(s) in length";
        public static readonly string OverMaximumLengthMessage = $"Drivers License cannot be over {MaximumLength} characters in length";
        public static readonly string DateRangeInvalidMessage = $"Drivers License must have valid dates";
        public static readonly string RequiredMessage = $"Drivers License number is required.";
        public static readonly string StateInvalidMessage = $"Please enter a valid State";

        public string Number { get; private set; }
        public DateTimeRange ValidDateRange { get; private set; }
        public State State { get; private set; }

        private DriversLicense(string number, State state, DateTimeRange validDateRange)
        {
            Number = number;
            State = state;
            ValidDateRange = validDateRange;
        }

        public static Result<DriversLicense> Create(string number, State state, DateTimeRange validRange)
        {
            if (string.IsNullOrWhiteSpace(number))
                return Result.Failure<DriversLicense>(RequiredMessage);

            if (!Enum.IsDefined(typeof(State), state))
                return Result.Failure<DriversLicense>(StateInvalidMessage);

            number = (number ?? string.Empty).Trim();

            if (validRange == null)
                return Result.Failure<DriversLicense>(DateRangeInvalidMessage);

            if (number.Length < MinimumLength)
                return Result.Failure<DriversLicense>(UnderMinimumLengthMessage);

            if (number.Length > MaximumLength)
                return Result.Failure<DriversLicense>(OverMaximumLengthMessage);

            return Result.Success(new DriversLicense(number, state, validRange));
        }

        public Result<DriversLicense> NewNumber(string newNumber)
        {
            return Result.Success(new DriversLicense(newNumber, State, ValidDateRange));
        }
        public Result<DriversLicense> NewState(State newState)
        {
            return Result.Success(new DriversLicense(Number, newState, ValidDateRange));
        }

        public Result<DriversLicense> NewValidRange(DateTime start, DateTime end)
        {
            return Result.Success(Create(Number, State, DateTimeRange.Create(start, end).Value).Value);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Number;
            yield return State;
            yield return ValidDateRange;
        }

        #region ORM

        // EF requires an empty constructor
        protected DriversLicense() { }

        #endregion
    }
}
