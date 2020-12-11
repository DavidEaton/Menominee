using Migrations.Core.Enums;
using Migrations.Core.Interfaces;
using Migrations.Core.ValueObjects;
using SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Migrations.Core.Entities
{
    public class Person : Entity, IPerson
    {
        public static readonly string PersonNameEmptyMessage = "First and Last Names cannot be empty";

        // EF requires an empty constructor
        protected Person()
        {
        }

        public Person(string lastName, string firstName, string middleName = null, DriversLicence driversLicence = null)
        {
            if (string.IsNullOrWhiteSpace(lastName) && string.IsNullOrWhiteSpace(firstName))
            {
                throw new ArgumentException(PersonNameEmptyMessage);
            }

            LastName = lastName;
            FirstName = firstName;
            MiddleName = string.IsNullOrWhiteSpace(middleName) ? null : middleName;
            Phones = new List<Phone>();
            DriversLicence = driversLicence;
        }

        [Required]
        [MaxLength(255)]
        [MinLength(1)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(255)]
        [MinLength(1)]
        public string FirstName { get; set; }

        [MaxLength(255)]
        [MinLength(1)]
        public string MiddleName { get; set; }
        public DateTime? Birthday { get; set; }

        [StringLength(1)]
        public Gender Gender { get; set; }
        public DriversLicence DriversLicence { get; set; }
        public string NameLastFirst
        {
            get => string.IsNullOrWhiteSpace(MiddleName) ? $"{LastName}, {FirstName}" : $"{LastName}, {FirstName} {MiddleName}";
        }
        public string NameFirstLast
        {
            get => string.IsNullOrWhiteSpace(MiddleName) ? $"{FirstName} {LastName}" : $"{FirstName} {MiddleName} {LastName}";
        }
        public Address Address { get; set; }
        public ICollection<Phone> Phones { get; set; }
    }
}
