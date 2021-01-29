using SharedKernel.Utilities;
using System;
using System.Collections.Generic;

namespace SharedKernel.ValueObjects
{
    public class PersonName : ValueObject
    {
        public static readonly string PersonNameEmptyMessage = "First Last and Middle Names cannot be empty";
        public PersonName(string lastName, string firstName, string middleName = null)
        {
            try
            {
                // First name can be null or empty OR last name can be, but not both
                Guard.ForNullOrEmpty(lastName + firstName, "Name");
            }
            catch (Exception)
            {
                throw new ArgumentException(PersonNameEmptyMessage);
            }

            LastName = lastName;
            FirstName = firstName;
            MiddleName = string.IsNullOrWhiteSpace(middleName) ? null : middleName;
        }

        public string LastName { get; }
        public string FirstName { get; }
        public string MiddleName { get; }

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
