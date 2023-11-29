using CSharpFunctionalExtensions;
using Menominee.Common.Enums;
using Menominee.Common.Extensions;
using Menominee.Common.ValueObjects;
using Menominee.Domain.BaseClasses;
using Menominee.Domain.Interfaces;
using System;
using System.Collections.Generic;

namespace Menominee.Domain.Entities
{
    public class Person : Contactable, ICustomerEntity
    {
        // Targeting tests at the abstract base class binds them to the code’s implementation details.
        // Always test all concrete classes; don’t test abstract classes directly (like Contactable)

        public PersonName Name { get; private set; }
        public Gender Gender { get; private set; }
        public DateTime? Birthday { get; private set; }
        public DriversLicense DriversLicense { get; private set; }
        public override string ToString() => Name.ToString();
        public string DisplayName => Name.ToString();
        public EntityType EntityType => EntityType.Person;

        internal Person(
            PersonName name,
            Gender gender,
            string notes,
            Address address,
            IReadOnlyList<Email> emails,
            IReadOnlyList<Phone> phones,
            DateTime? birthday = null,
            DriversLicense driversLicense = null)
            : base(notes, address, phones, emails)
        {
            Name = name;
            Gender = gender;
            Birthday = birthday;
            DriversLicense = driversLicense;
        }

        public static Result<Person> Create(
            PersonName name,
            Gender gender,
            string notes,
            DateTime? birthday = null,
            IReadOnlyList<Email> emails = null,
            IReadOnlyList<Phone> phones = null,
            Address address = null,
            DriversLicense driversLicense = null)
        {
            if (name is null)
                return Result.Failure<Person>(RequiredMessage);

            notes = (notes ?? string.Empty).Trim().Truncate(NoteMaximumLength);

            if (!string.IsNullOrWhiteSpace(notes) && notes.Length > NoteMaximumLength)
                return Result.Failure<Person>(NoteMaximumLengthMessage);

            if (!Enum.IsDefined(typeof(Gender), gender))
                return Result.Failure<Person>(InvalidValueMessage);

            if (birthday.HasValue)
                if (!IsValidAgeOn(birthday))
                    return Result.Failure<Person>(InvalidValueMessage);

            return Result.Success(new Person(name, gender, notes, address, emails, phones, birthday, driversLicense));
        }

        public Result<PersonName> SetName(PersonName name)
        {
            if (name is null)
                return Result.Failure<PersonName>(RequiredMessage);

            return Result.Success(Name = name);
        }

        public Result<Gender> SetGender(Gender gender)
        {
            if (!Enum.IsDefined(typeof(Gender), gender))
                return Result.Failure<Gender>(RequiredMessage);

            return Result.Success(Gender = gender);
        }

        public Result<DateTime?> SetBirthday(DateTime? birthday)
        {
            if (!IsValidAgeOn(birthday))
                return Result.Failure<DateTime?>(InvalidValueMessage);

            return Result.Success(Birthday = birthday);
        }

        public Result<DriversLicense> SetDriversLicense(DriversLicense driversLicense)
        {
            if (driversLicense is null)
                return Result.Failure<DriversLicense>(InvalidValueMessage);

            return Result.Success(DriversLicense = driversLicense);
        }

        public static bool IsValidAgeOn(DateTime? birthDate)
        {
            if (birthDate is null)
                return false;

            if (!birthDate.HasValue)
                return false;

            if (birthDate >= DateTime.Today)
                return false;

            int thisYear = DateTime.Today.Year;
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
