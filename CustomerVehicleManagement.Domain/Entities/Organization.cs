using CustomerVehicleManagement.Domain.BaseClasses;
using Menominee.Common.ValueObjects;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Domain.Entities
{
    public class Organization : Contactable
    {
        public Organization(OrganizationName name,
                            string note,
                            Person contact,
                            Address address = null,
                            IList<Email> emails = null,
                            IList<Phone> phones = null)
            : base(address, phones, emails)
        {
            Name = name;
            Note = note;
            Contact = contact;
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
            Note = note.Trim();
        }

        #region ORM

        // Code that pollutes our domain class (very minor impact in this case), but
        // is necessary for EntityFramework, makes our model <100% persistence ignorant.

        // EF requires an empty constructor
        protected Organization() { }

        #endregion
    }
}
