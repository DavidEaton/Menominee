using CustomerVehicleManagement.Domain.Enums;
using CustomerVehicleManagement.Domain.Interfaces;
using CustomerVehicleManagement.Domain.ValueObjects;
using SharedKernel;
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
                Entity = organization;

            if (entity is Person person)
                Entity = person;

            EntityId = entity.Id;
            Vehicles = new List<Vehicle>();
            Phones = new List<Phone>();
        }

        // Person or Organization
        public IEntity Entity { get; set; }
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
        public DateTime Created { get; set; }
        public DateTime? ValidThru { get; set; }
        public IList<Phone> Phones { get; set; }
        public IList<Vehicle> Vehicles { get; private set; }

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
