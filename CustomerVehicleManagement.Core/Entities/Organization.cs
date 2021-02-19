using CustomerVehicleManagement.Domain.Interfaces;
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

        public Organization(string name)
            : this(name, null)
        {
        }

        public Organization(string name, Address address)
            : this(name, address, null)
        {
        }

        public Organization(string name, Address address, Person contact)
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
        }

        public string Name { get; private set; }
        public virtual Person Contact { get; private set; }
        public Address Address { get; private set; }
        public string Notes { get; private set; }
        public virtual IList<Phone> Phones { get; private set; } = new List<Phone>();

        public void AddPhone(Phone phone)
        {
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
