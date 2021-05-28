using CustomerVehicleManagement.Domain.Interfaces;
using SharedKernel;
using SharedKernel.Enums;
using SharedKernel.Interfaces;
using SharedKernel.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CustomerVehicleManagement.Domain.Entities
{
    public class Customer : Entity, ICustomer
    {
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

            if (entity != null)
                EntityId = entity.Id;

            Created = DateTime.UtcNow;
        }

        // Person or Organization
        [JsonInclude]
        public IEntity Entity { get; private set; }

        [JsonInclude]
        public EntityType EntityType { get; private set; }

        [JsonInclude]
        public int EntityId { get; private set; }

        [JsonInclude]
        public CustomerType CustomerType { get; private set; }

        [JsonInclude]
        public ContactPreferences ContactPreferences { get; private set; }

        [JsonInclude]
        public DateTime Created { get; private set; }

        [JsonInclude]
        public virtual IList<Phone> Phones { get; private set; } = new List<Phone>();

        [JsonInclude]
        public virtual IList<Email> Emails { get; private set; } = new List<Email>();

        [JsonInclude]
        public virtual IList<Vehicle> Vehicles { get; private set; } = new List<Vehicle>();

        public void AddPhone(Phone phone)
        {
            Phones.Add(phone);
        }

        public void RemovePhone(Phone phone)
        {
            Phones.Remove(phone);
        }

        public void AddEmail(Email email)
        {
            Emails.Add(email);
        }

        public void RemoveEmail(Email email)
        {
            Emails.Remove(email);
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
