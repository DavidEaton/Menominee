using CustomerVehicleManagement.Domain.Interfaces;
using SharedKernel;
using SharedKernel.Enums;
using SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CustomerVehicleManagement.Domain.Entities
{
    public class Customer : Entity, ICustomer
    {
        // Blazor 5 requires public JsonConstructor-attributed contructor, 
        [JsonConstructor]
        public Customer(IEntity entity)
        {
            if (entity is Organization organization)
            {
                Entity = organization;
                EntityType = EntityType.Organization;
            }

            if (entity is Person person)
            {
                Entity = person;
                EntityType = EntityType.Person;
            }

            EntityId = entity.Id;
        }

        // Person or Organization
        public IEntity Entity { get; set; }
        public EntityType EntityType { get; set; }
        public int EntityId { get; set; }
        public CustomerType CustomerType { get; set; }
        public bool AllowMail { get; set; }
        public bool AllowEmail { get; set; }
        public bool AllowSms { get; set; }
        public int PriceProfileId { get; set; }
        public int TaxIds { get; set; }
        public bool RewardsMember { get; set; }
        public bool OverrideCustomerTaxProfile { get; set; }
        public DateTime Created { get; set; }
        public IList<Phone> Phones { get; set; } = new List<Phone>();
        public IList<Vehicle> Vehicles { get; private set; } = new List<Vehicle>();

        public void AddPhone(Phone phone)
        {
            Phones.Add(phone);
        }

        public void RemovePhone(Phone phone)
        {
            Phones.Remove(phone);
        }

        public void AddVehicle(Vehicle vehicle)
        {
            Vehicles.Add(vehicle);
        }

        public void RemoveVehicle(Vehicle vehicle)
        {
            Vehicles.Remove(vehicle);
        }

        #region ORM

        // EF requires an empty constructor
        protected Customer() { }

        #endregion
    }
}
