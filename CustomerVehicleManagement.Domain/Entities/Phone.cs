using CSharpFunctionalExtensions;
using Menominee.Common.Enums;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;

namespace CustomerVehicleManagement.Domain.Entities
{
    public class Phone : Menominee.Common.Entity
    {
        public static readonly string InvalidMessage = "Email address and/or its format is invalid";
        public static readonly string EmptyMessage = "Phone number cannot be empty";

        private Phone(string number, PhoneType phoneType, bool isPrimary)
        {
            Number = number;
            PhoneType = phoneType;
            IsPrimary = isPrimary;
        }

        public static Result<Phone> Create(string number, PhoneType phoneType, bool isPrimary)
        {
            if (string.IsNullOrWhiteSpace(number))
                return Result.Failure<Phone>(EmptyMessage);

            number = (number ?? "").Trim();

            var phoneAttribute = new PhoneAttribute();

            if (!phoneAttribute.IsValid(number))
                return Result.Failure<Phone>(InvalidMessage);

            return Result.Success(new Phone(number, phoneType, isPrimary));
        }

        public string Number { get; private set; }
        public PhoneType PhoneType { get; private set; }
        public bool IsPrimary { get; private set; }

        public Phone NewNumber(string newNumber)
        {
            return new Phone(newNumber, PhoneType, IsPrimary);
        }

        public Phone NewPhoneType(PhoneType newPhoneType)
        {
            return new Phone(Number, newPhoneType, IsPrimary);
        }
        public Phone NewPrimary(bool primary)
        {
            return new Phone(Number, PhoneType, primary);
        }

        public override string ToString()
        {
            Number = RemoveNonNumericCharacters(Number);

            return Number.Length switch
            {
                7 => Regex.Replace(Number, @"(\d{3})(\d{4})", "$1-$2"),
                10 => Regex.Replace(Number, @"(\d{3})(\d{3})(\d{4})", "($1) $2-$3"),
                _ => Number,
            };
        }

        /// <summary>
        /// EqualsByProperty compares each member property and returns true of they are all equal between phone1 and phone2.
        /// </summary>
        /// <param name="phone1"></param>
        /// <param name="phone2"></param>
        /// <returns>True if all properties are equal comparing phone1 and phone2; False if one or more properties are not equal.</returns>
        public static bool EqualsByProperty(Phone phone1, Phone phone2)
        {
            return phone1.Number == phone2.Number
                && phone1.PhoneType == phone2.PhoneType
                && phone1.IsPrimary == phone2.IsPrimary;
        }

        private static string RemoveNonNumericCharacters(string input)
        {
            return new string(input.Where(c => char.IsDigit(c)).ToArray());
        }

        #region ORM

        // EF requires an empty constructor
        protected Phone() { }

        #endregion

    }

}
