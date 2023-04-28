using CustomerVehicleManagement.Api.ProductCodes;
using CustomerVehicleManagement.Domain.Entities.RepairOrders;
using CustomerVehicleManagement.Shared.Models.ProductCodes;
using CustomerVehicleManagement.Shared.Models.RepairOrders;
using CustomerVehicleManagement.Shared.Models.RepairOrders.Items;
using CustomerVehicleManagement.Shared.Models.RepairOrders.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace CustomerVehicleManagement.Api.RepairOrders
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class RepairOrdersController : ControllerBase
    {
        private readonly IRepairOrderRepository repository;
        private readonly IProductCodeRepository productCodesRepository;
        private readonly string BasePath = "/api/repairorders";

        public RepairOrdersController(IRepairOrderRepository repository, IProductCodeRepository productCodesRepository)
        {
            this.repository = repository ??
                throw new ArgumentNullException(nameof(repository));

            this.productCodesRepository = productCodesRepository ??
                throw new ArgumentNullException(nameof(productCodesRepository));
        }

        // api/repairorders/listing
        [Route("listing")]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<RepairOrderToReadInList>>> GetRepairOrderListAsync()
        {
            var results = await repository.GetRepairOrderListAsync();

            if (results == null)
                return NotFound();

            return Ok(results);
        }

        // api/repairorders/1
        [HttpGet("{id:long}")]
        public async Task<ActionResult<RepairOrderToRead>> GetRepairOrderAsync(long id)
        {
            var result = await repository.GetRepairOrderAsync(id);

            if (result == null)
                return NotFound();

            return result;
        }

        // api/repairorders/1
        [HttpPut("{id:long}")]
        public async Task<IActionResult> UpdateRepairOrderAsync(long id, RepairOrderToWrite repairOrderFromCaller)
        {
            var notFoundMessage = $"Could not find Repair Order #{id} to update.";

            var repairOrderFromRepository = await repository.GetRepairOrderEntityAsync(id);

            if (repairOrderFromRepository is null)
                return NotFound(notFoundMessage);

            // UpdateRepairOrder(repairOrderFromCaller, repairOrderFromRepository);
            // await UpdateServicesAsync(repairOrderFromCaller, repairOrderFromRepository, manufacturerCodes);
            //UpdatePayments(repairOrder, repairOrderFromRepository);
            //UpdateTaxes(repairOrder, repairOrderFromRepository);

            //await repository.SaveChangesAsync();

            return NoContent();
        }

        private static void UpdateTaxes(RepairOrderToWrite repairOrder, RepairOrder repairOrderFromRepository)
        {
            foreach (var tax in repairOrder?.Taxes)
            {
                var editableTax = repairOrderFromRepository.Taxes.Find(x => x.Id == tax.Id);
                if (editableTax is null)
                    continue;

                editableTax.LaborTax = tax.LaborTax;
                editableTax.LaborTaxRate = tax.LaborTaxRate;
                editableTax.PartTax = tax.PartTax;
                editableTax.PartTaxRate = tax.PartTaxRate;
                editableTax.RepairOrderId = tax.RepairOrderId;
                editableTax.TaxId = tax.TaxId;
            }
        }

        private static void UpdatePayments(RepairOrderToWrite repairOrder, RepairOrder repairOrderFromRepository)
        {
            foreach (var payment in repairOrder?.Payments)
            {
                var editablePayment = repairOrderFromRepository.Payments.Find(x => x.Id == payment.Id);
                if (editablePayment is null)
                    continue;

                editablePayment.Amount = payment.Amount;
                editablePayment.PaymentMethod = payment.PaymentMethod;
                editablePayment.RepairOrderId = payment.RepairOrderId;
            }
        }

        private static async Task UpdateServicesAsync(RepairOrderToWrite repairOrder, RepairOrder repairOrderFromRepository, IReadOnlyList<ProductCodeToRead> productCodes)
        {
            foreach (var service in repairOrder?.Services)
            {
                var editableService = repairOrderFromRepository.Services.Find(x => x.Id == service.Id);

                if (editableService is null)
                    continue;

                editableService.DiscountTotal = service.DiscountTotal;
                editableService.IsCounterSale = service.IsCounterSale;
                editableService.IsDeclined = service.IsDeclined;
                editableService.LaborTotal = service.LaborTotal;
                editableService.PartsTotal = service.PartsTotal;
                editableService.RepairOrderId = repairOrderFromRepository.Id;
                editableService.SaleCode = service.SaleCode;
                editableService.ServiceName = service.ServiceName;
                editableService.ShopSuppliesTotal = service.ShopSuppliesTotal;
                editableService.TaxTotal = service.TaxTotal;
                editableService.Total = service.Total;

                await UpdateServiceItemsAsync(service, editableService, productCodes);
                //UpdateServiceTaxes(service);
                //UpdateServiceTechnician(service);

                // Services.Taxes = RepairOrderHelper.CreateServiceTaxes(service.Taxes),
                // Services.Techs = RepairOrderHelper.CreateTechnicians(service.Techs)

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

        private static void UpdateServiceItemWarranties(RepairOrderItemToWrite item, RepairOrderItem editableItem)
        {
            List<RepairOrderWarranty> addedWarranties = new();

            foreach (var warranty in item?.Warranties)
            {
                RepairOrderWarranty editableWarranty
                    = editableItem?.Warranties.Find(x => x.Id == warranty.Id);

                editableWarranty = editableWarranty ?? new();

                editableWarranty.RepairOrderItemId = warranty.RepairOrderItemId;
                editableWarranty.NewWarranty = warranty.NewWarranty;
                editableWarranty.OriginalInvoiceId = warranty.OriginalInvoiceId;
                editableWarranty.OriginalWarranty = warranty.OriginalWarranty;
                editableWarranty.Quantity = warranty.Quantity;
                editableWarranty.Type = warranty.Type;

                if (editableWarranty.Id == 0)
                    addedWarranties.Add(editableWarranty);
            }

            if (addedWarranties.Count > 0)
                editableItem.Warranties.AddRange(addedWarranties);
        }

        private static void UpdateServiceItemSerialNumbers(RepairOrderItemToWrite item, RepairOrderItem editableItem)
        {
            List<RepairOrderSerialNumber> addedSerialNumbers = new();

            foreach (var serialNumber in item?.SerialNumbers)
            {
                var editableSerialNumber = editableItem?.SerialNumbers.Find(x => x.Id == serialNumber.Id);

                editableSerialNumber = editableSerialNumber ?? new();

                editableSerialNumber.RepairOrderItemId = serialNumber.RepairOrderItemId;
                editableSerialNumber.SerialNumber = serialNumber.SerialNumber;

                if (editableSerialNumber.Id == 0)
                    // This unfound serial number was added in BuildSerialNumberList, disconnected
                    // from the db context, so we must add it to the tracked collection here
                    addedSerialNumbers.Add(editableSerialNumber);
            }

            if (addedSerialNumbers.Count > 0)
                editableItem.SerialNumbers.AddRange(addedSerialNumbers);
        }

        private static void UpdateRepairOrder(RepairOrderToWrite repairOrder, RepairOrder repairOrderFromRepository)
        {
            repairOrderFromRepository.CustomerName = repairOrder.CustomerName;
            repairOrderFromRepository.DateModified = DateTime.Today;
            repairOrderFromRepository.DiscountTotal = repairOrder.DiscountTotal;
            repairOrderFromRepository.HazMatTotal = repairOrder.HazMatTotal;
            repairOrderFromRepository.InvoiceNumber = repairOrder.InvoiceNumber;
            repairOrderFromRepository.LaborTotal = repairOrder.LaborTotal;
            repairOrderFromRepository.PartsTotal = repairOrder.PartsTotal;
            repairOrderFromRepository.RepairOrderNumber = repairOrder.RepairOrderNumber;
            repairOrderFromRepository.ShopSuppliesTotal = repairOrder.ShopSuppliesTotal;
            repairOrderFromRepository.TaxTotal = repairOrder.TaxTotal;
            repairOrderFromRepository.Total = repairOrder.Total;
            repairOrderFromRepository.Vehicle = repairOrder.Vehicle;

            if (repairOrder.DateInvoiced.HasValue)
                repairOrderFromRepository.DateInvoiced = (DateTime)repairOrder.DateInvoiced;
        }

        [HttpPost]
        public async Task<IActionResult> AddRepairOrderAsync(RepairOrderToWrite repairOrderToAdd)
        {
            //var repairOrder = RepairOrderHelper.ConvertWriteDtoToEntity(repairOrderToAdd);

            //await repository.AddRepairOrderAsync(repairOrder);
            //await repository.SaveChangesAsync();

            //return Created(new Uri($"{BasePath}/{repairOrder.Id}",
            //                   UriKind.Relative),
            //                   new { id = repairOrder.Id });\
            throw new NotImplementedException();
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeleteRepairOrderAsync(long id)
        {
            var roFromRepository = await repository.GetRepairOrderAsync(id);
            if (roFromRepository == null)
                return NotFound($"Could not find Repair Order in the database to delete with id of {id}.");

            await repository.DeleteRepairOrderAsync(id);
            await repository.SaveChangesAsync();

            return NoContent();
        }
    }
}
