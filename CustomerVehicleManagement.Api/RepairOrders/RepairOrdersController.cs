using CustomerVehicleManagement.Domain.Entities.RepairOrders;
using CustomerVehicleManagement.Shared.Models.RepairOrders;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.RepairOrders
{
    [ApiController]
    [Route("api/[controller]")]
    public class RepairOrdersController : ControllerBase
    {
        private readonly IRepairOrderRepository repository;
        private readonly string BasePath = "/api/repairorders";

        public RepairOrdersController(IRepairOrderRepository repository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        // api/repairorders/listing
        [Route("listing")]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<RepairOrderToReadInList>>> GetRepairOrderListAsync()
        {
            var results = await repository.GetRepairOrderListAsync();

            //if (results == null)
            //    return NotFound();

            return Ok(results);
        }

        // api/repairorders
        //[HttpGet]
        //public async Task<ActionResult<IReadOnlyList<RepairOrderToRead>>> GetRepairOrdersAsync()
        //{
        //    var result = await repository.GetRepairOrdersAsync();
        //    return Ok(result);
        //}

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
        public async Task<IActionResult> UpdateRepairOrderAsync(long id, RepairOrderToWrite roToUpdate)
        {
            if (!await repository.RepairOrderExistsAsync(id))
                return NotFound($"Could not find RO # {roToUpdate.Id} to update.");

            //1) Get domain entity from repository
            var ro = repository.GetRepairOrderEntityAsync(id).Result;
            WriteDtoToEntity(roToUpdate, ro);

            // 2) Update domain entity with data in data transfer object(DTO)

            // Update the objects ObjectState and synch the EF Change Tracker
            // 3) Set entity's TrackingState to Modified
            ro.SetTrackingState(TrackingState.Modified);

            // 4) FixTrackingState: moves entity state tracking into the context
            repository.FixTrackingState();

            repository.UpdateRepairOrderAsync(ro);

            await repository.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<RepairOrderToRead>> AddRepairOrderAsync(RepairOrderToWrite roToWrite)
        {
            // 1. Convert dto to domain entity
            var ro = new RepairOrder();
            WriteDtoToEntity(roToWrite, ro);

            // 2. Add domain entity to repository
            await repository.AddRepairOrderAsync(ro);

            // 3. Save changes on repository
            await repository.SaveChangesAsync();

            // 4. Return new Id from database to consumer after save
            return Created(new Uri($"{BasePath}/{ro.Id}", UriKind.Relative), new { id = ro.Id });
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeleteRepairOrderAsync(long id)
        {
            var roFromRepository = await repository.GetRepairOrderAsync(id);
            if (roFromRepository == null)
                return NotFound($"Could not find RO in the database to delete with id of {id}.");

            await repository.DeleteRepairOrderAsync(id);

            if (await repository.SaveChangesAsync())
                return NoContent();

            return BadRequest($"Failed to delete Repair Order with Id of {id}.");
        }

        private void WriteDtoToEntity(RepairOrderToWrite roToUpdate, RepairOrder ro)
        {
            ro.RepairOrderNumber = roToUpdate.RepairOrderNumber;
            ro.InvoiceNumber = roToUpdate.InvoiceNumber;
            ro.CustomerName = roToUpdate.CustomerName;
            ro.Vehicle = roToUpdate.Vehicle;
            ro.Total = roToUpdate.Total;
            ro.DateCreated = (DateTime)roToUpdate.DateCreated;
            ro.DateModified = (DateTime)roToUpdate.DateModified;
            ro.DateInvoiced = (DateTime)roToUpdate.DateInvoiced;

            List<RepairOrderService> services = new();
            List<RepairOrderPayment> payments = new();
            List<RepairOrderTax> taxes = new();

            if (roToUpdate?.Services?.Count > 0)
            {
                foreach (var serviceToUpdate in roToUpdate.Services)
                {
                    RepairOrderService service = new();
                    List<RepairOrderItem> items = new();
                    List<RepairOrderTech> techs = new();
                    List<RepairOrderServiceTax> serviceTaxes = new();

                    service.RepairOrderId = serviceToUpdate.RepairOrderId;
                    service.SequenceNumber = serviceToUpdate.SequenceNumber;
                    service.ServiceName = serviceToUpdate.ServiceName;
                    service.SaleCode = serviceToUpdate.SaleCode;
                    service.IsCounterSale = serviceToUpdate.IsCounterSale;
                    service.IsDeclined = serviceToUpdate.IsDeclined;
                    service.PartsTotal = serviceToUpdate.PartsTotal;
                    service.LaborTotal = serviceToUpdate.LaborTotal;
                    service.TaxTotal = serviceToUpdate.TaxTotal;
                    service.ShopSuppliesTotal = serviceToUpdate.ShopSuppliesTotal;
                    service.Total = serviceToUpdate.Total;

                    if (serviceToUpdate.Items?.Count > 0)
                    {
                        foreach (var itemToUpdate in serviceToUpdate.Items)
                        {
                            RepairOrderItem item = new();
                            List<RepairOrderSerialNumber> serialNumbers = new();
                            List<RepairOrderWarranty> warranties = new();
                            List<RepairOrderItemTax> itemTaxes = new();

                            item.RepairOrderServiceId = itemToUpdate.RepairOrderServiceId;
                            item.SequenceNumber = itemToUpdate.SequenceNumber;
                            item.ManufacturerId = itemToUpdate.ManufacturerId;
                            item.PartNumber = itemToUpdate.PartNumber;
                            item.Description = itemToUpdate.Description;
                            item.SaleCode = itemToUpdate.SaleCode;
                            item.ProductCode = itemToUpdate.ProductCode;
                            item.SaleType = itemToUpdate.SaleType;
                            item.PartType = itemToUpdate.PartType;
                            item.IsDeclined = itemToUpdate.IsDeclined;
                            item.IsCounterSale = itemToUpdate.IsDeclined;
                            item.QuantitySold = itemToUpdate.QuantitySold;
                            item.SellingPrice = itemToUpdate.SellingPrice;
                            item.LaborEach = itemToUpdate.LaborEach;
                            item.Cost = itemToUpdate.Cost;
                            item.Core = itemToUpdate.Core;
                            item.Total = itemToUpdate.Total;

                            if (itemToUpdate.SerialNumbers?.Count > 0)
                            {
                                foreach (var serialNumberToUpdate in itemToUpdate.SerialNumbers)
                                {
                                    RepairOrderSerialNumber serialNumber = new();
                                    serialNumber.RepairOrderItemId = serialNumberToUpdate.RepairOrderItemId;
                                    serialNumber.SerialNumber = serialNumberToUpdate.SerialNumber;

                                    serialNumbers.Add(serialNumber);
                                }
                            }
                            item.SetSerialNumbers(serialNumbers);

                            if (itemToUpdate.Warranties?.Count > 0)
                            {
                                foreach (var warrantyToUpdate in itemToUpdate.Warranties)
                                {
                                    RepairOrderWarranty warranty = new();
                                    warranty.RepairOrderItemId = warrantyToUpdate.RepairOrderItemId;
                                    warranty.SequenceNumber = warrantyToUpdate.SequenceNumber;
                                    warranty.Quantity = warrantyToUpdate.Quantity;
                                    warranty.Type = warrantyToUpdate.Type;
                                    warranty.NewWarranty = warrantyToUpdate.NewWarranty;
                                    warranty.OriginalWarranty = warrantyToUpdate.OriginalWarranty;
                                    warranty.OriginalInvoiceId = warrantyToUpdate.OriginalInvoiceId;

                                    warranties.Add(warranty);
                                }
                            }
                            item.SetWarranties(warranties);

                            if (itemToUpdate.Taxes?.Count > 0)
                            {
                                foreach (var taxToUpdate in itemToUpdate.Taxes)
                                {
                                    RepairOrderItemTax tax = new();
                                    tax.RepairOrderItemId = taxToUpdate.RepairOrderItemId;
                                    tax.SequenceNumber = taxToUpdate.SequenceNumber;
                                    tax.TaxId = taxToUpdate.TaxId;
                                    tax.PartTaxRate = taxToUpdate.PartTaxRate;
                                    tax.LaborTaxRate = taxToUpdate.LaborTaxRate;
                                    tax.PartTax = taxToUpdate.PartTax;
                                    tax.LaborTax = taxToUpdate.LaborTax;

                                    itemTaxes.Add(tax);
                                }
                            }
                            item.SetTaxes(itemTaxes);

                            items.Add(item);
                        }
                    }
                    service.SetItems(items);

                    if (serviceToUpdate.Techs?.Count > 0)
                    {
                        foreach (var techToUpdate in serviceToUpdate.Techs)
                        {
                            RepairOrderTech tech = new();
                            tech.RepairOrderServiceId = techToUpdate.RepairOrderServiceId;
                            tech.TechnicianId = techToUpdate.TechnicianId;

                            techs.Add(tech);
                        }
                    }
                    service.SetTechs(techs);

                    if (serviceToUpdate.Taxes?.Count > 0)
                    {
                        foreach (var taxToUpdate in serviceToUpdate.Taxes)
                        {
                            RepairOrderServiceTax tax = new();
                            tax.RepairOrderServiceId = taxToUpdate.RepairOrderServiceId;
                            tax.SequenceNumber = taxToUpdate.SequenceNumber;
                            tax.TaxId = taxToUpdate.TaxId;
                            tax.PartTaxRate = taxToUpdate.PartTaxRate;
                            tax.LaborTaxRate = taxToUpdate.LaborTaxRate;
                            tax.PartTax = taxToUpdate.PartTax;
                            tax.LaborTax = taxToUpdate.LaborTax;

                            serviceTaxes.Add(tax);
                        }
                    }
                    service.SetTaxes(serviceTaxes);

                    services.Add(service);
                }
            }
            ro.SetServices(services);

            if (roToUpdate?.Taxes?.Count > 0)
            {
                foreach (var taxToUpdate in roToUpdate.Taxes)
                {
                    RepairOrderTax tax = new();
                    tax.RepairOrderId = taxToUpdate.RepairOrderId;
                    tax.SequenceNumber = taxToUpdate.SequenceNumber;
                    tax.TaxId = taxToUpdate.TaxId;
                    tax.PartTaxRate = taxToUpdate.PartTaxRate;
                    tax.LaborTaxRate = taxToUpdate.LaborTaxRate;
                    tax.PartTax = taxToUpdate.PartTax;
                    tax.LaborTax = taxToUpdate.LaborTax;

                    taxes.Add(tax);
                }
            }
            ro.SetTaxes(taxes);

            if (roToUpdate?.Payments?.Count > 0)
            {
                foreach (var paymentToUpdate in roToUpdate.Payments)
                {
                    RepairOrderPayment payment = new();
                    payment.RepairOrderId = paymentToUpdate.RepairOrderId;
                    payment.PaymentMethod = paymentToUpdate.PaymentMethod;
                    payment.Amount = paymentToUpdate.Amount;

                    payments.Add(payment);
                }
            }
            ro.SetPayments(payments);
        }
    }
}
