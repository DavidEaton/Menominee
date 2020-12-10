using SharedKernel.Enums;
using SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Migrations.Core.Entities
{
    public class Organization : IEntity
    {
        // EF reuires an empty constructor
        protected Organization()
        {
        }

        public Organization(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name cannot be empty");
            }
            
            Name = name;
            Phones = new List<Phone>();
            // Keep EF informed of object state in disconnected api
            TrackingState = TrackingState.Added;
        }

        public Organization(string name, Person contact) : this (name)
        {
            Contact = contact;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        [MinLength(1)]
        public string Name { get; set; }
        public Person Contact { get; set; }
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
