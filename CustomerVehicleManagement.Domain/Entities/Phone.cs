using CSharpFunctionalExtensions;
using CustomerVehicleManagement.Domain.Interfaces;
using Menominee.Common.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using Entity = Menominee.Common.Entity;

namespace CustomerVehicleManagement.Domain.Entities
{
    public class Phone : Entity, IHasPrimary
    {
        public static readonly string InvalidMessage = "Email address and/or its format is invalid";
        public static readonly string EmptyMessage = "Phone number cannot be empty";
        public static readonly string PhoneTypeInvalidMessage = $"Please enter a valid Phone Type";

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

            if (!Enum.IsDefined(typeof(PhoneType), phoneType))
                return Result.Failure<Phone>(PhoneTypeInvalidMessage);

            number = (number ?? string.Empty).Trim();

            var phoneAttribute = new PhoneAttribute();

            if (!phoneAttribute.IsValid(number))
                return Result.Failure<Phone>(InvalidMessage);

            return Result.Success(new Phone(number, phoneType, isPrimary));
        }

        public string Number { get; private set; }
        public PhoneType PhoneType { get; private set; }
        public bool IsPrimary { get; private set; }

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

        public void SetNumber(string number)
        {
            if (number is null)
                throw new ArgumentOutOfRangeException(nameof(number), "payment");

            number = number.Trim();

            var phoneAttribute = new PhoneAttribute();

            if (!phoneAttribute.IsValid(number))
                throw new ArgumentOutOfRangeException(nameof(number), InvalidMessage);

            Number = number;
        }

        public void SetPhoneType(PhoneType phoneType)
        {
            if (!Enum.IsDefined(typeof(PhoneType), phoneType))
                throw new ArgumentOutOfRangeException(typeof(PhoneType).ToString(), InvalidMessage); ;

            PhoneType = phoneType;
        }

        public void SetIsPrimary(bool isPrimary)
        {
            IsPrimary = isPrimary;
        }

        private static string RemoveNonNumericCharacters(string input)
        {
            return new string(input.Where(c => char.IsDigit(c)).ToArray());
        }

        #region ORM

        // EF requires a parameterless constructor
        protected Phone() { }

        #endregion

    }

}
