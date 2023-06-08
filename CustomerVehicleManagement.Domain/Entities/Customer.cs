using CSharpFunctionalExtensions;
using Menominee.Common.Enums;
using Menominee.Common.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using Entity = Menominee.Common.Entity;

namespace CustomerVehicleManagement.Domain.Entities
{
    public class Customer : Entity
    {
        public static readonly string DuplicateItemMessagePrefix = $"Customer already has this ";
        public static readonly string UnknownEntityTypeMessage = $"Customer is unknown entity type.";
        public static readonly string RequiredMessage = "Please include all required items.";

        public Person Person { get; private set; }
        public Organization Organization { get; private set; }
        public EntityType EntityType => GetEntityType();
        public CustomerType CustomerType { get; private set; }
        private DateTime Created { get; set; }
        public ContactPreferences ContactPreferences { get; private set; }

        private readonly List<Vehicle> vehicles = new();
        public IReadOnlyList<Vehicle> Vehicles => vehicles.ToList();
        public string Name =>
            Person is not null
                ? Person.Name.FirstMiddleLast 
                : Organization is not null 
                    ? Organization.Name.Name 
                    : string.Empty;

        public string Notes =>
            Person is not null 
                ? Person.Notes 
                : Organization is not null 
                    ? Organization.Notes 
                    : string.Empty;

        public IReadOnlyList<Phone> Phones =>
            Person is not null 
                ? Person.Phones.ToList()
                : Organization is not null 
                    ? Organization.Phones.ToList() 
                    : new List<Phone>();

        public IReadOnlyList<Email> Emails =>
            Person is not null
                ? Person.Emails.ToList()
                : Organization is not null
                    ? Organization.Emails.ToList()
                    : new List<Email>();

        public Address Address =>
            Person is not null
                ? Person.Address
                : Organization is not null
                    ? Organization.Address 
                    : null;

        public Person Contact => Organization?.Contact;

        private Customer(Person person, CustomerType customerType)
        {
            Person = person;
            CustomerType = customerType;
            Created = DateTime.Now;
        }

        private Customer(Organization organization, CustomerType customerType)
        {
            Organization = organization;
            CustomerType = customerType;
            Created = DateTime.Now;
        }

        public static Result<Customer> Create(Person person, CustomerType customerType)
        {
            if (person is null)
                return Result.Failure<Customer>(RequiredMessage);

            if (!Enum.IsDefined(typeof(CustomerType), customerType))
                return Result.Failure<Customer>(RequiredMessage);

            return Result.Success(new Customer(person, customerType));
        }

        public static Result<Customer> Create(Organization organization, CustomerType customerType)
        {
            if (organization is null)
                return Result.Failure<Customer>(RequiredMessage);

            if (!Enum.IsDefined(typeof(CustomerType), customerType))
                return Result.Failure<Customer>(RequiredMessage);

            return Result.Success(new Customer(organization, customerType));
        }

        private EntityType GetEntityType() => Person is not null
                ? EntityType.Person
                : Organization is not null
                    ? EntityType.Organization
                    : throw new InvalidOperationException(UnknownEntityTypeMessage);

        public Result SetAddress(Address address)
        {
            // Address (if present) is guaranteed to be valid;
            // it was validated on creation.
            if (address is null)
                return Result.Failure<Address>(RequiredMessage);

            if (Person is not null)
                return Result.Success(Person.SetAddress(address));

            if (Organization is not null)
                return Result.Success(Organization.SetAddress(address));

            return Result.Failure<Address>(UnknownEntityTypeMessage);
        }

        public Result ClearAddress()
        {
            if (Person is not null)
                return Result.Success(Person.ClearAddress());

            if (Organization is not null)
                return Result.Success(Organization.ClearAddress());

            return Result.Failure<Address>(UnknownEntityTypeMessage);
        }

        public Result<Phone> AddPhone(Phone phone)
        {
            if (phone is null)
                return Result.Failure<Phone>(RequiredMessage);

            if (CustomerHasPhone(phone))
                return Result.Failure<Phone>($"{DuplicateItemMessagePrefix} Phone: {phone.PhoneType} - {phone.ToString}");

            switch (EntityType)
            {
                case EntityType.Person:
                    Person.AddPhone(phone);
                    return Result.Success(phone);

                case EntityType.Organization:
                    Organization.AddPhone(phone);
                    return Result.Success(phone);

                default: return Result.Failure<Phone>(UnknownEntityTypeMessage);
            }
        }

        public Result<Phone> RemovePhone(Phone phone)
        {
            if (phone is null)
                return Result.Failure<Phone>(RequiredMessage);

            switch (EntityType)
            {
                case EntityType.Person:
                    Person.RemovePhone(phone);
                    return Result.Success(phone);

                case EntityType.Organization:
                    Organization.RemovePhone(phone);
                    return Result.Success(phone);

                default: return Result.Failure<Phone>(UnknownEntityTypeMessage);
            }
        }

        private bool CustomerHasPhone(Phone phone)
        {
            if (Person is not null)
                return Person.Phones.Any(x => x == phone);

            if (Organization is not null)
                return Organization.Phones.Any(x => x == phone);

            throw new InvalidOperationException(UnknownEntityTypeMessage);
        }

        public Result<Email> AddEmail(Email email)
        {
            if (email is null)
                return Result.Failure<Email>(RequiredMessage);

            if (CustomerHasEmail(email))
                return Result.Failure<Email>($"{DuplicateItemMessagePrefix} Email: {email.Address}");

            switch (EntityType)
            {
                case EntityType.Person:
                    Person.AddEmail(email);
                    return Result.Success(email);

                case EntityType.Organization:
                    Organization.AddEmail(email);
                    return Result.Success(email);

                default: return Result.Failure<Email>(UnknownEntityTypeMessage);
            }
        }

        private bool CustomerHasEmail(Email email)
        {
            if (Person is not null)
                return Person.Emails.Any(x => x == email);

            if (Organization is not null)
                return Organization.Emails.Any(x => x == email);

            throw new InvalidOperationException(UnknownEntityTypeMessage);
        }

        public Result<Email> RemoveEmail(Email email)
        {
            if (email is null)
                return Result.Failure<Email>(RequiredMessage);

            switch (EntityType)
            {
                case EntityType.Person:
                    Person.RemoveEmail(email);
                    return Result.Success(email);

                case EntityType.Organization:
                    Organization.RemoveEmail(email);
                    return Result.Success(email);

                default: return Result.Failure<Email>(UnknownEntityTypeMessage);
            }
        }
        public Result<Vehicle> AddVehicle(Vehicle vehicle)
        {
            if (vehicle is null)
                return Result.Failure<Vehicle>(RequiredMessage);

            if (CustomerHasVehicle(vehicle))
                return Result.Failure<Vehicle>($"{DuplicateItemMessagePrefix} Vehicle: {vehicle.ToString}, VIN: {vehicle.VIN}");

            vehicles.Add(vehicle);
            return Result.Success(vehicle);
        }

        public Result<Vehicle> RemoveVehicle(Vehicle vehicle)
        {
            if (vehicle is null)
                return Result.Failure<Vehicle>(RequiredMessage);

            vehicles.Remove(vehicle);
            return Result.Success(vehicle);
        }

        private bool CustomerHasVehicle(Vehicle vehicle)
        {
            return Vehicles.Any(v => v == vehicle);
        }

        #region ORM

        // EF requires a parameterless constructor
        protected Customer() { }

        #endregion

    }
}
