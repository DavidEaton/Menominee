using Menominee.Common;
using Menominee.Common.Enums;
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

            Person = person;
            CustomerType = customerType;
        }

        public Customer(Organization organization, CustomerType customerType)
        {

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

        public void AddPhone(Phone phone)
        {

            if (CustomerHasPhone(phone))
                throw new InvalidOperationException("customer already has this phone.");

            if (EntityType is EntityType.Person)
                Person.Phones.Add(phone);

            if (EntityType is EntityType.Organization)
                Organization.Phones.Add(phone);
        }

        private bool CustomerHasPhone(Phone phone)
        {

            if (EntityType is EntityType.Person)
                return Person.Phones.Any(x => x == phone);

            if (EntityType is EntityType.Organization)
                return Organization.Phones.Any(x => x == phone);

            throw new InvalidOperationException("customer is unknown entity type.");
        }

        public void AddEmail(Email email)
        {

            if (CustomerHasEmail(email))
                throw new InvalidOperationException("customer already has this email.");

            if (EntityType is EntityType.Person)
                Person.Emails.Add(email);

            if (EntityType is EntityType.Organization)
                Organization.Emails.Add(email);
        }

        private bool CustomerHasEmail(Email email)
        {

            if (EntityType is EntityType.Person)
                return Person.Emails.Any(x => x == email);

            if (EntityType is EntityType.Organization)
                return Organization.Emails.Any(x => x == email);

            throw new InvalidOperationException("customer is unknown entity type.");
        }

        public void AddVehicle(Vehicle vehicle)
        {

            if (CustomerHasVehicle(vehicle))
                throw new InvalidOperationException("customer already has this vehicle.");

            Vehicles.Add(vehicle);
        }

        public void RemoveVehicle(Vehicle vehicle)
        {
            Vehicles.Remove(vehicle);
        }

        private bool CustomerHasVehicle(Vehicle vehicle)
        {
            return Vehicles.Any(v => v == vehicle);
        }
        #region ORM

        // EF requires an empty constructor
        protected Customer() { }

        #endregion

    }
}
