using SharedKernel;
using SharedKernel.Enums;
using SharedKernel.ValueObjects;
using System;
using System.Collections.Generic;
using SharedKernel.Utilities;
using System.Linq;

namespace CustomerVehicleManagement.Domain.Entities
{
    public class Customer : Entity
    {
        public Person Person { get; private set; }
        public Organization Organization { get; private set; }
        public EntityType EntityType => GetEntityType();
        public CustomerType CustomerType { get; private set; }
        public ContactPreferences ContactPreferences { get; private set; }
        public DateTime Created { get; private set; }
        public virtual IList<Vehicle> Vehicles { get; private set; } = new List<Vehicle>();
        public Customer(Person person)
        {
            Guard.ForNull(person, "person == null");

            Person = person;
            CustomerType = CustomerType.Retail;
            Created = DateTime.UtcNow;
        }

        public Customer(Organization organization)
        {
            Guard.ForNull(organization, "organization == null");

            Organization = organization;
            CustomerType = CustomerType.Retail;
            Created = DateTime.UtcNow;
        }

        private EntityType GetEntityType()
        {
            if (Person is not null)
                return EntityType.Person;

            if (Organization is not null)
                return EntityType.Organization;

            throw new InvalidOperationException("Unknown entity type");
        }

        public void AddVehicle(Vehicle vehicle)
        {
            Guard.ForNull(vehicle, "vehicle");

            if (CustomerHasVehicle(vehicle))
                throw new Exception("customer already has this vehicle.");

            Vehicles.Add(vehicle);
        }

        public void RemoveVehicle(Vehicle vehicle)
        {
            Guard.ForNull(vehicle, "vehicle");
            Vehicles.Remove(vehicle);
        }

        private bool CustomerHasVehicle(Vehicle vehicle)
        {
            return Vehicles.Any(x => x == vehicle);
        }
        #region ORM

        // EF requires an empty constructor
        protected Customer() { }

        #endregion

    }
}
