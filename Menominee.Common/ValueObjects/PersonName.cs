using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;

namespace Menominee.Common.ValueObjects
{
    public class PersonName : AppValueObject
    {
        public static readonly int MinimumLength = 1;
        public static readonly int MaximumLength = 255;
        public static readonly string InvalidLengthMessage = $"Must be between {MinimumLength} character(s) {MaximumLength} and in length";
        public static readonly string RequiredMessage = $"Please enter all required items";

        private PersonName(string lastName, string firstName, string middleName = null)
        {
            LastName = lastName;
            FirstName = firstName;
            MiddleName = string.IsNullOrWhiteSpace(middleName) ? null : middleName;
        }

        public string LastName { get; }
        public string FirstName { get; }
        public string MiddleName { get; }

        public static Result<PersonName> Create(string lastName, string firstName, string middleName = null)
        {
            if (string.IsNullOrWhiteSpace(lastName) || string.IsNullOrWhiteSpace(firstName))
                return Result.Failure<PersonName>(RequiredMessage);

            lastName = (lastName ?? string.Empty).Trim();
            firstName = (firstName ?? string.Empty).Trim();
            middleName = (middleName is null || middleName == string.Empty) ? null : middleName.Trim();

            if (lastName.Length < MinimumLength ||
                lastName.Length > MaximumLength ||
                firstName.Length > MaximumLength ||
                firstName.Length > MaximumLength ||
                middleName?.Length > MaximumLength ||
                middleName?.Length > MaximumLength)
                return Result.Failure<PersonName>(InvalidLengthMessage);

            return Result.Success(new PersonName(lastName, firstName, middleName));
        }

        public Result<PersonName> NewLastName(string newLastName)
        {
            newLastName = (newLastName ?? string.Empty).Trim();

            if (newLastName.Length < MinimumLength ||
                newLastName.Length > MaximumLength)
                return Result.Failure<PersonName>(InvalidLengthMessage);

            return Result.Success(new PersonName(newLastName, FirstName, MiddleName));
        }

        public Result<PersonName> NewFirstName(string newFirstName)
        {
            newFirstName = (newFirstName ?? string.Empty).Trim();

            if (newFirstName.Length < MinimumLength ||
                newFirstName.Length > MaximumLength)
                return Result.Failure<PersonName>(InvalidLengthMessage);

            return Result.Success(new PersonName(LastName, newFirstName, MiddleName));
        }

        public Result<PersonName> NewMiddleName(string newMiddleName)
        {
            newMiddleName = (newMiddleName is null || newMiddleName == string.Empty) ? null : newMiddleName.Trim();

            if (newMiddleName?.Length < MinimumLength ||
                newMiddleName?.Length > MaximumLength)
                return Result.Failure<PersonName>(InvalidLengthMessage);

            return Result.Success(new PersonName(LastName, FirstName, newMiddleName));
        }

        public string LastFirstMiddle
        {
            get => string.IsNullOrWhiteSpace(MiddleName) ? $"{LastName}, {FirstName}" : $"{LastName}, {FirstName} {MiddleName}";
        }
        public string LastFirstMiddleInitial
        {
            get => string.IsNullOrWhiteSpace(MiddleName) ? $"{LastName}, {FirstName}" : $"{LastName}, {FirstName} {MiddleName[0]}.";
        }
        public string FirstMiddleLast
        {
            get => string.IsNullOrWhiteSpace(MiddleName) ? $"{FirstName} {LastName}" : $"{FirstName} {MiddleName} {LastName}";
        }

        public override string ToString()
        {
            return LastFirstMiddleInitial;
        }

        protected override IEnumerable<IComparable> GetEqualityComponents()
        {
            yield return LastName;
            yield return FirstName;
            yield return MiddleName;
        }

        #region ORM

        // EF requires an empty constructor
        protected PersonName() { }

        #endregion
    }
}
