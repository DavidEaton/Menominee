using CustomerVehicleManagement.Core.Enums;
using SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerVehicleManagement.Core.Model
{
    public class Customer : IEntity
    {
        protected Customer()
        {
            Vehicles = new List<Vehicle>();
        }

        public Customer(IEntity entity)
        {
            if (entity is Organization organization)
                Entity = organization;

            if (entity is Person person)
                Entity = person;

            EntityId = entity.Id;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [NotMapped]
        public IEntity Entity { get; set; }
        public int EntityId { get; set; }

        [NotMapped]
        public EntityType EntityType
        {
            get => (Entity.GetType().ToString() == EntityType.Organization.ToString()) ? EntityType.Organization : EntityType.Person;
        }

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
    }
}
