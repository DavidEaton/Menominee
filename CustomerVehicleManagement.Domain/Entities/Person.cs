using CSharpFunctionalExtensions;
using CustomerVehicleManagement.Domain.BaseClasses;
using Menominee.Common.Enums;
using Menominee.Common.ValueObjects;
using System;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Domain.Entities
{
    public class Person : Contactable
    {
        public static readonly string RequiredMessage = $"Please include all required items.";
        public static readonly int NoteMaximumLength = 10000;
        public static readonly string NoteMaximumLengthMessage = $"Note cannot be over {NoteMaximumLength} characters in length.";

        public PersonName Name { get; private set; }
        public Gender Gender { get; private set; }
        public DateTime? Birthday { get; private set; }
        public DriversLicense DriversLicense { get; private set; }
        private Person(PersonName name,
                      Gender gender,
                      Address address,
                      IList<Email> emails,
                      IList<Phone> phones,
                      DateTime? birthday = null,
                      DriversLicense driversLicense = null)
             : base(address, phones, emails)
        {
            Name = name;
            Gender = gender;

            if (birthday.HasValue)
                Birthday = birthday;

            if (driversLicense != null)
                DriversLicense = driversLicense;
        }

        public static Result<Person> Create(
            PersonName name,
            Gender gender,
            DateTime? birthday = null,
            IList<Email> emails = null,
            IList<Phone> phones = null,
            Address address = null,
            DriversLicense driversLicense = null)
        {
            if (name is null)
                return Result.Failure<Person>(RequiredMessage);

            if (!Enum.IsDefined(typeof(Gender), gender))
                return Result.Failure<Person>("Invalid Gender");

            if (birthday.HasValue)
                if (!IsValidAge(birthday))
                    return Result.Failure<Person>("Invalid Birthday");

            return Result.Success(new Person(name, gender, address, emails, phones, birthday, driversLicense));
        }

        public void SetName(PersonName name)
        {
            if (name is null)
                throw new ArgumentOutOfRangeException(nameof(name), RequiredMessage);

            Name = name;
        }

        public void SetGender(Gender gender)
        {
            Gender = gender;
        }

        public void SetBirthday(DateTime? birthday)
        {
                Birthday = birthday;
        }

        public void SetDriversLicense(DriversLicense driversLicense)
        {
            if (driversLicense != null)
                DriversLicense = driversLicense;
        }
        protected static bool IsValidAge(DateTime? birthDate)
        {
            if (birthDate is null)
                return false;

            if (!birthDate.HasValue)
                return false;

            if (birthDate >= DateTime.Today)
                return false;

            int thisYear = DateTime.Now.Year;
            int birthYear = birthDate.Value.Year;

            if (birthYear <= thisYear && birthYear > (thisYear - 120))
                return true;

            return false;
        }
        #region ORM

        // EF requires a parameterless constructor
        protected Person() { }

        #endregion

    }
}
