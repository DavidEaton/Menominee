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

        public IList<Phone> Phones { get; private set; } = new List<Phone>();
        public IList<Email> Emails { get; private set; } = new List<Email>();
        public Address Address { get; private set; }

        public Contactable(Address address, IList<Phone> phones, IList<Email> emails)
        {
            if (address is not null)
                SetAddress(address);

            if (phones is not null)
                SetPhones(phones);

            if (emails is not null)
                SetEmails(emails);
        }

        public void RemovePhone(Phone phone)
        {
            if (phone is null)
                throw new ArgumentNullException(RequiredMessage);

            Phones.Remove(phone);
        }

        public void SetPhones(IList<Phone> phones)
        {
            // Client may send an empty or null collection, signifying
            // NO CHANGE TO COLLECTION
            if (phones is not null && phones?.Count > 0)
                Phones = phones;
        }

        public void AddEmail(Email email)
        {
            if (ContactableHasEmail(email))
                throw new Exception("Duplicate email.");

            if (ContactableHasPrimaryEmail() && email.IsPrimary)
                throw new Exception("Primary email has already been entered.");

            Emails.Add(email);
        }

        public void RemoveEmail(Email email)
        {
            Emails.Remove(email);
        }

        public void SetEmails(IList<Email> emails)
        {
            // Client may send an empty or null collection, signifying
            // NO CHANGE TO COLLECTION
            if (emails is not null && emails?.Count > 0)
                Emails = emails;
        }

        public void SetAddress(Address address)
        {
            // Address is guaranteed to be valid; it was validated on creation.
            // Address is optional, so excluding it shouldn't throw an exception:
            // BTW, if user removes Address, it will be null here so use it.
            Address = address;
        }

        public void AddPhone(Phone phone)
        {
            /* Use a guard to throw exception in this exceptional case: we don't
             *  expect a null to ever reach here, so there must be a bug. -DE */
            if (phone is null)
                throw new ArgumentNullException(RequiredMessage);

            if (ContactableHasPhone(phone))
                throw new Exception("Duplicate phone");

            if (ContactableHasPrimaryPhone() && phone.IsPrimary)
                throw new Exception("Primary phone has already been entered.");

            Phones.Add(phone);
        }

        // VK: no need to make these methods public, they are just for the Contactable class
        // you can also keep them non-static, so that you don't need to pass in the existing collections
        private bool ContactableHasPhone(Phone phone)
        {
            return Phones.Any(x => x.Number == phone.Number);
        }

        private bool ContactableHasPrimaryPhone()
        {
            return Phones.Any(x => x.IsPrimary);
        }

        private bool ContactableHasPrimaryEmail()
        {
            return Emails.Any(x => x.IsPrimary);
        }

        private bool ContactableHasEmail(Email email)
        {
            return Emails.Any(x => x.Address == email.Address);
        }

        // EF requires parameterless constructor
        protected Contactable() { }
    }
}
