using CSharpFunctionalExtensions;
using Menominee.Common.Enums;
using Menominee.Common.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using Entity = Menominee.Common.Entity;

namespace Menominee.Domain.Entities
{
    public class Customer : Entity
    {
        public static readonly int MaximumCodeLength = 20;
        public static readonly string DuplicateItemMessagePrefix = $"Customer already has this ";
        public static readonly string UnknownEntityTypeMessage = $"Customer is unknown entity type.";
        public static readonly string RequiredMessage = "Please include all required items.";
        public static readonly string InvalidCodeLengthMessage = $"Code must be {MaximumCodeLength} characters or less.";

        public Person Person { get; private set; }
        public Business Business { get; private set; }
        public EntityType EntityType => GetEntityType();
        public CustomerType CustomerType { get; private set; }
        private DateTime Created { get; set; }
        public ContactPreferences ContactPreferences { get; private set; }
        public string Code { get; private set; }

        private readonly List<Vehicle> vehicles = new();
        public IReadOnlyList<Vehicle> Vehicles => vehicles.ToList();
        public string Name =>
            Person is not null
                ? Person.Name.FirstMiddleLast
                : Business is not null
                    ? Business.Name.Name
                    : string.Empty;

        public string Notes =>
            Person is not null
                ? Person.Notes
                : Business is not null
                    ? Business.Notes
                    : string.Empty;

        public IReadOnlyList<Phone> Phones =>
            Person is not null
                ? Person.Phones.ToList()
                : Business is not null
                    ? Business.Phones.ToList()
                    : new List<Phone>();

        public IReadOnlyList<Email> Emails =>
            Person is not null
                ? Person.Emails.ToList()
                : Business is not null
                    ? Business.Emails.ToList()
                    : new List<Email>();

        public Address Address =>
            Person is not null
                ? Person.Address
                : Business is not null
                    ? Business.Address
                    : null;

        public Person Contact => Business?.Contact;

        private Customer(Person person, CustomerType customerType, string code)
        {
            Person = person;
            CustomerType = customerType;
            Created = DateTime.Now;
            Code = code;
        }

        private Customer(Business business, CustomerType customerType, string code)
        {
            Business = business;
            CustomerType = customerType;
            Created = DateTime.Now;
            Code = code;
        }

        public static Result<Customer> Create(Person person, CustomerType customerType, string code)
        {
            if (person is null)
                return Result.Failure<Customer>(RequiredMessage);

            if (!Enum.IsDefined(typeof(CustomerType), customerType))
                return Result.Failure<Customer>(RequiredMessage);

            code = code?.Trim() ?? string.Empty;
            var codeResult = ValidateCode(code);
            if (codeResult.IsFailure)
                return Result.Failure<Customer>(codeResult.Error);

            return Result.Success(new Customer(person, customerType, code));
        }

        public static Result<Customer> Create(Business business, CustomerType customerType, string code)
        {
            if (business is null)
                return Result.Failure<Customer>(RequiredMessage);

            if (!Enum.IsDefined(typeof(CustomerType), customerType))
                return Result.Failure<Customer>(RequiredMessage);

            code = code?.Trim() ?? string.Empty;
            var codeResult = ValidateCode(code);
            if (codeResult.IsFailure)
                return Result.Failure<Customer>(codeResult.Error);

            return Result.Success(new Customer(business, customerType, code));
        }

        private EntityType GetEntityType() => Person is not null
                ? EntityType.Person
                : Business is not null
                    ? EntityType.Business
                    : throw new InvalidOperationException(UnknownEntityTypeMessage);

        public Result SetAddress(Address address)
        {
            // Address (if present) is guaranteed to be valid;
            // it was validated on creation.
            if (address is null)
                return Result.Failure<Address>(RequiredMessage);

            if (Person is not null)
                return Result.Success(Person.SetAddress(address));

            if (Business is not null)
                return Result.Success(Business.SetAddress(address));

            return Result.Failure<Address>(UnknownEntityTypeMessage);
        }

        public Result ClearAddress()
        {
            if (Person is not null)
                return Result.Success(Person.ClearAddress());

            if (Business is not null)
                return Result.Success(Business.ClearAddress());

            return Result.Failure<Address>(UnknownEntityTypeMessage);
        }

        public Result<Phone> AddPhone(Phone phone)
        {
            if (phone is null)
                return Result.Failure<Phone>(RequiredMessage);

            if (CustomerHasPhone(phone))
                return Result.Failure<Phone>($"{DuplicateItemMessagePrefix} Phone: {phone.PhoneType} - {phone}");

            switch (EntityType)
            {
                case EntityType.Person:
                    Person.AddPhone(phone);
                    return Result.Success(phone);

                case EntityType.Business:
                    Business.AddPhone(phone);
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

                case EntityType.Business:
                    Business.RemovePhone(phone);
                    return Result.Success(phone);

                default: return Result.Failure<Phone>(UnknownEntityTypeMessage);
            }
        }

        private bool CustomerHasPhone(Phone phone)
        {
            if (Person is not null)
                return Person.Phones.Any(x => x == phone);

            if (Business is not null)
                return Business.Phones.Any(x => x == phone);

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

                case EntityType.Business:
                    Business.AddEmail(email);
                    return Result.Success(email);

                default: return Result.Failure<Email>(UnknownEntityTypeMessage);
            }
        }

        private bool CustomerHasEmail(Email email)
        {
            if (Person is not null)
                return Person.Emails.Any(x => x == email);

            if (Business is not null)
                return Business.Emails.Any(x => x == email);

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

                case EntityType.Business:
                    Business.RemoveEmail(email);
                    return Result.Success(email);

                default: return Result.Failure<Email>(UnknownEntityTypeMessage);
            }
        }

        public Result<Vehicle> AddVehicle(Vehicle vehicle)
        {
            if (vehicle is null)
                return Result.Failure<Vehicle>(RequiredMessage);

            if (CustomerHasVehicle(vehicle))
                return Result.Failure<Vehicle>($"{DuplicateItemMessagePrefix} Vehicle: {vehicle}, VIN: {vehicle.VIN}");

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

        public Result<string> SetCode(string code)
        {
            code = code?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(code)) return Result.Success(Code = code);

            return code.Length <= MaximumCodeLength
                ? Result.Success(Code = code)
                : Result.Failure<string>(InvalidCodeLengthMessage);
        }

        private static Result ValidateCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code)) return Result.Success();

            return code.Length <= MaximumCodeLength
                ? Result.Success()
                : Result.Failure(InvalidCodeLengthMessage);
        }

        #region ORM

        // EF requires a parameterless constructor
        protected Customer()
        {
            vehicles = new List<Vehicle>();
        }

        #endregion

    }
}
