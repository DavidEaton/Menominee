using Menominee.Common;
using Menominee.Common.Enums;
using Menominee.Common.Utilities;
using Menominee.Common.ValueObjects;
using System;
using System.Collections.Generic;
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
        public virtual IList<Vehicle> Vehicles { get; private set; } = new List<Vehicle>();

        public Customer(Person person, CustomerType customerType)
        {
            Guard.ForNull(person, "person == null");

            Person = person;
            CustomerType = customerType;
        }

        public Customer(Organization organization, CustomerType customerType)
        {
            Guard.ForNull(organization, "organization == null");

            Organization = organization;
            CustomerType = customerType;
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
