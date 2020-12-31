using Migrations.Core.ValueObjects;
using SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Migrations.Core.Entities
{
    public class Organization : Entity
    {
        public static readonly string OrganizationNameEmptyMessage = "Name cannot be empty";
        public static readonly string OrganizationContactNullMessage = "Contact Person cannot be empty";

        // EF requires an empty constructor
        protected Organization()
        {
        }

        public Organization(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException(OrganizationNameEmptyMessage);
            }
            
            Name = name;
            Phones = new List<Phone>();
        }

        public Organization(string name, Person contact) : this (name)
        {
            if (contact == null)
            {
                throw new ArgumentException(OrganizationContactNullMessage);
            }
        }

        [Required]
        [MaxLength(255)]
        [MinLength(1)]
        public string Name { get; set; }
        public Person Contact { get; set; }
        public Address Address { get; set; }
        public ICollection<Phone> Phones { get; set; }
    }
}
