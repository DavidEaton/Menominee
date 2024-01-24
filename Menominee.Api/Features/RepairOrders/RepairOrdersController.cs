using CSharpFunctionalExtensions;
using Menominee.Api.Features.Contactables.Businesses;
using Menominee.Api.Features.Contactables.Persons;
using Menominee.Api.Features.Customers;
using Menominee.Api.Features.Employees;
using Menominee.Api.Features.Manufacturers;
using Menominee.Api.Features.ProductCodes;
using Menominee.Api.Features.SaleCodes;
using Menominee.Api.Features.Vehicles;
using Menominee.Domain.Entities;
using Menominee.Domain.Entities.RepairOrders;
using Menominee.Domain.Interfaces;
using Menominee.Shared.Models.Businesses;
using Menominee.Shared.Models.Customers;
using Menominee.Shared.Models.Http;
using Menominee.Shared.Models.Persons;
using Menominee.Shared.Models.ProductCodes;
using Menominee.Shared.Models.RepairOrders;
using Menominee.Shared.Models.RepairOrders.LineItems.Item;
using Menominee.Shared.Models.RepairOrders.Payments;
using Menominee.Shared.Models.RepairOrders.Services;
using Menominee.Shared.Models.RepairOrders.Statuses;
using Menominee.Shared.Models.RepairOrders.Taxes;
using Menominee.Shared.Models.Vehicles;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Menominee.Api.Features.RepairOrders
{
    public class RepairOrdersController : BaseApplicationController<RepairOrdersController>
    {
        private readonly IRepairOrderRepository repository;
        private readonly IProductCodeRepository productCodeRepository;
        private readonly ICustomerRepository customerRepository;
        private readonly IVehicleRepository vehicleRepository;
        private readonly ISaleCodeRepository saleCodeRepository;
        private readonly IManufacturerRepository manufacturersRepository;
        private readonly IEmployeeRepository employeeRepository;
        private readonly IBusinessRepository businessRepository;
        private readonly IPersonRepository personRepository;

        public RepairOrdersController(
            IRepairOrderRepository repository,
            IProductCodeRepository productCodeRepository,
            ICustomerRepository customerRepository,
            IVehicleRepository vehicleRepository,
            ISaleCodeRepository saleCodeRepository,
            IManufacturerRepository manufacturersRepository,
            IEmployeeRepository employeeRepository,
            IBusinessRepository businessRepository,
            IPersonRepository personRepository,
            ILogger<RepairOrdersController> logger) : base(logger)
        {
            this.repository = repository ??
                throw new ArgumentNullException(nameof(repository));

            this.productCodeRepository = productCodeRepository ??
                throw new ArgumentNullException(nameof(productCodeRepository));

            this.customerRepository = customerRepository ??
                throw new ArgumentNullException(nameof(customerRepository));

            this.vehicleRepository = vehicleRepository ??
                throw new ArgumentNullException(nameof(vehicleRepository));

            this.saleCodeRepository = saleCodeRepository ??
                throw new ArgumentNullException(nameof(saleCodeRepository));

            this.manufacturersRepository = manufacturersRepository ??
                throw new ArgumentNullException(nameof(manufacturersRepository));

            this.employeeRepository = employeeRepository ??
                throw new ArgumentNullException(nameof(employeeRepository));

            this.businessRepository = businessRepository ??
                throw new ArgumentNullException(nameof(businessRepository));

            this.personRepository = personRepository ??
                throw new ArgumentNullException(nameof(personRepository));
        }

        [HttpGet("listing")]
        public async Task<ActionResult<IReadOnlyList<RepairOrderToReadInList>>> GetListAsync()
        {
            var result = await repository.GetListAsync();

            return result is null
                ? NotFound()
                : Ok(result);
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<RepairOrderToRead>> GetAsync(long id)
        {
            var result = await repository.GetAsync(id);

            return result is null
                ? NotFound()
                : Ok(result);
        }

        [HttpPut("{id:long}")]
        public async Task<ActionResult> UpdateAsync(RepairOrderToWrite repairOrderFromCaller)
        {
            var repairOrderFromRepository = await repository.GetEntityAsync(repairOrderFromCaller.Id);

            if (repairOrderFromRepository is null)
                return NotFound($"Could not find Repair Order Id {repairOrderFromCaller.Id} to update.");

            var customer = await customerRepository.GetEntityAsync(repairOrderFromCaller.Customer.Id);
            if (customer is null)
                return NotFound($"Could not find Customer Id {repairOrderFromCaller.Customer.Id} to update.");

            var vehicle = await vehicleRepository.GetEntityAsync(repairOrderFromCaller.Vehicle.Id);
            if (vehicle is null)
                return NotFound($"Could not find Vehicle Id {repairOrderFromCaller.Vehicle.Id} to update.");

            var result = UpdateRepairOrder(repairOrderFromCaller, repairOrderFromRepository, customer, vehicle);

            // Using Result here is overkill; asp.net pipeline has already invoked FluentValidation validator(s)
            if (result.IsFailure)
                return BadRequest(result.Error);

            // await UpdateServicesAsync(repairOrderFromCaller, repairOrderFromRepository, manufacturerCodes);
            UpdatePayments(repairOrderFromCaller, repairOrderFromRepository);
            UpdateTaxes(repairOrderFromCaller, repairOrderFromRepository);
            UpdateStatuses(repairOrderFromCaller, repairOrderFromRepository);

            await repository.SaveChangesAsync();

            return NoContent();
        }

        private void UpdateStatuses(RepairOrderToWrite repairOrderFromCaller, RepairOrder repairOrderFromRepository)
        {
            foreach (var status in repairOrderFromCaller?.Statuses)
            {
                var editableStatus = repairOrderFromRepository.Statuses.FirstOrDefault(orderStatus => orderStatus.Id == status.Id);
                if (editableStatus is null)
                    continue;
                // TODO: Take advantage of Result returned from Set{PropertyName}() methods;
                editableStatus.SetStatus(status.Status);
                editableStatus.SetDescription(status.Description);
            }
        }

        private static void UpdateTaxes(RepairOrderToWrite repairOrderFromCaller, RepairOrder repairOrderFromRepository)
        {
            foreach (var tax in repairOrderFromCaller?.Taxes)
            {
                var editableTax = repairOrderFromRepository.Taxes.FirstOrDefault(oTax => oTax.Id == tax.Id);
                if (editableTax is null)
                    continue;

                // TODO: Take advantage of Result returned from Create() factory methods;
                editableTax.SetLaborTax(LaborTax.Create(tax.LaborTax.Rate, tax.LaborTax.Amount).Value);
                editableTax.SetPartTax(PartTax.Create(tax.PartTax.Rate, tax.PartTax.Amount).Value);
            }
        }

        private static void UpdatePayments(RepairOrderToWrite repairOrder, RepairOrder repairOrderFromRepository)
        {
            foreach (var payment in repairOrder?.Payments)
            {
                var editablePayment = repairOrderFromRepository.Payments.FirstOrDefault(x => x.Id == payment.Id);
                if (editablePayment is null)
                    continue;
                // TODO: Take advantage of Result returned from Set{PropertyName}() methods;
                editablePayment.SetAmount(payment.Amount);
                editablePayment.SetPaymentMethod(payment.PaymentMethod);
            }
        }

        private static async Task UpdateServicesAsync(RepairOrderToWrite repairOrder, RepairOrder repairOrderFromRepository, IReadOnlyList<ProductCodeToRead> productCodes)
        {
            foreach (var service in repairOrder?.Services)
            {
                //var editableService = repairOrderFromRepository.Services.Find(x => x.Id == service.Id);

                //if (editableService is null)
                //    continue;

                //editableService.DiscountTotal = service.DiscountTotal;
                //editableService.IsCounterSale = service.IsCounterSale;
                //editableService.IsDeclined = service.IsDeclined;
                //editableService.LaborTotal = service.LaborTotal;
                //editableService.PartsTotal = service.PartsTotal;
                //editableService.RepairOrderId = repairOrderFromRepository.Id;
                //editableService.SaleCode = service.SaleCode;
                //editableService.ServiceName = service.ServiceName;
                //editableService.ShopSuppliesTotal = service.ShopSuppliesTotal;
                //editableService.TaxTotal = service.TaxTotal;
                //editableService.Total = service.Total;

                //await UpdateServiceItemsAsync(service, editableService, productCodes);
                ////UpdateServiceTaxes(service);
                ////UpdateServiceTechnician(service);

                //// Services.Taxes = RepairOrderHelper.CreateServiceTaxes(service.Taxes),
                //// Services.Techs = RepairOrderHelper.CreateTechnicians(service.Techs)

            }
        }

        private static Task UpdateServiceItemsAsync(RepairOrderServiceToWrite service, RepairOrderService editableService, IReadOnlyList<ProductCodeToRead> productCodes)
        {
            //foreach (var item in service?.Items)
            //{
            //    var editableItem = editableService.Items.Find(x => x.Id == item.Id);

            //    if (editableItem is null)
            //        continue;

            //    editableItem.Core = item.Core;
            //    editableItem.Cost = item.Cost;
            //    editableItem.Description = item.Description;
            //    editableItem.DiscountEach = item.DiscountEach;
            //    editableItem.DiscountType = item.DiscountType;
            //    editableItem.IsCounterSale = item.IsCounterSale;
            //    editableItem.IsDeclined = item.IsDeclined;
            //    editableItem.LaborEach = item.LaborEach;
            //    editableItem.LaborType = item.LaborType;
            //    //editableItem.Manufacturer = item.Manufacturer;
            //    editableItem.ManufacturerId = item.ManufacturerId;
            //    editableItem.PartNumber = item.PartNumber;
            //    editableItem.PartType = item.PartType;
            //    editableItem.ProductCode = ProductCodeHelper.ConvertWriteDtoToEntity(item.ProductCode, productCodes);
            //    editableItem.ProductCodeId = item.ProductCodeId;
            //    editableItem.QuantitySold = item.QuantitySold;
            //    editableItem.RepairOrderServiceId = item.RepairOrderServiceId;
            //    editableItem.SaleCode = SaleCodeHelper.ConvertWriteDtoToEntity(item.SaleCode);
            //    editableItem.SaleCodeId = item.SaleCodeId;
            //    editableItem.SaleType = item.SaleType;
            //    editableItem.SellingPrice = item.SellingPrice;
            //    editableItem.Total = item.Total;

            //    UpdateServiceItemSerialNumbers(item, editableItem);
            //    UpdateServiceItemWarranties(item, editableItem);
            //    //editableItem.SerialNumbers = CreateSerialNumbers(item.SerialNumbers);
            //    //editableItem.Warranties = CreateWarranties(item.Warranties)
            //    //editableItem.Taxes = CreateItemTaxes(item.Taxes);
            //}

            //return Task.CompletedTask;
            throw new NotImplementedException();
        }

        private static void UpdateServiceItemWarranties(RepairOrderItemToWrite item, RepairOrderLineItem editableItem)
        {
            //List<RepairOrderWarranty> addedWarranties = new();

            //foreach (var warranty in item?.Warranties)
            //{
            //    RepairOrderWarranty editableWarranty
            //        = editableItem?.Warranties.Find(x => x.Id == warranty.Id);

            //    editableWarranty = editableWarranty ?? new();

            //    editableWarranty.RepairOrderItemId = warranty.RepairOrderItemId;
            //    editableWarranty.NewWarranty = warranty.NewWarranty;
            //    editableWarranty.OriginalInvoiceId = warranty.OriginalInvoiceId;
            //    editableWarranty.OriginalWarranty = warranty.OriginalWarranty;
            //    editableWarranty.Quantity = warranty.Quantity;
            //    editableWarranty.Type = warranty.Type;

            //    if (editableWarranty.Id == 0)
            //        addedWarranties.Add(editableWarranty);
            //}

            //if (addedWarranties.Count > 0)
            //    editableItem.Warranties.AddRange(addedWarranties);
        }

        private static void UpdateServiceItemSerialNumbers(RepairOrderItemToWrite item, RepairOrderLineItem editableItem)
        {
            //List<RepairOrderSerialNumber> addedSerialNumbers = new();

            //foreach (var serialNumber in item?.SerialNumbers)
            //{
            //    var editableSerialNumber = editableItem?.SerialNumbers.Find(x => x.Id == serialNumber.Id);

            //    editableSerialNumber = editableSerialNumber ?? new();

            //    editableSerialNumber.RepairOrderItemId = serialNumber.RepairOrderItemId;
            //    editableSerialNumber.SerialNumber = serialNumber.SerialNumber;

            //    if (editableSerialNumber.Id == 0)
            //        // This unfound serial number was added in BuildSerialNumberList, disconnected
            //        // from the db context, so we must add it to the tracked collection here
            //        addedSerialNumbers.Add(editableSerialNumber);
            //}

            //if (addedSerialNumbers.Count > 0)
            //    editableItem.SerialNumbers.AddRange(addedSerialNumbers);
        }

        private static Result UpdateRepairOrder(
            RepairOrderToWrite repairOrderFromCaller,
            RepairOrder repairOrderFromRepository,
            Customer customer,
            Vehicle vehicle)
        {
            return Result.Combine(
                customer is not null && customer != repairOrderFromRepository.Customer
                    ? repairOrderFromRepository.SetCustomer(customer)
                    : Result.Success(),
                repairOrderFromCaller.InvoiceNumber != repairOrderFromRepository.InvoiceNumber
                    ? repairOrderFromRepository.SetInvoiceNumber(repairOrderFromCaller.InvoiceNumber)
                    : Result.Success(),
                vehicle is not null && vehicle != repairOrderFromRepository.Vehicle
                    ? repairOrderFromRepository.SetVehicle(vehicle)
                    : Result.Success(),
                repairOrderFromCaller.AccountingDate != repairOrderFromRepository.AccountingDate
                    ? repairOrderFromRepository.SetAccountingDate(repairOrderFromCaller.AccountingDate)
                    : Result.Success(),
                repairOrderFromRepository.RepairOrderNumber != repairOrderFromCaller.RepairOrderNumber
                    ? repairOrderFromRepository.SetRepairOrderNumber(repairOrderFromCaller.RepairOrderNumber, DateTime.Today)
                    : Result.Success()
            );
        }

        [HttpPost]
        public async Task<ActionResult<PostResponse>> AddAsync(RepairOrderToWrite repairOrderToAdd)
        {
            var customer = await HandleCustomerAsync(repairOrderToAdd?.Customer);
            var vehicle = await HandleVehicleAsync(repairOrderToAdd?.Vehicle);
            // var saleCodeIds = repairOrderToAdd.Services.Select(service => service.SaleCode.Id).ToList();
            // var saleCodes = await saleCodeRepository.GetSaleCodeEntitiesAsync(saleCodeIds, all: true);
            var lastInvoiceNumberOrSeed = repository.GetLastInvoiceNumberOrSeed();


            var repairOrder = RepairOrder.Create(
                customer: customer,
                vehicle: vehicle,
                accountingDate: repairOrderToAdd.AccountingDate,
                repairOrderNumbers: await repository.GetTodaysRepairOrderNumbersAsync(),
                lastInvoiceNumber: lastInvoiceNumberOrSeed,
                statuses: StatusHelper.ConvertWriteDtosToEntities(repairOrderToAdd.Statuses),
                taxes: RepairOrderTaxHelper.ConvertWriteDtosToEntities(repairOrderToAdd.Taxes),
                payments: PaymentHelper.ConvertWriteDtosToEntities(repairOrderToAdd.Payments)
            ).Value;

            repository.Add(repairOrder);
            await repository.SaveChangesAsync();

            return Created(new Uri($"api/RepairOrders/{repairOrder.Id}", UriKind.Relative), new { repairOrder.Id });
        }

        private async Task<Customer> HandleCustomerAsync(CustomerToWrite customer)
        {
            if (customer is null)
            {
                return null;
            }

            if (customer.Id is not 0)
            {
                return await customerRepository.GetEntityAsync(customer.Id);
            }

            ICustomerEntity? customerEntity = null;

            if (customer.IsBusiness)
            {
                customerEntity = await HandleBusinessAsync(customer);
            }
            else if (customer.IsPerson)
            {
                customerEntity = await HandlePersonAsync(customer);
            }

            if (customerEntity is null)
            {
                throw new InvalidOperationException("No valid customer entity was created.");
            }

            var createdCustomer = Customer.Create(customerEntity, customer.CustomerType, customer.Code).Value;

            customerRepository.Add(createdCustomer);
            await customerRepository.SaveChangesAsync();

            return createdCustomer;
        }

        private async Task<Vehicle> HandleVehicleAsync(VehicleToWrite vehicleToWrite)
        {
            if (vehicleToWrite is null)
            {
                return null;
            }

            if (vehicleToWrite.Id is not 0)
            {
                return await vehicleRepository.GetEntityAsync(vehicleToWrite.Id);
            }
            else
            {
                var newVehicle = Vehicle
                    .Create(
                        vehicleToWrite.VIN,
                        vehicleToWrite.Year,
                        vehicleToWrite.Make,
                        vehicleToWrite.Model,
                        vehicleToWrite.Plate,
                        vehicleToWrite.PlateStateProvince,
                        vehicleToWrite.UnitNumber,
                        vehicleToWrite.Color,
                        vehicleToWrite.Active,
                        vehicleToWrite.NonTraditionalVehicle).Value;

                vehicleRepository.Add(newVehicle);
                await vehicleRepository.SaveChangesAsync();
                return newVehicle;
            }
        }


        private async Task<Business?> HandleBusinessAsync(CustomerToWrite customerToWrite)
        {
            var business = BusinessHelper.ConvertWriteDtoToEntity(customerToWrite.Business);

            if (customerToWrite.Business.Id is 0)
            {
                businessRepository.Add(business);
                await businessRepository.SaveChangesAsync();
            }

            return business;
        }

        private async Task<Person?> HandlePersonAsync(CustomerToWrite customerToWrite)
        {
            var person = PersonHelper.ConvertWriteDtoToEntity(customerToWrite.Person);

            if (customerToWrite.Person.Id is 0)
            {
                personRepository.Add(person);
                await personRepository.SaveChangesAsync();
            }

            return person;
        }


        [HttpDelete("{id:long}")]
        public async Task<ActionResult> DeleteAsync(long id)
        {
            var repairOrder = await repository.GetEntityAsync(id);
            if (repairOrder is null)
            {
                return NotFound($"Could not find Repair Order in the database to delete with id of {id}.");
            }

            repository.Delete(repairOrder);
            await repository.SaveChangesAsync();

            return NoContent();
        }
    }
}
