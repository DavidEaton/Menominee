using CSharpFunctionalExtensions;
using Menominee.Common.Enums;
using Menominee.Common.Utilities;
using System;
using System.Collections.Generic;

namespace Menominee.Common.ValueObjects
{
    public class DriversLicense : AppValueObject
    {
        public static readonly int DriversLicenseNumberMinimumLength = 3;
        public static readonly int DriversLicenseNumberMaximumLength = 255;
        public static readonly string DriversLicenseNumberUnderMinimumLengthMessage = $"Drivers License cannot be less than {DriversLicenseNumberMinimumLength} character(s) in length";
        public static readonly string DriversLicenseNumberOverMaximumLengthMessage = $"Drivers License cannot be over {DriversLicenseNumberMaximumLength} characters in length";
        public static readonly string DriversLicenseDateRangeInvalidMessage = $"Drivers License must have valid dates";

        public string Number { get; private set; }
        public DateTimeRange ValidRange { get; private set; }
        public State State { get; private set; }

        private DriversLicense(string number, State state, DateTimeRange validRange)
        {
            Number = number;
            State = state;
            ValidRange = validRange;
        }

        public static Result<DriversLicense> Create(string number, State state, DateTimeRange validRange)
        {
            number = (number ?? string.Empty).Trim();

            if (validRange == null)
                return Result.Failure<DriversLicense>(DriversLicenseDateRangeInvalidMessage);

            if (number.Length < DriversLicenseNumberMinimumLength)
                return Result.Failure<DriversLicense>(DriversLicenseNumberUnderMinimumLengthMessage);

            if (number.Length > DriversLicenseNumberMaximumLength)
                return Result.Failure<DriversLicense>(DriversLicenseNumberOverMaximumLengthMessage);

            return Result.Success(new DriversLicense(number, state, validRange));
        }

        public DriversLicense NewNumber(string newNumber)
        {
            return new DriversLicense(newNumber, State, ValidRange);
        }
        public DriversLicense NewState(State newState)
        {
            return new DriversLicense(Number, newState, ValidRange);
        }

        public DriversLicense NewValidRange(DateTime start, DateTime end)
        {
            return Create(Number, State, DateTimeRange.Create(start, end).Value).Value;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Number;
            yield return State;
            yield return ValidRange;
        }

        #region ORM

        // EF requires an empty constructor
        protected DriversLicense() { }

        #endregion
    }
}
