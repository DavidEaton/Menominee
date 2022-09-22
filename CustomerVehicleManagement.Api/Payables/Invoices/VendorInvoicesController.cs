using CustomerVehicleManagement.Api.Data;
using CustomerVehicleManagement.Api.Manufacturers;
using CustomerVehicleManagement.Api.Payables.PaymentMethods;
using CustomerVehicleManagement.Api.Payables.Vendors;
using CustomerVehicleManagement.Api.SaleCodes;
using CustomerVehicleManagement.Api.Taxes;
using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Domain.Entities.Inventory;
using CustomerVehicleManagement.Domain.Entities.Payables;
using CustomerVehicleManagement.Domain.Entities.Taxes;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices.LineItems.Items;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.Payables.Invoices
{
    public class VendorInvoicesController : ApplicationController
    {
        private readonly IVendorInvoiceRepository repository;
        private readonly IVendorRepository vendorRepository;
        private readonly IVendorInvoicePaymentMethodRepository paymentMethodRepository;
        private readonly ISalesTaxRepository salesTaxRepository;
        private readonly IManufacturerRepository manufacturerRepository;
        private readonly ISaleCodeRepository saleCodeRepository;
        private readonly string BasePath = "/api/vendorinvoices";

        public VendorInvoicesController(
            IVendorInvoiceRepository repository,
            IVendorRepository vendorRepository,
            IVendorInvoicePaymentMethodRepository paymentMethodRepository,
            ISalesTaxRepository salesTaxRepository,
            IManufacturerRepository manufacturerRepository,
            ISaleCodeRepository saleCodeRepository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.vendorRepository = vendorRepository ?? throw new ArgumentNullException(nameof(vendorRepository)); ;
            this.paymentMethodRepository = paymentMethodRepository ?? throw new ArgumentNullException(nameof(paymentMethodRepository));
            this.salesTaxRepository = salesTaxRepository ?? throw new ArgumentNullException(nameof(salesTaxRepository));
            this.manufacturerRepository = manufacturerRepository ?? throw new ArgumentNullException(nameof(manufacturerRepository));
            this.saleCodeRepository = saleCodeRepository ?? throw new ArgumentNullException(nameof(saleCodeRepository));
        }

        // GET: api/vendorinvoices/list
        [Route("listing")]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<VendorInvoiceToReadInList>>> GetInvoiceListAsync()
        {
            var invoices = await repository.GetInvoiceListAsync();

            if (invoices == null)
                return NotFound();

            return Ok(invoices);
        }

        // GET: api/vendorinvoices/1
        [HttpGet("{id:long}", Name = "GetInvoiceAsync")]
        public async Task<ActionResult<VendorInvoiceToRead>> GetInvoiceAsync(long id)
        {
            var invoice = await repository.GetInvoiceAsync(id);

            if (invoice == null)
                return NotFound();

            return Ok(invoice);
        }

        // PUT: api/vendorinvoices/1
        [HttpPut("{id:long}")]
        public async Task<IActionResult> UpdateInvoiceAsync(long id, VendorInvoiceToWrite invoiceFromCaller)
        {
            var notFoundMessage = $"Could not find Vendor Invoice to update with Id = {id}.";

            if (!await repository.InvoiceExistsAsync(id))
                return NotFound(notFoundMessage);

            var invoiceFromRepository = await repository.GetInvoiceEntityAsync(id);

            if (invoiceFromRepository is null)
                return NotFound(notFoundMessage);

            // Update each member of VendorInvoice
            // Aggregate root entity VendorInvoice:
            if (invoiceFromRepository.SetVendor(await vendorRepository.GetVendorEntityAsync(invoiceFromCaller.Vendor.Id)).IsFailure)
                return BadRequest();

            if (invoiceFromRepository.SetVendorInvoiceStatus(invoiceFromCaller.Status).IsFailure)
                return BadRequest();

            if (invoiceFromRepository.SetTotal(invoiceFromCaller.Total).IsFailure)
                return BadRequest();

            if (invoiceFromRepository.SetInvoiceNumber(invoiceFromCaller.InvoiceNumber).IsFailure)
                return BadRequest();

            if (invoiceFromRepository.SetDate(invoiceFromCaller.Date).IsFailure)
                return BadRequest();

            if (invoiceFromCaller.DatePosted is not null)
                if (invoiceFromRepository.SetDatePosted(invoiceFromCaller.DatePosted).IsFailure)
                    return BadRequest();

            foreach (var lineItem in invoiceFromCaller?.LineItems)
            {
                // Added
                if (lineItem.Id == 0)
                    invoiceFromRepository.AddLineItem(
                        VendorInvoiceLineItem.Create(
                            lineItem.Type, VendorInvoiceItemHelper.ConvertWriteDtoToEntity(
                                lineItem.Item,
                                await GetManufacturers(invoiceFromCaller),
                                await GetSaleCodes(invoiceFromCaller)),
                            lineItem.Quantity,
                            lineItem.Cost,
                            lineItem.Core,
                            lineItem.PONumber,
                            lineItem.TransactionDate)
                        .Value);
                // Updated
                if (lineItem.Id != 0)
                {
                    var contextLineItem = invoiceFromRepository?.LineItems.FirstOrDefault(contextLineItem => contextLineItem.Id == lineItem.Id);
                    contextLineItem.SetType(lineItem.Type);
                    contextLineItem.SetItem(VendorInvoiceItemHelper.ConvertWriteDtoToEntity(
                        lineItem.Item,
                        await GetManufacturers(invoiceFromCaller),
                        await GetSaleCodes(invoiceFromCaller)));
                    contextLineItem.SetQuantity(lineItem.Quantity);
                    contextLineItem.SetCost(lineItem.Cost);
                    contextLineItem.SetCore(lineItem.Core);
                    contextLineItem.SetPONumber(lineItem.PONumber);
                    contextLineItem.SetTransactionDate(lineItem.TransactionDate);
                    contextLineItem.SetTrackingState(TrackingState.Modified);
                }
                // TODO: Deleted
                //if (lineItem.Id != 0)
                //    invoiceFromRepository.RemoveLineItem(
                //        invoiceFromRepository.LineItems.FirstOrDefault(
                //            contextLineItem =>
                //            contextLineItem.Id == lineItem.Id));
            }
            //IList<VendorInvoicePayment> Payments
            foreach (var payment in invoiceFromCaller?.Payments)
            {
                // Added
                if (payment.Id == 0)
                    invoiceFromRepository.AddPayment(
                        VendorInvoicePayment.Create(
                            await paymentMethodRepository.GetPaymentMethodEntityAsync(
                                payment.PaymentMethod.Id), payment.Amount)
                        .Value);
                // Updated
                if (payment.Id != 0)
                {
                    var contextPayment = invoiceFromRepository?.Payments.FirstOrDefault(contextPayment => contextPayment.Id == payment.Id);
                    contextPayment.SetPaymentMethod(
                        await paymentMethodRepository.GetPaymentMethodEntityAsync(payment.PaymentMethod.Id));
                    contextPayment.SetAmount(payment.Amount);
                    //contextPayment.SetTrackingState(TrackingState.Modified);
                }
                // TODO: Deleted
                //if (payment.Id != 0)
                //    invoiceFromRepository.RemovePayment(
                //        invoiceFromRepository.Payments.FirstOrDefault(
                //            contextPayment =>
                //            contextPayment.Id == payment.Id));
            }
            //IList<VendorInvoiceTax> Taxes
            foreach (var tax in invoiceFromCaller?.Taxes)
            {
                // Added
                if (tax.Id == 0)
                    invoiceFromRepository.AddTax(
                        VendorInvoiceTax.Create(
                            await salesTaxRepository.GetSalesTaxEntityAsync(tax.SalesTax.Id), tax.TaxId).Value);
                // Updated
                if (tax.Id != 0)
                {
                    var contextTax = invoiceFromRepository?.Taxes.FirstOrDefault(contextTax => contextTax.Id == tax.Id);
                    contextTax.SetSalesTax(await salesTaxRepository.GetSalesTaxEntityAsync(tax.SalesTax.Id));
                    contextTax.SetTaxId(tax.TaxId);
                    contextTax.SetTrackingState(TrackingState.Modified);
                }
                // TODO: Deleted
                //if (tax.Id != 0)
                //    invoiceFromRepository.RemoveTax(
                //        invoiceFromRepository.Taxes.FirstOrDefault(
                //            contextTax =>
                //            contextTax.Id == tax.Id));
            }


            // Remove Julie Lerman's superior implementation in favor of EF
            // Core 6 flawed way of treating attached objects: marking them
            // as Modified when they should be marked as Unchanged until
            // they are modified while context tracks it.
            //invoiceFromRepository.SetTrackingState(TrackingState.Modified);
            //repository.FixTrackingState();

            repository.InspectTrackingStates(invoiceFromRepository);

            await repository.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/vendorinvoices
        [HttpPost]
        public async Task<ActionResult<VendorInvoiceToRead>> AddInvoiceAsync(VendorInvoiceToWrite invoiceToAdd)
        {
            var vendor = await vendorRepository.GetVendorEntityAsync(invoiceToAdd.Vendor.Id);

            var invoice = VendorInvoiceHelper.ConvertWriteDtoToEntity(
                invoiceToAdd,
                vendor,
                await GetManufacturers(invoiceToAdd),
                await GetSaleCodes(invoiceToAdd),
                await salesTaxRepository.GetSalesTaxEntities(),
                await paymentMethodRepository.GetPaymentMethodsAsync());

            repository.AddInvoice(invoice);
            await repository.SaveChangesAsync();

            return Created(new Uri($"{BasePath}/{invoice.Id}",
                UriKind.Relative),
                new { invoice.Id });
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeleteInvoiceAsync(long id)
        {
            var invoiceFromRepository = await repository.GetInvoiceEntityAsync(id);

            if (invoiceFromRepository is null)
                return NotFound($"Could not find Vendor Invoice in the database to delete with Id: {id}.");

            repository.DeleteInvoice(invoiceFromRepository);

            await repository.SaveChangesAsync();

            return NoContent();
        }

        private async Task<IReadOnlyList<Manufacturer>> GetManufacturers(VendorInvoiceToWrite invoice)
        {
            return await manufacturerRepository.GetManufacturerEntitiesAsync(
                invoice.LineItems?.Select(
                    lineItem => lineItem.Item.Manufacturer.Id)
                .ToList());
        }

        private async Task<IReadOnlyList<SaleCode>> GetSaleCodes(VendorInvoiceToWrite invoiceToAdd)
        {
            return await saleCodeRepository.GetSaleCodeEntitiesAsync(
                invoiceToAdd.LineItems?.Select(
                    lineItem => lineItem.Item.SaleCode.Id)
                .ToList());
        }

    }
}
