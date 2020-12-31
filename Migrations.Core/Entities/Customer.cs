using Migrations.Core.Enums;
using Migrations.Core.Interfaces;
using Migrations.Core.ValueObjects;
using SharedKernel;
using SharedKernel.Enums;
using SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace Migrations.Core.Entities
{
    public class Customer : Entity, ICustomer
    {
        // EF requires an empty constructor
        protected Customer() { }

        public Customer(IEntity entity)
        {
            if (entity is Organization organization)
                Entity = organization;

            if (entity is Person person)
                Entity = person;

            EntityId = entity.Id;
            Vehicles = new List<Vehicle>();
        }

        public IEntity Entity { get; set; }
        public EntityType EntityType { get; set; }
        public int EntityId { get; set; }
        public Address Address { get; set; }
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
        public ICollection<Phone> Phones { get; private set; }
        public ICollection<Vehicle> Vehicles { get; private set; }
    }
}
