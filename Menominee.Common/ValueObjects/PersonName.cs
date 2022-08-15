using CSharpFunctionalExtensions;
using Menominee.Common.Utilities;
using System.Collections.Generic;

namespace Menominee.Common.ValueObjects
{
    public class PersonName : AppValueObject
    {
        public static readonly int LastNameMinimumLength = 2;
        public static readonly int LastNameMaximumLength = 255;
        public static readonly string LastNameMinimumLengthMessage = $"Last name cannot be less than {LastNameMinimumLength} character(s) in length";
        public static readonly string LastNameMaximumLengthMessage = $"Last name cannot be over {LastNameMaximumLength} characters in length";
        public static readonly string LastNameRequiredMessage = $"Last name is required";

        public static readonly int FirstNameMinimumLength = 1;
        public static readonly int FirstNameMaximumLength = 255;
        public static readonly string FirstNameMinimumLengthMessage = $"First name cannot be less than {FirstNameMinimumLength} character(s) in length";
        public static readonly string FirstNameMaximumLengthMessage = $"First name cannot be over {FirstNameMaximumLength} characters in length";
        public static readonly string FirstNameRequiredMessage = $"First name is required";

        public static readonly int MiddleNameMinimumLength = 1;
        public static readonly int MiddleNameMaximumLength = 255;
        public static readonly string MiddleNameMinimumLengthMessage = $"Middle name cannot be less than {MiddleNameMinimumLength} character(s) in length";
        public static readonly string MiddleNameMaximumLengthMessage = $"Middle name cannot be over {MiddleNameMaximumLength} characters in length";
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
            if (string.IsNullOrWhiteSpace(lastName))
                return Result.Failure<PersonName>(LastNameRequiredMessage);

            if (string.IsNullOrWhiteSpace(firstName))
                return Result.Failure<PersonName>(FirstNameRequiredMessage);

            lastName = (lastName ?? string.Empty).Trim();
            firstName = (firstName ?? string.Empty).Trim();
            middleName = (middleName is null || middleName == string.Empty) ? null : middleName.Trim();

            if (lastName.Length < LastNameMinimumLength)
                return Result.Failure<PersonName>(LastNameMinimumLengthMessage);

            if (lastName.Length > LastNameMaximumLength)
                return Result.Failure<PersonName>(LastNameMaximumLengthMessage);

            if (firstName.Length < FirstNameMinimumLength)
                return Result.Failure<PersonName>(FirstNameMinimumLengthMessage);

            if (firstName.Length > FirstNameMaximumLength)
                return Result.Failure<PersonName>(FirstNameMaximumLengthMessage);

            if (middleName?.Length < MiddleNameMinimumLength)
                return Result.Failure<PersonName>(MiddleNameMinimumLengthMessage);

            if (middleName?.Length > MiddleNameMaximumLength)
                return Result.Failure<PersonName>(MiddleNameMaximumLengthMessage);

            return Result.Success(new PersonName(lastName, firstName, middleName));
        }

        public PersonName NewLastName(string newLastName)
        {
            Guard.ForNullOrEmpty(newLastName, "newLastName");
            newLastName = (newLastName ?? string.Empty).Trim();
            return new PersonName(newLastName, FirstName, MiddleName);
        }

        public PersonName NewFirstName(string newFirstName)
        {
            Guard.ForNullOrEmpty(newFirstName, "newFirstName");
            newFirstName = (newFirstName ?? string.Empty).Trim();
            return new PersonName(LastName, newFirstName, MiddleName);
        }

        public PersonName NewMiddleName(string newMiddleName)
        {
            Guard.ForNullOrEmpty(newMiddleName, "newMiddleName");
            newMiddleName = (newMiddleName ?? string.Empty).Trim();
            return new PersonName(LastName, FirstName, newMiddleName);
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

        protected override IEnumerable<object> GetEqualityComponents()
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
