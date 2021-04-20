using CustomerVehicleManagement.Domain.BaseClasses;
using SharedKernel;
using SharedKernel.ValueObjects;

namespace CustomerVehicleManagement.Domain.Entities
{
    public class Organization : Contactable
    {
        public Organization(OrganizationName name)
        {
            Name = name;
        }

        public OrganizationName Name { get; private set; }
        public virtual Person Contact { get; private set; }
        public Address Address { get; private set; }
        public string Notes { get; private set; }

        public void SetName(OrganizationName name)
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
            if (notes != null)
                Notes = notes;
        }

        #region ORM

        // EF requires an empty constructor
        protected Organization() { }

        #endregion
    }
}
