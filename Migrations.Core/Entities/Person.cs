using Migrations.Core.Enums;
using Migrations.Core.ValueObjects;
using SharedKernel.Enums;
using SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Migrations.Core.Entities
{
    public class Person : IEntity
    {
        public Person()
        {
        }
        public Person(string lastName, string firstName, string middleName = null, DriversLicence driversLicence = null)
        {
            LastName = lastName;
            FirstName = firstName;
            MiddleName = string.IsNullOrWhiteSpace(middleName) ? null : middleName;
            DriversLicence = driversLicence;
            // Keep EF informed of object state in disconnected api
            TrackingState = TrackingState.Added;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

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
        //public int? DriversLicenceId { get; set; }
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
        
        [NotMapped]
        public TrackingState TrackingState { get; private set; }
        public void UpdateState(TrackingState state)
        {
            TrackingState = state;
        }
    }
}
