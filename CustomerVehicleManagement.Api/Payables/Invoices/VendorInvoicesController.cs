using CustomerVehicleManagement.Api.Data;
using CustomerVehicleManagement.Api.Manufacturers;
using CustomerVehicleManagement.Api.Payables.PaymentMethods;
using CustomerVehicleManagement.Api.Payables.Vendors;
using CustomerVehicleManagement.Api.SaleCodes;
using CustomerVehicleManagement.Api.Taxes;
using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Domain.Entities.Inventory;
using CustomerVehicleManagement.Domain.Entities.Payables;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices.LineItems.Items;
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

            // Load lookup lists once
            IReadOnlyList<Manufacturer> manufacturers = await GetManufacturers(invoiceFromCaller);
            IReadOnlyList<SaleCode> salesCodes = await GetSaleCodes(invoiceFromCaller);

            // VendorInvoiceLineItem is an Entity that contains the VendorInvoiceItem Value Object
            // VendorInvoiceItem is a Value Object (no Id) that contains the Item (aka Part)


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

            // Line Items
            foreach (var lineItemFromCaller in invoiceFromCaller?.LineItems)
            {
                // Added
                if (lineItemFromCaller.Id == 0)
                    invoiceFromRepository.AddLineItem(
                        VendorInvoiceLineItem.Create(
                            lineItemFromCaller.Type,
                            item: VendorInvoiceItemHelper.ConvertWriteDtoToEntity(
                                lineItemFromCaller.Item,
                                manufacturers,
                                salesCodes),
                            lineItemFromCaller.Quantity,
                            lineItemFromCaller.Cost,
                            lineItemFromCaller.Core,
                            lineItemFromCaller.PONumber,
                            lineItemFromCaller.TransactionDate)
                        .Value);
                // Updated
                if (lineItemFromCaller.Id != 0)
                {
                    var lineItemFromContext = invoiceFromRepository?.LineItems.FirstOrDefault(contextLineItem => contextLineItem.Id == lineItemFromCaller.Id);
                    lineItemFromContext.SetType(lineItemFromCaller.Type);
                    lineItemFromContext.SetItem(VendorInvoiceItemHelper.ConvertWriteDtoToEntity(
                        lineItemFromCaller.Item,
                        manufacturers,
                        salesCodes));
                    lineItemFromContext.SetQuantity(lineItemFromCaller.Quantity);
                    lineItemFromContext.SetCost(lineItemFromCaller.Cost);
                    lineItemFromContext.SetCore(lineItemFromCaller.Core);
                    lineItemFromContext.SetPONumber(lineItemFromCaller.PONumber);
                    lineItemFromContext.SetTransactionDate(lineItemFromCaller.TransactionDate);
                    //contextLineItem.SetTrackingState(TrackingState.Modified);
                }
                // TODO: Deleted
                //if (lineItem.Id != 0)
                //    invoiceFromRepository.RemoveLineItem(
                //        invoiceFromRepository.LineItems.FirstOrDefault(
                //            contextLineItem =>
                //            contextLineItem.Id == lineItem.Id));
            }
            // Payments
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
            // Taxes
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
                }
                // TODO: Deleted
                //if (tax.Id != 0)
                //    invoiceFromRepository.RemoveTax(
                //        invoiceFromRepository.Taxes.FirstOrDefault(
                //            contextTax =>
                //            contextTax.Id == tax.Id));
            }

            //repository.InspectTrackingStates(invoiceFromRepository);

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

        private async Task<IReadOnlyList<SaleCode>> GetSaleCodes(VendorInvoiceToWrite invoice)
        {
            return await saleCodeRepository.GetSaleCodeEntitiesAsync(
                invoice.LineItems?.Select(
                    lineItem => lineItem.Item.SaleCode.Id)
                .ToList());
        }

    }
}
