using SharedKernel;
using SharedKernel.Enums;
using SharedKernel.ValueObjects;
using System;
using System.Collections.Generic;
using SharedKernel.Utilities;
using System.Linq;

namespace CustomerVehicleManagement.Domain.Entities
{
    // VK: the overarching idea here is to move Contactable's logic to Customer and then inherit Person and Organization from it
    // To store the hierarchy in the DB, there are 2 common strategies: Table-per-hierarchy and Table-per-type
    // I usually use Table-per-hierarchy: it's simple and straightforward. Here the documentation: https://docs.microsoft.com/en-us/ef/core/modeling/inheritance

    public abstract class Customer : Entity
    {
        public virtual IList<Phone> Phones { get; private set; } = new List<Phone>();
        public virtual IList<Email> Emails { get; private set; } = new List<Email>();
        public CustomerType CustomerType { get; private set; }
        public ContactPreferences ContactPreferences { get; private set; }
        public DateTime Created { get; private set; }
        public virtual IList<Vehicle> Vehicles { get; private set; } = new List<Vehicle>();
        // VK: I've moved the Address here because it is present in both Person and Organization
        public Address Address { get; private set; }

        public Customer()
        {
            Created = DateTime.UtcNow;
        }

        public void AddPhone(Phone phone)
        {
            if (DuplicatePhoneNumberExists(phone, Phones))
                throw new Exception("duplicate phone");

            if (PrimaryPhoneExists(Phones) && phone.IsPrimary)
                throw new Exception("primary phone already exists.");

            // VK: no need for this since Phones is initialized on class instantiation
            //if (Phones == null)
            //    Phones = new List<Phone>();

            Phones.Add(phone);
        }

        public void RemovePhone(Phone phone)
        {
            // VK: phone number being null is usually a bug, so best to put a guard here instead of the null check
            Guard.ForNull(phone, "phone");
            Phones.Remove(phone);
        }

        public void SetPhones(IList<Phone> phones)
        {
            Guard.ForNull(phones, "phones");

            if (DuplicatePhoneExists(phones))
                throw new Exception("phones == null");

            if (PrimaryPhoneCountExceedsOne(phones))
                throw new Exception("primary phone already exists.");

            Phones = phones;
        }

        public void AddEmail(Email email)
        {
            Guard.ForNull(email, "email");

            if (DuplicateEmailExists(email, Emails))
                throw new Exception("duplicate email.");

            if (PrimaryEmailExists(Emails) && email.IsPrimary)
                throw new Exception("primary email exists.");

            Emails.Add(email);
        }

        public void RemoveEmail(Email email)
        {
            Guard.ForNull(email, "email");
            Emails.Remove(email);
        }

        public void SetEmails(IList<Email> emails)
        {
            Guard.ForNull(emails, "emails");

            if (DuplicateEmailExists(emails))
                throw new Exception("duplicate email.");

            if (PrimaryEmailCountExceedsOne(emails))
                throw new Exception("primary email exists.");

            Emails = emails;
        }

        public void AddVehicle(Vehicle vehicle)
        {
            Guard.ForNull(vehicle, "vehicle");
            Vehicles.Add(vehicle);
        }

        public void RemoveVehicle(Vehicle vehicle)
        {
            Guard.ForNull(vehicle, "vehicle");
            Vehicles.Remove(vehicle);
        }

        public void SetAddress(Address address)
        {
            Guard.ForNull(address, "address");
            Address = address;
        }

        public static bool DuplicatePhoneNumberExists(Phone phone, IList<Phone> phones)
        {
            Guard.ForNull(phones, "phones");

            bool result = false;

            foreach (var existingPhone in phones)
            {
                if (existingPhone.Number == phone.Number)
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        public static bool PrimaryPhoneExists(IList<Phone> phones)
        {
            Guard.ForNull(phones, "phones");

            bool result = false;

            foreach (var existingPhone in phones)
            {
                if (existingPhone.IsPrimary)
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        public static bool DuplicatePhoneExists(IList<Phone> phones)
        {
            Guard.ForNull(phones, "phones");

            return phones.Count != phones.Distinct().Count();
        }

        public static bool PrimaryPhoneCountExceedsOne(IList<Phone> phones)
        {
            Guard.ForNull(phones, "phones");

            int primaryPhoneCount = 0;

            foreach (var existingPhone in phones)
                if (existingPhone.IsPrimary)
                    primaryPhoneCount += 1;

            return primaryPhoneCount > 1;
        }

        public static bool PrimaryEmailExists(IList<Email> emails)
        {
            Guard.ForNull(emails, "emails");

            bool result = false;

            foreach (var existingEmail in emails)
            {
                if (existingEmail.IsPrimary)
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        public static bool PrimaryEmailCountExceedsOne(IList<Email> emails)
        {
            Guard.ForNull(emails, "emails");

            int primaryEmailCount = 0;

            foreach (var existingEmail in emails)
                if (existingEmail.IsPrimary)
                    primaryEmailCount += 1;

            return primaryEmailCount > 1;
        }

        public static bool DuplicateEmailExists(IList<Email> emails)
        {
            Guard.ForNull(emails, "emails");

            return emails.Count != emails.Distinct().Count();
        }

        public static bool DuplicateEmailExists(Email email, IList<Email> emails)
        {
            Guard.ForNull(emails, "emails");

            bool result = false;

            foreach (var existingEmail in emails)
            {
                if (existingEmail.Address == email.Address)
                {
                    result = true;
                    break;
                }
            }

            return result;
        }
    }
}
