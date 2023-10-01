using CSharpFunctionalExtensions;
using Menominee.Api.Businesses;
using Menominee.Api.Common;
using Menominee.Api.Persons;
using Menominee.Api.Vehicles;
using Menominee.Common.Enums;
using Menominee.Common.Http;
using Menominee.Domain.Entities;
using Menominee.Shared.Models.Businesses;
using Menominee.Shared.Models.Customers;
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
        private readonly IVehicleRepository vehicleRepository;
        private readonly PersonsController personsController;
        private readonly BusinessesController businessesController;

        public CustomersController(
            ICustomerRepository customerRepository,
            IPersonRepository personRepository,
            IBusinessRepository businessRepository,
            IVehicleRepository vehicleRepository,
            PersonsController personsController,
            BusinessesController businessesController,
            ILogger<CustomersController> logger) : base(logger)
        {
            this.customerRepository = customerRepository ??
                throw new ArgumentNullException(nameof(customerRepository));
            this.personRepository = personRepository ??
                throw new ArgumentNullException(nameof(personRepository));
            this.businessRepository = businessRepository ??
                throw new ArgumentNullException(nameof(businessRepository));
            this.vehicleRepository = vehicleRepository ??
                throw new ArgumentNullException(nameof(vehicleRepository));
            this.personsController = personsController ??
                throw new ArgumentNullException(nameof(personsController));
            this.businessesController = businessesController ??
                throw new ArgumentNullException(nameof(businessesController));
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

            if (customerFromRepository == null || customerFromRepository?.EntityType == null)
                return NotFound($"Could not find Customer in the database to update.");

            if (customerFromRepository.EntityType == EntityType.Business)
            {
                //await businessesController.UpdateAsync(customerToWrite.Business);
                var businessFromRepository = customerFromRepository.Business;
            }

            if (customerFromRepository.EntityType == EntityType.Person)
                await personsController.UpdateAsync(customerToWrite.Person);

            AddNewVehicles(customerFromRepository, customerToWrite);
            UpdateExistingVehicles(customerFromRepository, customerToWrite);
            DeleteVehicles(customerFromRepository, customerToWrite);

            await customerRepository.SaveChangesAsync();

            return NoContent();
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
                        if (vehicle.Active != updatedVehicle.Active)
                            vehicle.SetActive(updatedVehicle.Active);

                        if (vehicle.Color != updatedVehicle.Color)
                            vehicle.SetColor(updatedVehicle.Color);

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
            var customer = CreateCustomer(customerToAdd);
            AddVehiclesToCustomer(customer, customerToAdd.Vehicles.ToList());

            customerRepository.Add(customer);
            await customerRepository.SaveChangesAsync();

            return Created(
              new Uri($"api/Customers/{customer.Id}",
              UriKind.Relative),
              new { customer.Id });
        }

        private Customer CreateCustomer(CustomerToWrite customerToAdd) =>
            customerToAdd.EntityType switch
            {
                EntityType.Person => Customer.Create(
                    PersonHelper.ConvertWriteDtoToEntity(customerToAdd.Person),
                    customerToAdd.CustomerType,
                    customerToAdd.Code
                ).Value,

                EntityType.Business => Customer.Create(
                    BusinessHelper.ConvertWriteDtoToEntity(customerToAdd.Business),
                    customerToAdd.CustomerType,
                    customerToAdd.Code
                ).Value,

                _ => throw new InvalidOperationException("Invalid entity type")
            };

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