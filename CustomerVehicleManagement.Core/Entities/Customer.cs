using CustomerVehicleManagement.Domain.Enums;
using CustomerVehicleManagement.Domain.Interfaces;
using SharedKernel;
using SharedKernel.Enums;
using SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Domain.Entities
{
    public class Customer : Entity, ICustomer
    {
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
            Vehicles = new List<Vehicle>();
            Phones = new List<Phone>();
        }

        // Person or Organization
        public IEntity Entity { get; private set; }
        public EntityType EntityType { get; private set; }
        public int EntityId { get; private set; }
        public CustomerType CustomerType { get; private set; }
        public bool AllowMail { get; private set; }
        public bool AllowEmail { get; private set; }
        public bool AllowSms { get; private set; }
        public int PriceProfileId { get; private set; }
        public int TaxIds { get; private set; }
        public bool RewardsMember { get; private set; }
        public bool OverrideCustomerTaxProfile { get; private set; }
        public DateTime Created { get; private set; }
        public IList<Phone> Phones { get; private set; } = new List<Phone>();
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

        public void SetEntity(Entity entity)
        {
            Entity = entity;
        }

        #region ORM

        // EF requires an empty constructor
        protected Customer() { }

        #endregion
    }
}
