using CustomerVehicleManagement.Domain.ValueObjects;
using SharedKernel;
using System;

namespace CustomerVehicleManagement.Domain.Entities
{
    public class Organization : Entity
    {
        public static readonly string OrganizationNameEmptyMessage = "Name cannot be empty";
        //public static readonly string OrganizationContactNullMessage = "Contact Person cannot be empty";

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

        public string Name { get; set; }
        public Person Contact { get; set; }
        public Address Address { get; set; }

        #region ORM

        // EF requires an empty constructor
        protected Organization() { }

        #endregion
    }
}
