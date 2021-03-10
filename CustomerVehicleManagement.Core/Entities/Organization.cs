using CustomerVehicleManagement.Domain.BaseClasses;
using SharedKernel.Utilities;
using SharedKernel.ValueObjects;
using System;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Domain.Entities
{
    public class Organization : Contactable
    {
        public static readonly string OrganizationNameEmptyMessage = "Name cannot be empty";

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
            : this(name, address, contact, phones, null)
        {
        }

        public Organization(string name, Address address, Person contact, IList<Phone> phones, IList<Email> emails)
        {
            try
            {
                Guard.ForNullOrEmpty(name, "name");

            }
            catch (Exception)
            {
                throw new ArgumentException(OrganizationNameEmptyMessage, nameof(name));
            }

            Name = name;
            Address = address;
            Contact = contact;
            if (phones != null) SetPhones(phones);
            if (emails != null) SetEmails(emails);
        }

        public string Name { get; private set; }
        public virtual Person Contact { get; private set; }
        public Address Address { get; private set; }
        public string Notes { get; private set; }

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
            if (address != null)
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
