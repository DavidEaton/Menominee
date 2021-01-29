using SharedKernel.Utilities;
using System;
using System.Collections.Generic;

namespace SharedKernel.ValueObjects
{
    public class DriversLicense : ValueObject
    {
        public static readonly string DriversLicenseInvalidMessage = "Drivers License details cannot be empty";
        public string Number { get; }
        public DateTimeRange ValidRange { get; }
        public string State { get; }
        
        public DriversLicense(string number, string state, DateTimeRange validRange)
        {
            try
            {
                Guard.ForNullOrEmpty(number, "number");
                Guard.ForNullOrEmpty(state, "state");
                Guard.ForNull(validRange, "validRange");
            }
            catch (Exception)
            {
                throw new ArgumentException(DriversLicenseInvalidMessage);
            }

            Number = number;
            State = state;
            ValidRange = validRange;
        }

        public DriversLicense NewNumber(string newNumber)
        {
            return new DriversLicense(newNumber, State, ValidRange);
        }
        public DriversLicense NewState(string newState)
        {
            return new DriversLicense(Number, newState, ValidRange);
        }

        public DriversLicense NewValidRange(DateTime start, DateTime end)
        {
            return new DriversLicense(Number, State, new DateTimeRange(start, end));
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
