using Migrations.Core.ValueObjects;
using SharedKernel;
using System;

namespace Migrations.Core.Entities
{
    public class Organization : Entity
    {
        public static readonly string OrganizationNameEmptyMessage = "Name cannot be empty";
        public static readonly string OrganizationContactNullMessage = "Contact Person cannot be empty";

        public Organization(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException(OrganizationNameEmptyMessage);
            }
            
            Name = name;
        }

        public Organization(string name, Person contact) : this (name)
        {
            if (contact == null)
            {
                throw new ArgumentException(OrganizationContactNullMessage);
            }
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
