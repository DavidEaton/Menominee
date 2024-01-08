using CSharpFunctionalExtensions;
using Menominee.Domain.Enums;
using Menominee.Domain.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using Entity = Menominee.Domain.BaseClasses.Entity;

namespace Menominee.Domain.Entities
{
    public class Phone : Entity, IHasPrimary
    {
        public static readonly string InvalidMessage = "Phone number and/or its format is invalid";
        public static readonly string EmptyMessage = "Phone number cannot be empty";
        public static readonly string PhoneTypeInvalidMessage = $"Please enter a valid Phone Type";

        public string Number { get; private set; }
        public PhoneType PhoneType { get; private set; }
        public bool IsPrimary { get; private set; }

        private Phone(string number, PhoneType phoneType, bool isPrimary)
        {
            Number = number;
            PhoneType = phoneType;
            IsPrimary = isPrimary;
        }

        public static Result<Phone> Create(string number, PhoneType phoneType, bool isPrimary)
        {
            if (!Enum.IsDefined(typeof(PhoneType), phoneType))
                return Result.Failure<Phone>(PhoneTypeInvalidMessage);

            number = (number ?? string.Empty).Trim();

            var phoneAttribute = new PhoneAttribute();

            if (!phoneAttribute.IsValid(number))
                return Result.Failure<Phone>(InvalidMessage);

            return Result.Success(new Phone(number, phoneType, isPrimary));
        }

        public override string ToString()
        {
            var numericNumber = RemoveNonNumericCharacters(Number);

            return numericNumber.Length switch
            {
                7 => Regex.Replace(numericNumber, @"(\d{3})(\d{4})", "$1-$2"),
                10 => Regex.Replace(numericNumber, @"(\d{3})(\d{3})(\d{4})", "($1) $2-$3"),
                _ => numericNumber,
            };
        }

        public Result<string> SetNumber(string number)
        {
            number = (number ?? string.Empty).Trim();

            var phoneAttribute = new PhoneAttribute();

            if (!phoneAttribute.IsValid(number))
                return Result.Failure<string>(InvalidMessage);

            return Result.Success(Number = number);
        }

        public Result<PhoneType> SetPhoneType(PhoneType phoneType)
        {
            if (!Enum.IsDefined(typeof(PhoneType), phoneType))
                return Result.Failure<PhoneType>(InvalidMessage);

            return Result.Success(PhoneType = phoneType);
        }

        public Result<bool> SetIsPrimary(bool isPrimary)
        {
            return Result.Success(IsPrimary = isPrimary);
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
