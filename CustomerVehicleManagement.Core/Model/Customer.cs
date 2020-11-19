using CustomerVehicleManagement.Core.Enums;
using CustomerVehicleManagement.Core.Interfaces;
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
            if (EntityType == EntityType.Organization)
                Organization = (Organization)entity;

            if (EntityType == EntityType.Person)
                Person = (Person)entity;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Person Person { get; set; }
        public Organization Organization { get; set; }
        public EntityType EntityType
        {
            get => (Organization != null) ? EntityType.Organization : EntityType.Person;

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
        public IList<Vehicle> Vehicles { get; private set; }
    }
}
