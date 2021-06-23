using SharedKernel.Utilities;
using SharedKernel.ValueObjects;

namespace CustomerVehicleManagement.Domain.Entities
{
    public class Organization : Customer
    {
        public Organization(OrganizationName name)
        {
            Name = name;
        }

        public OrganizationName Name { get; private set; }
        public virtual Person Contact { get; private set; }
        public string Notes { get; private set; }

        public void SetName(OrganizationName name)
        {
            Name = name;
        }

        public void SetContact(Person contact)
        {
            Guard.ForNull(contact, "contact");

            Contact = contact;
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
