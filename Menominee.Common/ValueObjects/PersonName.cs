using Menominee.Common.Utilities;
using System;
using System.Collections.Generic;

namespace Menominee.Common.ValueObjects
{
    public class PersonName : ValueObject
    {
        public static readonly int LastNameMinimumLength = 1;
        public static readonly int LastNameMaximumLength = 255;
        public static readonly string LastNameUnderMinimumLengthMessage = $"Last name cannot be less than {LastNameMinimumLength} character(s) in length";
        public static readonly string LastNameOverMaximumLengthMessage = $"Last name cannot be over {LastNameMaximumLength} characters in length";

        public static readonly int FirstNameMinimumLength = 1;
        public static readonly int FirstNameMaximumLength = 255;
        public static readonly string FirstNameUnderMinimumLengthMessage = $"First name cannot be less than {FirstNameMinimumLength} character(s) in length";
        public static readonly string FirstNameOverMaximumLengthMessage = $"First name cannot be over {FirstNameMaximumLength} characters in length";

        public static readonly int MiddleNameMinimumLength = 1;
        public static readonly int MiddleNameMaximumLength = 255;
        public static readonly string MiddleNameUnderMinimumLengthMessage = $"Middle name cannot be less than {MiddleNameMinimumLength} character(s) in length";
        public static readonly string MiddleNameOverMaximumLengthMessage = $"Middle name cannot be over {MiddleNameMaximumLength} characters in length";
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
            lastName = (lastName ?? string.Empty).Trim();
            firstName = (firstName ?? string.Empty).Trim();
            middleName = (middleName ?? string.Empty).Trim();

            if (lastName.Length < LastNameMinimumLength)
                return Result.Fail<PersonName>(LastNameUnderMinimumLengthMessage);

            if (lastName.Length > LastNameMaximumLength)
                return Result.Fail<PersonName>(LastNameOverMaximumLengthMessage);

            if (firstName.Length < FirstNameMinimumLength)
                return Result.Fail<PersonName>(FirstNameUnderMinimumLengthMessage);

            if (firstName.Length > FirstNameMaximumLength)
                return Result.Fail<PersonName>(FirstNameOverMaximumLengthMessage);

            if (middleName.Length > MiddleNameMaximumLength)
                return Result.Fail<PersonName>(MiddleNameOverMaximumLengthMessage);

            return Result.Ok(new PersonName(lastName, firstName, middleName));
        }

        public PersonName NewLastName(string newLastName)
        {
            newLastName = (newLastName ?? string.Empty).Trim();
            Guard.ForNullOrEmpty(newLastName, "newLastName");
            return new PersonName(newLastName, FirstName, MiddleName);
        }

        public PersonName NewFirstName(string newFirstName)
        {
            newFirstName = (newFirstName ?? string.Empty).Trim();
            Guard.ForNullOrEmpty(newFirstName, "newFirstName");
            return new PersonName(LastName, newFirstName, MiddleName);
        }

        public PersonName NewMiddleName(string newMiddleName)
        {
            newMiddleName = (newMiddleName ?? string.Empty).Trim();
            Guard.ForNullOrEmpty(newMiddleName, "newMiddleName");
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
