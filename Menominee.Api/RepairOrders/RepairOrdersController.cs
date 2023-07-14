﻿using CSharpFunctionalExtensions;
using Menominee.Api.Customers;
using Menominee.Api.ProductCodes;
using Menominee.Api.SaleCodes;
using Menominee.Api.Vehicles;
using Menominee.Domain.Entities;
using Menominee.Domain.Entities.RepairOrders;
using Menominee.Shared.Models.ProductCodes;
using Menominee.Shared.Models.RepairOrders;
using Menominee.Shared.Models.RepairOrders.LineItems.Item;
using Menominee.Shared.Models.RepairOrders.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Menominee.Api.RepairOrders
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class RepairOrdersController : ControllerBase
    {
        private readonly IRepairOrderRepository repository;
        private readonly IProductCodeRepository productCodeRepository;
        private readonly ICustomerRepository customerRepository;
        private readonly IVehicleRepository vehicleRepository;
        private readonly ISaleCodeRepository saleCodeRepository;
        private readonly string BasePath = "/api/repairorders";

        public RepairOrdersController(
            IRepairOrderRepository repository,
            IProductCodeRepository productCodesRepository,
            ICustomerRepository customerRepository,
            IVehicleRepository vehicleRepository,
            ISaleCodeRepository saleCodeRepository)
        {
            this.repository = repository ??
                throw new ArgumentNullException(nameof(repository));

            this.productCodeRepository = productCodesRepository ??
                throw new ArgumentNullException(nameof(productCodesRepository));

            this.customerRepository = customerRepository ??
                throw new ArgumentNullException(nameof(customerRepository));

            this.vehicleRepository = vehicleRepository ??
                throw new ArgumentNullException(nameof(vehicleRepository));

            this.saleCodeRepository = saleCodeRepository ??
                throw new ArgumentNullException(nameof(saleCodeRepository));
        }

        // api/repairorders/listing
        [Route("listing")]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<RepairOrderToReadInList>>> GetRepairOrderListAsync()
        {
            var results = await repository.Get();

            if (results == null)
                return NotFound();

            return Ok(results);
        }

        // api/repairorders/1
        [HttpGet("{id:long}")]
        public async Task<ActionResult<RepairOrderToRead>> GetRepairOrderAsync(long id)
        {
            var result = await repository.Get(id);

            if (result == null)
                return NotFound();

            return result;
        }

        // api/repairorders/1
        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(long id, RepairOrderToWrite repairOrderFromCaller)
        {
            var repairOrderFromRepository = await repository.GetEntity(id);

            if (repairOrderFromRepository is null)
                return NotFound($"Could not find Repair Order #{id} to update.");

            var customer = await customerRepository.GetCustomerEntityAsync(repairOrderFromCaller.Customer.Id);
            var vehicle = await vehicleRepository.GetEntity(repairOrderFromCaller.Vehicle.Id);
            var repairOrderNumbers = new List<long>();

            var result = UpdateRepairOrder(repairOrderFromCaller, repairOrderFromRepository, customer, vehicle, new List<long>());

            if (result.IsFailure)
                return BadRequest(result.Error);

            // await UpdateServicesAsync(repairOrderFromCaller, repairOrderFromRepository, manufacturerCodes);
            //UpdatePayments(repairOrderFromCaller, repairOrderFromRepository);
            //UpdateTaxes(repairOrderFromCaller, repairOrderFromRepository);

            await repository.SaveChanges();

            return NoContent();
        }

        private static void UpdateTaxes(RepairOrderToWrite repairOrder, RepairOrder repairOrderFromRepository)
        {
            //foreach (var tax in repairOrder?.Taxes)
            //{
            //    var editableTax = repairOrderFromRepository.Taxes.Find(x => x.Id == tax.Id);
            //    if (editableTax is null)
            //        continue;

            //    editableTax.LaborTax = tax.LaborTax;
            //    editableTax.LaborTaxRate = tax.LaborTaxRate;
            //    editableTax.PartTax = tax.PartTax;
            //    editableTax.PartTaxRate = tax.PartTaxRate;
            //    editableTax.RepairOrderId = tax.RepairOrderId;
            //    editableTax.TaxId = tax.TaxId;
            //}
        }

        private static void UpdatePayments(RepairOrderToWrite repairOrder, RepairOrder repairOrderFromRepository)
        {
            //foreach (var payment in repairOrder?.Payments)
            //{
            //    var editablePayment = repairOrderFromRepository.Payments.Find(x => x.Id == payment.Id);
            //    if (editablePayment is null)
            //        continue;

            //    editablePayment.Amount = payment.Amount;
            //    editablePayment.PaymentMethod = payment.PaymentMethod;
            //    editablePayment.RepairOrderId = payment.RepairOrderId;
            //}
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
            Vehicle vehicle,
            List<long> repairOrderNumbers)
        {
            return Result.Combine(
                (customer is not null) && (customer != repairOrderFromRepository.Customer)
                    ? repairOrderFromRepository.SetCustomer(customer)
                    : Result.Success(),
                (repairOrderFromCaller.InvoiceNumber != repairOrderFromRepository.InvoiceNumber)
                    ? repairOrderFromRepository.SetInvoiceNumber(repairOrderFromCaller.InvoiceNumber)
                    : Result.Success(),
                (vehicle is not null) && (vehicle != repairOrderFromRepository.Vehicle)
                    ? repairOrderFromRepository.SetVehicle(vehicle)
                    : Result.Success(),
                (repairOrderFromCaller.AccountingDate != repairOrderFromRepository.AccountingDate)
                    ? repairOrderFromRepository.SetAccountingDate(repairOrderFromCaller.AccountingDate)
                    : Result.Success(),
                (repairOrderNumbers is not null)
                    ? repairOrderFromRepository.SetRepairOrderNumber(repairOrderNumbers, DateTime.Today)
                    : Result.Success()
            );
        }

        [HttpPost]
        public async Task<IActionResult> Add(RepairOrderToWrite repairOrderToAdd)
        {
            var customer = await customerRepository.GetCustomerEntityAsync(repairOrderToAdd.Customer.Id);
            var vehicle = await vehicleRepository.GetEntity(repairOrderToAdd.Vehicle.Id);
            var repairOrderNumbers = await repository.GetTodaysRepairOrderNumbers();
            var saleCodeIds = repairOrderToAdd.Services.Select(service => service.SaleCode.Id).ToList();
            var saleCodes = await saleCodeRepository.GetSaleCodeEntitiesAsync(saleCodeIds);
            var lastInvoiceNumberOrSeed = repository.GetLastInvoiceNumberOrSeed();
            var repairOrder = RepairOrderHelper.ConvertWriteDtoToEntity(repairOrderToAdd, customer, vehicle, repairOrderNumbers, lastInvoiceNumberOrSeed, saleCodes);

            await repository.Add(repairOrder);
            await repository.SaveChanges();

            return Created(
                new Uri($"{BasePath}/{repairOrder.Id}", UriKind.Relative),
                new { id = repairOrder.Id });
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            var roFromRepository = await repository.Get(id);
            if (roFromRepository == null)
                return NotFound($"Could not find Repair Order in the database to delete with id of {id}.");

            await repository.Delete(id);
            await repository.SaveChanges();

            return NoContent();
        }
    }
}