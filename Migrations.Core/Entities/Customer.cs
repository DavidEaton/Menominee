using Migrations.Core.Enums;
using Migrations.Core.Interfaces;
using SharedKernel.Enums;
using SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Migrations.Core.Entities
{
    public class Customer : IEntity, ICustomer
    {
        // EF requires an empty constructor
        protected Customer()
        {
        }

        public Customer(IEntity entity)
        {
            if (entity is Organization organization)
                Entity = organization;

            if (entity is Person person)
                Entity = person;

            EntityId = entity.Id;
            Vehicles = new List<Vehicle>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [NotMapped]
        public IEntity Entity { get; set; }

        public EntityType EntityType { get; set; }
        public int EntityId { get; set; }

        [NotMapped]
        public Address Address { get; set; }

        [NotMapped]
        public ICollection<Phone> Phones { get; private set; }

        public CustomerType CustomerType { get; set; }
        public bool AllowMail { get; set; }
        public bool AllowEmail { get; set; }
        public bool AllowSms { get; set; }
        public int PriceProfileId { get; set; }
        public int TaxIds { get; set; }
        public bool RewardsMember { get; set; }
        public bool OverrideCustomerTaxProfile { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime? ValidThru { get; set; }
        public ICollection<Vehicle> Vehicles { get; private set; }

        [NotMapped]
        public TrackingState TrackingState { get; private set; }
        public void UpdateState(TrackingState state)
        {
            TrackingState = state;
        }
    }
}
