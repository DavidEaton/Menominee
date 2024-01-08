using CSharpFunctionalExtensions;
using Menominee.Domain.Enums;
using Menominee.Domain.Interfaces;
using Menominee.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using Entity = Menominee.Domain.BaseClasses.Entity;

namespace Menominee.Domain.Entities
{
    public class Customer : Entity
    {
        public static readonly int MaximumCodeLength = 20;
        public static readonly string DuplicateItemMessagePrefix = $"Customer already has this ";
        public static readonly string UnknownEntityTypeMessage = $"Unknown entity type.";
        public static readonly string UnknownCustomerTypeMessage = $"Unknown type.";
        public static readonly string RequiredMessage = "Please include all required items.";
        public static readonly string InvalidCodeLengthMessage = $"Code must be {MaximumCodeLength} characters or less.";
        public static readonly string UnsupportedMessage = "Unsupported customer entity type.";

        public CustomerType CustomerType { get; private set; }
        public string Code { get; private set; } //optional
        public ContactPreferences ContactPreferences { get; private set; }
        public ICustomerEntity CustomerEntity { get; private set; }
        public EntityType EntityType => CustomerEntity.EntityType;
        public string DisplayName => CustomerEntity.DisplayName;
        public string Notes => CustomerEntity.Notes;
        public Address Address => CustomerEntity.Address;
        public string Name => CustomerEntity.ToString();
        private readonly List<Vehicle> vehicles = new();
        public IReadOnlyList<Vehicle> Vehicles => vehicles.ToList();
        public IReadOnlyList<Phone> Phones => CustomerEntity.Phones;
        public IReadOnlyList<Email> Emails => CustomerEntity.Emails;

        private Customer(ICustomerEntity entity, CustomerType customerType, string code)
        {
            CustomerEntity = entity;
            CustomerType = customerType;
            Code = code;
        }

        public static Result<Customer> Create(ICustomerEntity entity, CustomerType customerType, string code)
        {
            if (entity is null)
                return Result.Failure<Customer>(RequiredMessage);

            if (!Enum.IsDefined(typeof(CustomerType), customerType))
                return Result.Failure<Customer>(UnknownCustomerTypeMessage);

            code = code?.Trim() ?? string.Empty;

            if (code.Length > MaximumCodeLength)
                return Result.Failure<Customer>(InvalidCodeLengthMessage);

            return Result.Success(new Customer(entity, customerType, code));
        }

        public Result SetAddress(Address address)
        {
            switch (CustomerEntity)
            {
                case Person person:
                    return person.SetAddress(address);

                case Business business:
                    return business.SetAddress(address);

                default:
                    return Result.Failure(UnsupportedMessage);
            }
        }

        public void ClearAddress()
        {
            switch (CustomerEntity)
            {
                case Person person:
                    person.ClearAddress();
                    break;

                case Business business:
                    business.ClearAddress();
                    break;

                default:
                    throw new InvalidOperationException(UnsupportedMessage);
            }
        }

        public Result SetCustomerType(CustomerType customerType)
        {
            if (Enum.IsDefined(typeof(CustomerType), customerType))
            {
                CustomerType = customerType;
                return Result.Success();
            }

            return Result.Failure(RequiredMessage);
        }

        public Result<Phone> AddPhone(Phone phone)
        {
            switch (CustomerEntity)
            {
                case Person person:
                    return person.AddPhone(phone);

                case Business business:
                    return business.AddPhone(phone);

                default:
                    return Result.Failure<Phone>(UnsupportedMessage);
            }
        }

        public Result<Phone> RemovePhone(Phone phone)
        {
            switch (CustomerEntity)
            {
                case Person person:
                    return person.RemovePhone(phone);

                case Business business:
                    return business.RemovePhone(phone);

                default:
                    return Result.Failure<Phone>(UnsupportedMessage);
            }
        }

        public Result<Email> AddEmail(Email email)
        {
            switch (CustomerEntity)
            {
                case Person person:
                    return person.AddEmail(email);

                case Business business:
                    return business.AddEmail(email);

                default:
                    return Result.Failure<Email>(UnsupportedMessage);
            }
        }

        public Result<Email> RemoveEmail(Email email)
        {
            switch (CustomerEntity)
            {
                case Person person:
                    return person.RemoveEmail(email);

                case Business business:
                    return business.RemoveEmail(email);

                default:
                    return Result.Failure<Email>(UnsupportedMessage);
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
            return Vehicles.Any(existingVehicle => existingVehicle == vehicle);
        }

        public Result<string> SetCode(string code)
        {
            code = code?.Trim() ?? string.Empty;

            return code.Length <= MaximumCodeLength
                ? Result.Success(Code = code)
                : Result.Failure<string>(InvalidCodeLengthMessage);
        }

        public Result SetCustomerEntity(ICustomerEntity entity)
        {
            if (entity is null)
                return Result.Failure<ICustomerEntity>(RequiredMessage);

            switch (entity.EntityType)
            {
                case EntityType.Person:
                    CustomerEntity = entity;
                    break;

                case EntityType.Business:
                    CustomerEntity = entity;
                    break;

                default:
                    return Result.Failure(UnsupportedMessage);
            }

            return Result.Success();
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