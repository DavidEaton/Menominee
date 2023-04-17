using CSharpFunctionalExtensions;
using CustomerVehicleManagement.Api.Common;
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
using CustomerVehicleManagement.Shared.Models.Payables.Invoices.LineItems;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices.Payments;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices.Taxes;
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

        [Route("listing")]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<VendorInvoiceToReadInList>>> GetList([FromQuery] ResourceParameters resourceParameters)
        {
            var result = await repository.GetInvoiceListAsync(resourceParameters);

            return result is null
                ? NotFound()
                : Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<VendorInvoiceToRead>>> Get([FromQuery] ResourceParameters resourceParameters)
        {
            var result = await repository.GetInvoices(resourceParameters);

            return result is null
                ? NotFound()
                : Ok(result);
        }

        [HttpGet("{id:long}", Name = "GetInvoiceAsync")]
        public async Task<ActionResult<VendorInvoiceToRead>> Get(long id)
        {
            var result = await repository.GetInvoiceAsync(id);

            return result is null
                ? NotFound()
                : Ok(result);
        }

        private async Task<(IReadOnlyList<Manufacturer> manufacturers, IReadOnlyList<SaleCode> salesCodes, Vendor vendor)> GetEntitiesForUpdate(VendorInvoiceToWrite invoiceFromCaller)
        {
            var manufacturersFromRepository = await GetManufacturersInInvoice(invoiceFromCaller);
            var salesCodesFromRepository = await GetSaleCodesInInvoice(invoiceFromCaller);
            var vendorFromRepository = await vendorRepository.GetVendorEntityAsync(invoiceFromCaller.Vendor.Id);

            return (manufacturersFromRepository, salesCodesFromRepository, vendorFromRepository);
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(long id, VendorInvoiceToWrite invoiceFromCaller)
        {
            var invoiceFromRepository = await repository.GetInvoiceEntityAsync(id);

            if (invoiceFromRepository is null)
                return NotFound($"Could not find Vendor Invoice to update with Id = {id}.");

            var (manufacturers, saleCodes, vendorFromRepository) = await GetEntitiesForUpdate(invoiceFromCaller);
            var vendorInvoiceNumbers = await repository.GetVendorInvoiceNumbers(invoiceFromCaller.Vendor.Id);

            var result = UpdateVendorInvoiceProperties(
                invoiceFromRepository, invoiceFromCaller, vendorInvoiceNumbers, vendorFromRepository);

            if (result.IsFailure)
                return BadRequest(result.Error);

            // TODO: VK Question: Is this how you would go about updating colections?
            var vendorInvoiceCollections = VendorInvoiceCollectionsFactory.Create(
                (IReadOnlyList<VendorInvoiceLineItemToWrite>)invoiceFromCaller.LineItems,
                (IReadOnlyList<VendorInvoicePaymentToWrite>)invoiceFromCaller.Payments,
                (IReadOnlyList<VendorInvoiceTaxToWrite>)invoiceFromCaller.Taxes,
                manufacturers,
                saleCodes);

            result = invoiceFromRepository.UpdateCollections(vendorInvoiceCollections);

            if (result.IsFailure)
                return BadRequest(result.Error);

            await repository.SaveChanges();

            return NoContent();
        }

        private Result UpdateVendorInvoiceProperties(
            VendorInvoice invoiceFromRepository,
            VendorInvoiceToWrite invoiceFromCaller,
            IReadOnlyList<string> vendorInvoiceNumbers,
            Vendor vendor)
        {
            return invoiceFromRepository.UpdateProperties(
                vendor,
                invoiceFromCaller.Status,
                invoiceFromCaller.DocumentType,
                invoiceFromCaller.DatePosted,
                invoiceFromCaller.Date,
                invoiceFromCaller.InvoiceNumber,
                vendorInvoiceNumbers,
                invoiceFromCaller.Total
                );
        }

        // POST: api/vendorinvoices
        [HttpPost]
        public async Task<ActionResult> Add(VendorInvoiceToWrite invoiceToAdd)
        {
            // TODO: How do we return the domain class error message?
            // TODO: Settle on error handling/messaging/logging
            var vendor = await vendorRepository.GetVendorEntityAsync(invoiceToAdd.Vendor.Id);

            if (vendor is null)
                return NotFound(
                    new ApiError
                    {
                        Message = $"Could not add new Invoice, Number: {invoiceToAdd.InvoiceNumber}. Vendor '{invoiceToAdd.Vendor.Name}' not found."
                    });

            var invoiceOrError = VendorInvoice.Create(
                vendor,
                invoiceToAdd.Status,
                invoiceToAdd.DocumentType,
                invoiceToAdd.Total,
                await repository.GetVendorInvoiceNumbers(vendor.Id),
                invoiceToAdd.InvoiceNumber,
                invoiceToAdd.Date,
                invoiceToAdd.DatePosted);

            if (invoiceOrError.IsFailure)
                return BadRequest(
                    new ApiError
                    {
                        Message = $"Could not add new Invoice Number '{invoiceToAdd.InvoiceNumber}': {invoiceOrError.Error}."
                    });

            VendorInvoice invoice = invoiceOrError.Value;
            VendorInvoiceItem vendorInvoiceItem;

            if (invoiceToAdd?.LineItems.Count > 0)
                foreach (var item in invoiceToAdd.LineItems)
                {
                    vendorInvoiceItem = VendorInvoiceItem.Create(
                                item.Item.PartNumber,
                                item.Item.Description,
                                item.Item.Manufacturer is null
                                    ? null
                                    : await manufacturerRepository.GetManufacturerEntityAsync(item.Item.Manufacturer.Id),
                                item.Item.SaleCode is null
                                    ? null
                                    : await saleCodeRepository.GetSaleCodeEntityAsync(item.Item.SaleCode.Id))
                            .Value;

                    invoice.AddLineItem(
                        VendorInvoiceLineItem.Create(
                            item.Type,
                            vendorInvoiceItem,
                            item.Quantity,
                            item.Cost,
                            item.Core,
                            item.PONumber,
                            item.TransactionDate)
                        .Value);
                };

            if (invoiceToAdd?.Payments.Count > 0)
                foreach (var payment in invoiceToAdd.Payments)
                    invoice.AddPayment(
                        VendorInvoicePayment.Create(
                            payment.PaymentMethod is null
                            ? null
                            : await paymentMethodRepository.GetPaymentMethodEntityAsync(payment.PaymentMethod.Id),
                            payment.Amount).Value);

            if (invoiceToAdd?.Taxes.Count > 0)
                foreach (var tax in invoiceToAdd.Taxes)
                {
                    // Don't save new, zero amount taxes sent by client
                    if (tax.Amount == 0)
                        continue;

                    invoice.AddTax(
                        VendorInvoiceTax.Create(
                            await salesTaxRepository.GetSalesTaxEntityAsync(tax.SalesTax.Id),
                            tax.Amount).Value);
                }

            repository.AddInvoice(invoice);
            await repository.SaveChanges();

            return Created(
                new Uri($"{BasePath}/{invoice.Id}",
                UriKind.Relative),
                new
                {
                    invoice.Id
                });
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            var invoiceFromRepository = await repository.GetInvoiceEntityAsync(id);

            if (invoiceFromRepository is null)
                return NotFound($"Could not find Vendor Invoice in the database to delete with Id: {id}.");

            repository.DeleteInvoice(invoiceFromRepository);

            await repository.SaveChanges();

            return NoContent();
        }

        private async Task<IReadOnlyList<Manufacturer>> GetManufacturersInInvoice(VendorInvoiceToWrite invoice)
        {
            return await manufacturerRepository.GetManufacturerEntitiesAsync(
                invoice.LineItems
                    .Where(
                        lineItem =>
                        lineItem is not null
                        && lineItem.Item is not null
                        && lineItem.Item.Manufacturer is not null)
                    .Select(lineItem => lineItem.Item.Manufacturer.Id)
                    .Distinct()
                    .ToList());
        }

        private async Task<IReadOnlyList<SaleCode>> GetSaleCodesInInvoice(VendorInvoiceToWrite invoice)
        {
            if (invoice is null || invoice.LineItems is null)
                return new List<SaleCode>();

            return await saleCodeRepository.GetSaleCodeEntitiesAsync(
                invoice.LineItems
                    .Where(lineItem =>
                        lineItem is not null
                        && lineItem.Item is not null
                        && lineItem.Item.SaleCode is not null)
                    .Select(lineItem => lineItem.Item.SaleCode.Id)
                    .Distinct()
                    .ToList());
        }

        private async Task<IReadOnlyList<Vendor>> GetVendorsInInvoice(VendorInvoiceToWrite invoice)
        {
            var result = new List<Vendor>() { await vendorRepository.GetVendorEntityAsync(invoice.Vendor.Id) };

            List<long> ids = new();

            foreach (var payment in invoice.Payments)
            {
                if (payment.PaymentMethod.ReconcilingVendor is not null)
                    ids.Add(payment.PaymentMethod.ReconcilingVendor.Id);
            }

            result.AddRange(
                await vendorRepository.GetVendorEntitiesAsync(ids));

            return result;
        }
    }
}
