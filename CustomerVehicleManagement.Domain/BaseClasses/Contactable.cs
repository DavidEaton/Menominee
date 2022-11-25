using CSharpFunctionalExtensions;
using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Domain.Interfaces;
using Menominee.Common.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using Entity = Menominee.Common.Entity;

namespace CustomerVehicleManagement.Domain.BaseClasses
{
    public abstract class Contactable : Entity, IContactLists
    {
        public static readonly string RequiredMessage = $"Please enter all required items.";
        public static readonly string NonuniqueMessage = $"Item has already been entered; each item must be unique.";
        public static readonly string PrimaryExistsMessage = $"Primary has already been entered.";

        public IList<Phone> Phones { get; private set; } = new List<Phone>();
        public IList<Email> Emails { get; private set; } = new List<Email>();
        public Address Address { get; private set; }

        internal Contactable(Address address, IList<Phone> phones, IList<Email> emails)
        {
            if (address is not null)
                SetAddress(address);

            if (phones is not null)
                foreach (var phone in phones)
                    Phones.Add(phone);

            if (emails is not null)
                foreach (var email in emails)
                    Emails.Add(email);
        }

        public Result<Email> AddEmail(Email email)
        {
            if (email is null)
                return Result.Failure<Email>(RequiredMessage);

            if (ContactableHasEmail(email))
                return Result.Failure<Email>(NonuniqueMessage);

            if (ContactableHasPrimaryEmail() && email.IsPrimary)
                return Result.Failure<Email>(PrimaryExistsMessage);

            Emails.Add(email);
            return Result.Success(email);
        }

        public Result RemoveEmail(Email email)
        {
            if (email is null)
                return Result.Failure<Email>(RequiredMessage);

            return Result.Success(Emails.Remove(email));
        }

        public Result<Phone> AddPhone(Phone phone)
        {
            if (phone is null)
                return Result.Failure<Phone>(RequiredMessage);

            if (ContactableHasPhone(phone))
                return Result.Failure<Phone>(NonuniqueMessage);

            if (ContactableHasPrimaryPhone() && phone.IsPrimary)
                throw new Exception(PrimaryExistsMessage);

            Phones.Add(phone);
            return Result.Success(phone);
        }

        public Result RemovePhone(Phone phone)
        {
            if (phone is null)
                return Result.Failure<Phone>(RequiredMessage);

            return Result.Success(Phones.Remove(phone));
        }

        public Result SetAddress(Address address)
        {
            // Address is guaranteed to be valid; it was validated on creation.
            // Address is optional, so excluding it shouldn't throw an exception:
            return Result.Success(Address = address);
        }

        public void ClearAddress()
        {
            Address = null;
        }

        private bool ContactableHasPhone(Phone phone)
        {
            return Phones.Any(existingPhone => existingPhone.Number == phone.Number);
        }

        private bool ContactableHasPrimaryPhone()
        {
            return Phones.Any(existingPhone => existingPhone.IsPrimary);
        }

        private bool ContactableHasEmail(Email email)
        {
            return Emails.Any(existingEmail => existingEmail.Address == email.Address);
        }

        private bool ContactableHasPrimaryEmail()
        {
            return Emails.Any(email => email.IsPrimary);
        }

        #region ORM

        // EF requires parameterless constructor
        protected Contactable() { }

        #endregion
    }
}
