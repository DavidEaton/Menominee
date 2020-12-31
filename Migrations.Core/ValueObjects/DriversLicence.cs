using SharedKernel.ValueObjects;
using System;
using System.Collections.Generic;

namespace Migrations.Core.ValueObjects
{
    public class DriversLicence : ValueObject
    {
        public static readonly string DriversLicenceInvalidMessage = "Drivers Licence details cannot be empty";
        public string Number { get; }
        public DateTimeRange ValidRange { get; }
        public string State { get; }

        public DriversLicence(string number, string state, DateTimeRange validRange)
        {
            if (string.IsNullOrWhiteSpace(number) | string.IsNullOrWhiteSpace(state) | validRange == null)
            {
                throw new ArgumentException(DriversLicenceInvalidMessage);
            }

            Number = number;
            State = state;
            ValidRange = validRange;
        }

        public DriversLicence NewNumber(string newNumber)
        {
            return new DriversLicence(newNumber, State, ValidRange);
        }
        public DriversLicence NewState(string newState)
        {
            return new DriversLicence(Number, newState, ValidRange);
        }

        public DriversLicence NewValidRange(DateTime start, DateTime end)
        {
            return new DriversLicence(Number, State, new DateTimeRange(start, end));
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Number;
            yield return State;
            yield return ValidRange;
        }
    }
}
