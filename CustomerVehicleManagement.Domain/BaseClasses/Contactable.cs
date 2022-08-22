using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Domain.Interfaces;
using Menominee.Common;
using Menominee.Common.Utilities;
using Menominee.Common.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CustomerVehicleManagement.Domain.BaseClasses
{
    public abstract class Contactable : Entity, IContactLists
    {
        public virtual IList<Phone> Phones { get; private set; } = new List<Phone>();
        public virtual IList<Email> Emails { get; private set; } = new List<Email>();
        public Address Address { get; private set; }

        public Contactable(Address address, IList<Phone> phones, IList<Email> emails)
        {
            if (address != null)
                SetAddress(address);

            if (phones != null)
                SetPhones(phones);

            if (emails != null)
                SetEmails(emails);
        }

        public void AddPhone(Phone phone)
        {
            /* Null check silently swallows exception, hiding potential bugs. Use a guard to throw exception in
             * this exceptional case: we don't expect a null to ever reach here, so there must be a bug. -DE */
            Guard.ForNull(phone, "phone");

            if (ContactableHasPhone(phone))
                throw new Exception("Duplicate phone");

            if (ContactableHasPrimaryPhone() && phone.IsPrimary)
                throw new Exception("Primary phone has already been entered.");

            Phones.Add(phone);
        }

        public void RemovePhone(Phone phone)
        {
            Guard.ForNull(phone, "phone");
            Phones.Remove(phone);
        }

        public void SetPhones(IList<Phone> phones)
        {
            // Caller may send an empty or null collection, signifying removal/replacement
            if (phones is null || phones?.Count == 0)
                Phones = phones;

            if (phones?.Count > 0)
            {
                // Remove not found phones (phones that caller removed)
                foreach (var phone in Phones)
                    if (!Phones.Any(x => x.Id == phone.Id))
                        RemovePhone(phone);

                // Find and Update each with caller changes
                foreach (var phone in phones)
                {
                    var foundPhone = Phones.FirstOrDefault(p => p.Id == phone.Id);

                    if (foundPhone is not null)
                    {
                        foundPhone.SetNumber(phone.Number);
                        foundPhone.SetPhoneType(phone.PhoneType);
                        foundPhone.SetIsPrimary(phone.IsPrimary);
                    }
                }

                // Add each NEW phone
                foreach (var phone in phones)
                    if (phone.Id == 0)
                        AddPhone(phone);
            }
        }

        public void AddEmail(Email email)
        {
            Guard.ForNull(email, "email");

            if (ContactableHasEmail(email))
                throw new Exception("Duplicate email.");

            if (ContactableHasPrimaryEmail() && email.IsPrimary)
                throw new Exception("Primary email has already been entered.");

            Emails.Add(email);
        }

        public void RemoveEmail(Email email)
        {
            Guard.ForNull(email, "email");
            Emails.Remove(email);
        }

        public void SetEmails(IList<Email> emails)
        {
            // Client may send an empty or null collection, signifying removal/replacement
            if (emails is null || emails?.Count == 0)
                Emails = emails;

            if (emails.Count > 0)
            {
                // Remove not found phones (phones that caller removed)
                foreach (var email in emails)
                    if (!Emails.Any(x => x.Id == email.Id))
                        RemoveEmail(email);

                // Find and Update each with caller changes
                foreach (var email in emails)
                {
                    var foundEmail = Emails.FirstOrDefault(p => p.Id == email.Id);

                    if (foundEmail is not null)
                    {
                        foundEmail.SetAddress(email.Address);
                        foundEmail.SetIsPrimary(email.IsPrimary);
                    }
                }
                // Add each NEW email
                foreach (var email in emails)
                    if (email.Id == 0)
                        AddEmail(email);
            }
        }

        public void SetAddress(Address address)
        {
            // Guard unnecessarily throws exception; we just need a null check.
            // Address is guaranteed to be valid; it was validated on creation.
            // Address is optional, so excluding it shouldn't throw an exception:
            // BTW, if user removes Address, it will be null here so use it.
            Address = address;
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

        private static bool HasOnlyOnePrimaryPhone(IList<Phone> phones)
        {
            int primaryCount = 0;

            foreach (var phone in phones)
            {
                if (phone is null)
                    continue;

                if (phone.IsPrimary)
                    primaryCount += 1;
            }

            if (primaryCount > 1)
            {
                return false;
            }

            return true;
        }

        // EF requires empty constructor
        public Contactable() { }
    }
}
