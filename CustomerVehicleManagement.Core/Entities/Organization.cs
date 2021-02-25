using CustomerVehicleManagement.Domain.Interfaces;
using CustomerVehicleManagement.Domain.Utilities;
using SharedKernel;
using SharedKernel.Utilities;
using SharedKernel.ValueObjects;
using System;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Domain.Entities
{
    public class Organization : Entity, IListOfPhone
    {
        public static readonly string OrganizationNameEmptyMessage = "Name cannot be empty";
        public static readonly string DuplicatePhoneExistsMessage = "Cannot add duplicate phone.";
        public static readonly string PrimaryPhoneExistsMessage = "Cannot add more than one Primary phone.";
        public static readonly string DuplicateEmailExistsMessage = "Cannot add duplicate email.";
        public static readonly string PrimaryEmailExistsMessage = "Cannot add more than one Primary email.";
        public static readonly string EmptyEmailCollectionMessage = "Cannot add an empty email list";

        public Organization(string name)
            : this(name, null)
        {
        }

        public Organization(string name, Address address)
            : this(name, address, null)
        {
        }

        public Organization(string name, Address address, Person contact)
            : this(name, address, contact, null)
        {
        }

        public Organization(string name, Address address, Person contact, IList<Phone> phones)
        {
            try
            {
                Guard.ForNullOrEmpty(name, "name");

            }
            catch (Exception)
            {
                throw new ArgumentException(OrganizationNameEmptyMessage);
            }

            Name = name;
            Address = address;
            Contact = contact;
            Phones = phones;
        }

        public string Name { get; private set; }
        public virtual Person Contact { get; private set; }
        public Address Address { get; private set; }
        public string Notes { get; private set; }
        public virtual IList<Phone> Phones { get; private set; } = new List<Phone>();
        public virtual IList<Email> Emails { get; private set; } = new List<Email>();

        public void AddPhone(Phone phone)
        {
            if (PhoneHelpers.DuplicatePhoneNumberExists(phone, Phones))
                throw new ArgumentException(DuplicatePhoneExistsMessage);

            if (PhoneHelpers.PrimaryPhoneExists(Phones) && phone.IsPrimary)
                throw new ArgumentException(PrimaryPhoneExistsMessage);

            if (Phones == null)
                Phones = new List<Phone>();

            if (Phones != null)
                Phones.Add(phone);
        }

        public void RemovePhone(Phone phone)
        {
            Phones.Remove(phone);
        }

        public void SetPhones(IList<Phone> phones)
        {
            if (phones != null)
                Phones = phones;
        }

        public void AddEmail(Email email)
        {
            if (EmailHelpers.DuplicateEmailExists(email, Emails))
                throw new ArgumentException(DuplicateEmailExistsMessage);

            if (EmailHelpers.PrimaryEmailExists(Emails) && email.IsPrimary)
                throw new ArgumentException(PrimaryEmailExistsMessage);

            if (Emails == null)
                Emails = new List<Email>();

            if (Emails != null)
                Emails.Add(email);
        }

        public void RemoveEmail(Email email)
        {
            Emails.Remove(email);
        }

        public void SetEmails(IList<Email> emails)
        {
            if (emails == null)
                throw new ArgumentException(EmptyEmailCollectionMessage);


            if (EmailHelpers.DuplicateEmailExists(emails))
                throw new ArgumentException(DuplicateEmailExistsMessage);


            if (EmailHelpers.PrimaryEmailCountExceedsOne(emails))
                throw new ArgumentException(PrimaryEmailExistsMessage);

            Emails = emails;
        }

        public void SetName(string name)
        {
            Name = name;
        }

        public void SetContact(Person contact)
        {
            if (contact != null)
                Contact = contact;
        }

        public void SetAddress(Address address)
        {
            Address = address;
        }

        public void SetNotes(string notes)
        {
            Notes = notes;
        }

        #region ORM

        // EF requires an empty constructor
        protected Organization() { }

        #endregion
    }
}
