using CustomerVehicleManagement.Domain.BaseClasses;
using SharedKernel.Utilities;
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
        public string Note { get; private set; }

        public void SetName(OrganizationName name)
        {
            Name = name;
        }

        public void SetContact(Person contact)
        {
            if (contact != null)
                Contact = contact;
        }

        public void SetNote(string note)
        {
            if (note != null)
                Note = note;
        }

        #region ORM

        // EF requires an empty constructor
        protected Organization() { }

        #endregion
    }
}
