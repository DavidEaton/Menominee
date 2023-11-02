using CSharpFunctionalExtensions;
using Menominee.Api.Businesses;
using Menominee.Api.Common;
using Menominee.Api.Persons;
using Menominee.Common.Enums;
using Menominee.Common.Http;
using Menominee.Common.ValueObjects;
using Menominee.Domain.Entities;
using Menominee.Shared.Models.Addresses;
using Menominee.Shared.Models.Businesses;
using Menominee.Shared.Models.Contactable;
using Menominee.Shared.Models.Customers;
using Menominee.Shared.Models.DriversLicenses;
using Menominee.Shared.Models.Pagination;
using Menominee.Shared.Models.Persons;
using Menominee.Shared.Models.Vehicles;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Menominee.Api.Customers
{
    public class CustomersController : BaseApplicationController<CustomersController>
    {
        private readonly ICustomerRepository customerRepository;
        private readonly IPersonRepository personRepository;
        private readonly IBusinessRepository businessRepository;

        public CustomersController(
            ICustomerRepository customerRepository,
            IPersonRepository personRepository,
            IBusinessRepository businessRepository,
            ILogger<CustomersController> logger) : base(logger)
        {
            this.customerRepository = customerRepository ??
                throw new ArgumentNullException(nameof(customerRepository));
            this.personRepository = personRepository ??
                throw new ArgumentNullException(nameof(personRepository));
            this.businessRepository = businessRepository ??
                throw new ArgumentNullException(nameof(businessRepository));
        }

        [HttpGet("list")]
        public async Task<ActionResult<IReadOnlyList<CustomerToReadInList>>> GetListAsync()
        {
            var result = await customerRepository.GetListAsync();

            return result is null
                ? NotFound()
                : Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<CustomerToRead>>> GetAsync()
        {
            var result = await customerRepository.GetAllAsync();

            return result is null
                ? NotFound()
                : Ok(result);
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<CustomerToRead>> GetAsync(long id)
        {
            var customer = await GetCustomer(id);

            return customer.HasNoValue
                ? NotFound()
                : Ok(customer.GetValueOrThrow());
        }

        private async Task<Maybe<CustomerToRead>> GetCustomer(long id)
        {
            if (id == 0)
                return Maybe<CustomerToRead>.None;

            var customer = await customerRepository.GetAsync(id);

            return customer is null
                ? Maybe<CustomerToRead>.None
                : Maybe<CustomerToRead>.From(customer);
        }

        [HttpGet("{code}")]
        public async Task<ActionResult<PagedList<CustomerToRead>>> GetAsync(string code, [FromQuery] Pagination pagination)
        {
            if (string.IsNullOrEmpty(code))
                return BadRequest("Code parameter is required.");

            ValidatePagination(ref pagination);

            var result = await customerRepository.GetByCodeAsync(code, pagination);

            return result is null
                ? NotFound()
                : Ok(result);
        }

        private static void ValidatePagination(ref Pagination pagination)
        {
            pagination.PageNumber = Math.Max(1, pagination.PageNumber);
            pagination.PageSize = Math.Min(100, Math.Max(1, pagination.PageSize));
        }

        [HttpPut("{id:long}")]
        public async Task<ActionResult> UpdateAsync(long id, CustomerToWrite customerToWrite)
        {
            var customerFromRepository = await customerRepository.GetEntityAsync(id);

            if (customerFromRepository is null)
            {
                return NotFound($"Could not find Customer in the database to update with Id: {id}.");
            }

            UpdateCustomerEntity(customerToWrite, customerFromRepository);

            var result = UpdateCustomerType(customerToWrite, customerFromRepository);
            var badRequestResult = IsFailure(result);
            if (badRequestResult is not null)
            {
                return badRequestResult;
            }

            result = UpdateCustomerCode(customerToWrite, customerFromRepository);
            badRequestResult = IsFailure(result);
            if (badRequestResult is not null)
            {
                return badRequestResult;
            }

            AddNewVehicles(customerFromRepository, customerToWrite);
            UpdateExistingVehicles(customerFromRepository, customerToWrite);
            DeleteVehicles(customerFromRepository, customerToWrite);

            await customerRepository.SaveChangesAsync();

            return NoContent();
        }

        private ActionResult IsFailure(Result result)
        {
            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return null;
        }

        private static void UpdateCustomerEntity(CustomerToWrite customerToWrite, Customer? customerFromRepository)
        {
            // TODO: Retrun success or failure result
            switch (customerFromRepository?.CustomerEntity)
            {
                case Person person:
                    ContactDetailUpdaters.UpdatePerson(customerToWrite.Person, person);
                    break;
                case Business business:
                    ContactDetailUpdaters.UpdateBusiness(customerToWrite.Business, business);
                    break;

                default:
                    //return NotFound($"Could not find Customer in the database to update.");
                    break;
            }
        }

        private static Result UpdateCustomerType(CustomerToWrite customerToWrite, Customer? customerFromRepository)
        {
            if (customerFromRepository?.CustomerType != customerToWrite.CustomerType)
            {
                return customerFromRepository.SetCustomerType(customerToWrite.CustomerType);
            }

            return Result.Success();
        }

        private static Result UpdateCustomerCode(CustomerToWrite customerToWrite, Customer? customerFromRepository)
        {
            if (customerFromRepository?.Code != customerToWrite.Code)
            {
                return customerFromRepository.SetCode(customerToWrite.Code);
            }

            return Result.Success();
        }

        private static void DeleteVehicles(Customer customerFromRepository, CustomerToWrite customerFromCaller)
        {
            if (customerFromRepository.Vehicles is not null)
            {
                var vehiclesToDelete = customerFromRepository.Vehicles
                    .Where(vehicle => !customerFromCaller.Vehicles.Any(vehicleFromCaller => vehicleFromCaller.Id == vehicle.Id))
                    .ToList();

                foreach (var vehicleToDelete in vehiclesToDelete)
                {
                    customerFromRepository.RemoveVehicle(vehicleToDelete);
                }
            }
        }

        private static void UpdateExistingVehicles(Customer customerFromRepository, CustomerToWrite customerFromCaller)
        {
            if (customerFromRepository.Vehicles is not null)
            {
                foreach (var vehicle in customerFromRepository.Vehicles)
                {
                    var updatedVehicle = customerFromCaller.Vehicles
                        .FirstOrDefault(vehicleFromCaller => vehicleFromCaller.Id == vehicle.Id);

                    if (updatedVehicle is not null)
                    {
                        // TODO: return success or failure result
                        if (vehicle.NonTraditionalVehicle != updatedVehicle.NonTraditionalVehicle)
                            vehicle.SetNonTraditionalVehicle(updatedVehicle.NonTraditionalVehicle);

                        if (vehicle.Active != updatedVehicle.Active)
                            vehicle.SetActive(updatedVehicle.Active);

                        if (vehicle.Color != updatedVehicle.Color)
                            vehicle.SetColor(updatedVehicle.Color);

                        if (vehicle.UnitNumber != updatedVehicle.UnitNumber)
                            vehicle.SetUnitNumber(updatedVehicle.UnitNumber);

                        if (vehicle.Make != updatedVehicle.Make)
                            vehicle.SetMake(updatedVehicle.Make);

                        if (vehicle.Model != updatedVehicle.Model)
                            vehicle.SetModel(updatedVehicle.Model);

                        if (vehicle.Plate != updatedVehicle.Plate)
                            vehicle.SetPlate(updatedVehicle.Plate);

                        if (vehicle.PlateStateProvince != updatedVehicle.PlateStateProvince)
                            vehicle.SetPlateStateProvince(updatedVehicle.PlateStateProvince);

                        if (vehicle.VIN != updatedVehicle.VIN)
                            vehicle.SetVin(updatedVehicle.VIN);

                        if (vehicle.Year != updatedVehicle.Year)
                            vehicle.SetYear(updatedVehicle.Year);
                    }
                }
            }
        }

        private static void AddNewVehicles(Customer customerFromRepository, CustomerToWrite customerFromCaller)
        {
            var newVehicles = customerFromCaller.Vehicles?.Where(vehicle => vehicle.Id == 0) ?? Enumerable.Empty<VehicleToWrite>();

            foreach (var vehicleFromCaller in newVehicles)
            {
                customerFromRepository.AddVehicle(Vehicle.Create(
                    vehicleFromCaller.VIN,
                    vehicleFromCaller.Year,
                    vehicleFromCaller.Make,
                    vehicleFromCaller.Model,
                    vehicleFromCaller.Plate,
                    vehicleFromCaller.PlateStateProvince,
                    vehicleFromCaller.UnitNumber,
                    vehicleFromCaller.Color,
                    vehicleFromCaller.Active,
                    vehicleFromCaller.NonTraditionalVehicle)
                    .Value);
            }
        }

        [HttpPost]
        public async Task<ActionResult<PostResponse>> AddAsync(CustomerToWrite customerToAdd)
        {
            // All requests are routed thru dto validators using FluentValidation
            // in ASP.NET request pipeline, so we can assume that the request is valid
            // no need to validate request here again, just call .Value right away
            var customer = await CreateCustomerAsync(customerToAdd);
            AddVehiclesToCustomer(customer, customerToAdd.Vehicles.ToList());

            customerRepository.Add(customer);
            await customerRepository.SaveChangesAsync();

            return Created(
              new Uri($"api/Customers/{customer.Id}",
              UriKind.Relative),
              new { customer.Id });
        }

        private async Task<Customer> CreateCustomerAsync(CustomerToWrite customerToAdd)
        {
            return customerToAdd.EntityType switch
            {
                EntityType.Person => await CreatePersonCustomerAsync(customerToAdd),
                EntityType.Business => await CreateBusinessCustomerAsync(customerToAdd),
                _ => throw new InvalidOperationException("Invalid entity type"),
            };
        }

        private async Task<Customer> CreatePersonCustomerAsync(CustomerToWrite customerToAdd)
        {
            var person = customerToAdd.Person.Id > 0
                ? await personRepository.GetEntityAsync(customerToAdd.Person.Id)
                : await SaveNewPersonAsync(customerToAdd.Person);

            return CreateCustomer(person, customerToAdd.CustomerType, customerToAdd.Code);
        }

        private async Task<Person> SaveNewPersonAsync(PersonToWrite personToWrite)
        {
            var person = CreateNewPerson(personToWrite);
            personRepository.Add(person);
            await personRepository.SaveChangesAsync();
            return person;
        }

        private async Task<Customer> CreateBusinessCustomerAsync(CustomerToWrite customerToAdd)
        {
            var business = customerToAdd.Business.Id > 0
                ? await businessRepository.GetEntityAsync(customerToAdd.Business.Id)
                : await SaveNewBusinessAsync(customerToAdd.Business);

            return CreateCustomer(business, customerToAdd.CustomerType, customerToAdd.Code);
        }

        private async Task<Business> SaveNewBusinessAsync(BusinessToWrite businessToWrite)
        {
            var business = CreateNewBusiness(businessToWrite);
            businessRepository.Add(business);
            await businessRepository.SaveChangesAsync();
            return business;
        }

        private static Person CreateNewPerson(PersonToWrite personToAdd)
        {
            var newPerson = Person.Create(
                PersonName.Create(personToAdd.Name.LastName, personToAdd.Name.FirstName, personToAdd.Name.MiddleName).Value,
                personToAdd.Gender,
                personToAdd.Notes,
                personToAdd.Birthday).Value;

            SetDriversLicense(personToAdd.DriversLicense, newPerson);
            SetAddress(personToAdd.Address, newPerson);

            personToAdd.Phones
                .Select(phone => Phone.Create(phone.Number, phone.PhoneType, phone.IsPrimary).Value)
                .ToList()
                .ForEach(newPhone => newPerson.AddPhone(newPhone));

            personToAdd.Emails
                .Select(email => Email.Create(email.Address, email.IsPrimary).Value)
                .ToList()
                .ForEach(newEmail => newPerson.AddEmail(newEmail));

            return newPerson;
        }
        private static void SetAddress(AddressToWrite addressToAdd, Person newPerson)
        {
            if (addressToAdd is not null)
            {
                var address = Address.Create(
                    addressToAdd.AddressLine1,
                    addressToAdd.City,
                    addressToAdd.State,
                    addressToAdd.PostalCode,
                    addressToAdd.AddressLine2
                ).Value;

                newPerson.SetAddress(address);
            }
        }

        public static void SetDriversLicense(DriversLicenseToWrite driversLicenseToAdd, Person newPerson)
        {
            if (driversLicenseToAdd is not null)
            {
                var range = DateTimeRange.Create(driversLicenseToAdd.Issued, driversLicenseToAdd.Expiry).Value;

                var driversLicense = DriversLicense.Create(
                    driversLicenseToAdd.Number,
                    driversLicenseToAdd.State,
                    range
                ).Value;

                newPerson.SetDriversLicense(driversLicense);
            }
        }

        private static Business CreateNewBusiness(BusinessToWrite businessToAdd)
        {
            return Business.Create(
                BusinessName.Create(businessToAdd.Name).Value,
                businessToAdd.Notes,
                null,
                CreateAddress(businessToAdd),
                CreateEmails(businessToAdd.Emails),
                CreatePhones(businessToAdd.Phones)
                ).Value;
        }

        private static Address CreateAddress(BusinessToWrite businessToAdd)
        {
            return Address.Create(businessToAdd.Address.AddressLine1, businessToAdd.Address.City, businessToAdd.Address.State, businessToAdd.Address.PostalCode, businessToAdd.Address.AddressLine2).Value;
        }

        private static IReadOnlyList<Email> CreateEmails(List<EmailToWrite> emails)
        {
            return emails.Select(email => Email.Create(email.Address, email.IsPrimary).Value).ToList();
        }

        private static IReadOnlyList<Phone> CreatePhones(List<PhoneToWrite> phones)
        {
            return phones.Select(phone => Phone.Create(phone.Number, phone.PhoneType, phone.IsPrimary).Value).ToList();
        }

        private Customer CreateCustomer(Person entity, CustomerType customerType, string code)
        {
            return Customer.Create(entity, customerType, code).Value;
        }

        private Customer CreateCustomer(Business entity, CustomerType customerType, string code)
        {
            return Customer.Create(entity, customerType, code).Value;
        }

        private static void AddVehiclesToCustomer(Customer customer, List<VehicleToWrite> vehicles)
        {
            if (vehicles is not null && vehicles.Any())
            {
                foreach (var vehicle in vehicles
                    .Where(vehicle => vehicle.Id > 0))
                {
                    var vehicleToAdd = Vehicle
                        .Create(
                            vehicle.VIN,
                            vehicle.Year,
                            vehicle.Make,
                            vehicle.Model,
                            vehicle.Plate,
                            vehicle.PlateStateProvince,
                            vehicle.UnitNumber,
                            vehicle.Color,
                            vehicle.Active,
                            vehicle.NonTraditionalVehicle).Value;

                    customer.AddVehicle(vehicleToAdd);
                }
            }
        }

        [HttpDelete("{id:long}")]
        public async Task<ActionResult> DeleteAsync(long id)
        {
            var customerFromRepository = await customerRepository.GetEntityAsync(id);

            if (customerFromRepository is null)
                return NotFound($"Could not find Customer in the database to delete with Id: {id}.");

            customerRepository.Delete(customerFromRepository);
            await customerRepository.SaveChangesAsync();

            return NoContent();
        }
    }
}