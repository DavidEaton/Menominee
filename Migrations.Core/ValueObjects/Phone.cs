using Migrations.Core.Enums;
using SharedKernel.ValueObjects;
using System;
using System.Collections.Generic;

namespace Migrations.Core.ValueObjects
{
    public class Phone : ValueObject
    {
        public static readonly string PhoneEmptyMessage = "Phone number cannot be empty";
        public Phone(string number, PhoneType phoneType)
        {
            if (string.IsNullOrWhiteSpace(number))
            {
                throw new ArgumentException(PhoneEmptyMessage);
            }

            Number = number;
            PhoneType = phoneType;
        }

        public string Number { get; }
        public PhoneType PhoneType { get; }

        public Phone NewNumber(string newNumber)
        {
            return new Phone(newNumber, PhoneType);
        }

        public Phone NewPhoneType(PhoneType newPhoneType)
        {
            return new Phone(Number, newPhoneType);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Number;
            yield return PhoneType;
        }

        #region ORM

        // EF requires an empty constructor
        protected Phone() { }

        #endregion
    }
}
