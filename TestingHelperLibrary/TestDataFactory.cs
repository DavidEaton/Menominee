using Menominee.Domain.Entities;
using Menominee.Domain.Enums;
using Menominee.Shared.Models.Addresses;
using Menominee.Shared.Models.Businesses;
using Menominee.Shared.Models.Contactable;
using Menominee.Shared.Models.Customers;
using Menominee.Shared.Models.Persons;
using Menominee.Shared.Models.Persons.DriversLicenses;
using Menominee.Shared.Models.Vehicles;

namespace Menominee.TestingHelperLibrary
{
    public static class TestDataFactory
    {
        public static BusinessToWrite CreateBusinessRequest(bool invalid = false)
        {
            return invalid
                ? new BusinessToWrite { }
                : new BusinessToWrite
                {
                    Address = new AddressToWrite
                    {
                        AddressLine1 = "123 Main St",
                        AddressLine2 = "Apt 1",
                        City = "Anytown",
                        State = State.AK,
                        PostalCode = "12345"
                    },
                    Contact = CreatePersonRequest(),
                    Notes = "Some notes",
                    Name = new BusinessNameRequest { Name = "Business Name" },
                    Phones = new List<PhoneToWrite>
                    {
                        new PhoneToWrite
                        {
                            Number = "123-456-7890",
                            IsPrimary = true
                        }
                    },
                    Emails = new List<EmailToWrite>
                    {
                        new EmailToWrite
                        {
                            Address = "email@email.com",
                            IsPrimary = true
                        }
                    }
                };
        }

        public static PersonToWrite CreatePersonRequest(bool invalid = false)
        {
            return invalid
                ? new PersonToWrite { }
                : new PersonToWrite
                {
                    Address = new AddressToWrite
                    {
                        AddressLine1 = "123 Main St",
                        AddressLine2 = "Apt 1",
                        City = "Anytown",
                        State = State.AK,
                        PostalCode = "12345"
                    },
                    Birthday = DateTime.Today.AddYears(-40),
                    DriversLicense = new DriversLicenseToWrite
                    {
                        Number = "123456789",
                        State = State.AK,
                        Issued = DateTime.Today.AddYears(-2),
                        Expiry = DateTime.Today.AddYears(2),
                    },
                    Emails = new List<EmailToWrite>
                    {
                        new EmailToWrite
                        {
                            Address = "email@email.com",
                            IsPrimary = true
                        }
                    },
                    Name = new PersonNameToWrite
                    {
                        FirstName = "John",
                        LastName = "Doe"
                    },
                    Notes = "Some notes",
                    Phones = new List<PhoneToWrite>
                    {
                        new PhoneToWrite
                        {
                            Number = "123-456-7890",
                            IsPrimary = true
                        }
                    }
                };
        }

        public static CustomerToWrite CreateCustomerRequest(EntityType entityType, bool validDetails = true, CustomerType customerType = CustomerType.Retail)
        {
            var customer = new CustomerToWrite
            {
                EntityType = entityType,
                CustomerType = customerType,
                Code = validDetails ? "12345" : new string('A', Customer.MaximumCodeLength + 1) // Invalid code if details are not valid
            };

            switch (entityType)
            {
                case EntityType.Person:
                    customer.Person = validDetails ? CreatePersonRequest() : CreatePersonRequest(invalid: true);
                    break;
                case EntityType.Business:
                    customer.Business = validDetails ? CreateBusinessRequest() : CreateBusinessRequest(invalid: true);
                    break;
            }

            customer.Vehicles = validDetails ? CreateVehiclesList() : CreateVehiclesList(invalid: true);

            return customer;
        }

        public static List<VehicleToWrite> CreateVehiclesList(bool invalid = false)
        {
            return invalid
                ? new List<VehicleToWrite>
                {
                    new VehicleToWrite
                    {
                        // Invalid vehice details
                    }
                }
                : new List<VehicleToWrite>
                {
                    new VehicleToWrite
                    {
                        Make = "Ford",
                        Model = "F150",
                        Year = 2010,
                        VIN = "12345678901234567",
                        Plate = "ABC123",
                        PlateStateProvince = State.AK,
                        UnitNumber = "123",
                        Color = "Red",
                        Active = true,
                        NonTraditionalVehicle = false
                    }
                };
        }
    }
}
