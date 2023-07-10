using CSharpFunctionalExtensions;
using Menominee.Api.Common;
using Menominee.Api.Manufacturers;
using Menominee.Api.Payables.PaymentMethods;
using Menominee.Api.Payables.Vendors;
using Menominee.Api.SaleCodes;
using Menominee.Api.Taxes;
using Menominee.Domain.Entities;
using Menominee.Domain.Entities.Inventory;
using Menominee.Domain.Entities.Payables;
using Menominee.Shared.Models.Payables.Invoices;
using Menominee.Shared.Models.Payables.Invoices.LineItems;
using Menominee.Shared.Models.Payables.Invoices.Payments;
using Menominee.Shared.Models.Payables.Invoices.Taxes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Menominee.Api.Payables.Invoices
{
    public class VendorInvoicesController : BaseApplicationController<VendorInvoicesController>
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
            ISaleCodeRepository saleCodeRepository
            , ILogger<VendorInvoicesController> logger) : base(logger)
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
            var result = await repository.GetList(resourceParameters);

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
            var result = await repository.Get(id);

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
            var invoiceFromRepository = await repository.GetEntity(id);

            if (invoiceFromRepository is null)
                return NotFound($"Could not find Vendor Invoice to update with Id = {id}.");

            var (manufacturers, saleCodes, vendorFromRepository) = await GetEntitiesForUpdate(invoiceFromCaller);
            var vendorInvoiceNumbers = await repository.GetVendorInvoiceNumbers(invoiceFromCaller.Vendor.Id);

            var result = UpdateVendorInvoiceProperties(
                invoiceFromRepository, invoiceFromCaller, vendorInvoiceNumbers, vendorFromRepository);

            if (result.IsFailure)
                return BadRequest(result.Error);

            UpdateLineItems(invoiceFromCaller, invoiceFromRepository, manufacturers, saleCodes);
            await UpdatePayments(invoiceFromCaller, invoiceFromRepository);
            await UpdateTaxes(invoiceFromCaller, invoiceFromRepository);

            await repository.SaveChanges();
            return NoContent();
        }

        private async Task UpdateTaxes(VendorInvoiceToWrite invoiceFromCaller, VendorInvoice invoiceFromRepository)
        {
            foreach (var tax in invoiceFromRepository.Taxes)
            {
                var taxFromCaller = invoiceFromCaller.Taxes.FirstOrDefault(taxFromCaller => taxFromCaller.Id == tax.Id);

                if (taxFromCaller is null) continue;

                if (tax.Amount != taxFromCaller.Amount)
                    tax.SetAmount(taxFromCaller.Amount);

                if (tax.SalesTax.Id != taxFromCaller.SalesTax.Id)
                    tax.SetSalesTax(
                        await salesTaxRepository.GetSalesTaxEntityAsync(taxFromCaller.SalesTax.Id));
            }
        }

        private async Task UpdatePayments(VendorInvoiceToWrite invoiceFromCaller, VendorInvoice invoiceFromRepository)
        {
            foreach (var payment in invoiceFromRepository.Payments)
            {
                var paymentFromCaller = invoiceFromCaller.Payments.FirstOrDefault(paymentFromCaller => paymentFromCaller.Id == payment.Id);

                if (paymentFromCaller is null) continue;

                if (payment.Amount != paymentFromCaller.Amount)
                    payment.SetAmount(paymentFromCaller.Amount);

                if (payment.PaymentMethod.Id != paymentFromCaller.PaymentMethod.Id)
                    payment.SetPaymentMethod(
                        await paymentMethodRepository.GetPaymentMethodEntityAsync(paymentFromCaller.PaymentMethod.Id));
            }
        }

        private static void UpdateLineItems(VendorInvoiceToWrite invoiceFromCaller, VendorInvoice invoiceFromRepository, IReadOnlyList<Manufacturer> manufacturers, IReadOnlyList<SaleCode> saleCodes)
        {
            foreach (var line in invoiceFromRepository.LineItems)
            {
                var lineFromCaller = invoiceFromCaller.LineItems.FirstOrDefault(lineFromCaller => lineFromCaller.Id == line.Id);

                if (lineFromCaller is null) continue;

                if (line.TransactionDate != lineFromCaller.TransactionDate)
                    line.SetTransactionDate(lineFromCaller.TransactionDate);

                if (line.PONumber != lineFromCaller.PONumber)
                    line.SetPONumber(lineFromCaller.PONumber);

                if (line.Cost != lineFromCaller.Cost)
                    line.SetCost(lineFromCaller.Cost);

                if (line.Core != lineFromCaller.Core)
                    line.SetCore(lineFromCaller.Core);

                if (line.Quantity != lineFromCaller.Quantity)
                    line.SetQuantity(lineFromCaller.Quantity);

                if (line.Type != lineFromCaller.Type)
                    line.SetType(lineFromCaller.Type);

                if (line.Item.Description != lineFromCaller.Item.Description
                    || line.Item.PartNumber != lineFromCaller.Item.PartNumber
                    || line.Item.Manufacturer.Id != lineFromCaller.Item.Manufacturer.Id
                    || line.Item.SaleCode.Id != lineFromCaller.Item.SaleCode.Id)
                {
                    line.SetItem(
                        VendorInvoiceItem.Create(
                            lineFromCaller.Item.PartNumber,
                            lineFromCaller.Item.Description,
                            lineFromCaller.Item.Manufacturer is null
                            ? null
                            : manufacturers.FirstOrDefault(m => m.Id == lineFromCaller.Item.Manufacturer.Id),
                            lineFromCaller.Item.SaleCode is null
                            ? null
                            : saleCodes.FirstOrDefault(saleCode => saleCode.Id == lineFromCaller.Item.SaleCode.Id)
                        ).Value);
                }
            }
        }

        private static Result UpdateVendorInvoiceProperties(
            VendorInvoice invoiceFromRepository,
            VendorInvoiceToWrite invoiceFromCaller,
            IReadOnlyList<string> vendorInvoiceNumbers,
            Vendor vendor)
        {
            return Result.Combine(
                (vendor is not null) && (vendor.Id != invoiceFromRepository.Vendor.Id)
                    ? invoiceFromRepository.SetVendor(vendor)
                    : Result.Success(),
                (invoiceFromCaller.Status != invoiceFromRepository.Status)
                    ? invoiceFromRepository.SetStatus(invoiceFromCaller.Status)
                        : Result.Success(),
                (invoiceFromCaller.DocumentType != invoiceFromRepository.DocumentType)
                    ? invoiceFromRepository.SetDocumentType(invoiceFromCaller.DocumentType)
                    : Result.Success(),
                (invoiceFromCaller.DatePosted is not null) && (invoiceFromCaller.DatePosted != invoiceFromRepository.DatePosted)
                    ? invoiceFromRepository.SetDatePosted(invoiceFromCaller.DatePosted)
                        : Result.Success(),
                (invoiceFromCaller.Date is not null) && (invoiceFromCaller.Date != invoiceFromRepository.Date)
                    ? invoiceFromRepository.SetDate(invoiceFromCaller.Date)
                    : Result.Success(),
                (invoiceFromCaller.InvoiceNumber != invoiceFromRepository.InvoiceNumber)
                    ? invoiceFromRepository.SetInvoiceNumber(invoiceFromCaller.InvoiceNumber, vendorInvoiceNumbers)
                    : Result.Success(),
                (invoiceFromCaller.Total != invoiceFromRepository.Total)
                    ? invoiceFromRepository.SetTotal(invoiceFromCaller.Total)
                    : Result.Success());
        }

        // POST: api/vendorinvoices
        [HttpPost]
        public async Task<ActionResult> Add(VendorInvoiceToWrite invoiceToAdd)
        {
            var failureMessage = $"Could not add new Invoice, Number: {invoiceToAdd?.InvoiceNumber}. Vendor '{invoiceToAdd?.Vendor.Name}' not found.";

            var vendorFromRepository = await vendorRepository.GetVendorEntityAsync(invoiceToAdd.Vendor.Id);

            if (vendorFromRepository is null)
                return NotFound(new ApiError { Message = failureMessage });

            var manufacturers = await manufacturerRepository.GetManufacturerEntitiesAsync(
                invoiceToAdd.LineItems.Select(
                    lineItem =>
                    lineItem.Item.Manufacturer.Id)
                .ToList());

            var saleCodes = await saleCodeRepository.GetSaleCodeEntitiesAsync(
                invoiceToAdd.LineItems.Select(
                    lineItem =>
                    lineItem.Item.SaleCode.Id)
                .ToList());

            var invoiceEntity = VendorInvoiceHelper.ConvertWriteToEntity(
                invoiceToAdd,
                vendorFromRepository,
                VendorInvoiceLineItemHelper.ConvertWriteDtosToEntities(invoiceToAdd.LineItems, manufacturers, saleCodes),
                VendorInvoicePaymentHelper.ConvertWriteDtosToEntities(invoiceToAdd.Payments),
                VendorInvoiceTaxHelper.ConvertWriteDtosToEntities(invoiceToAdd.Taxes),
                await repository.GetVendorInvoiceNumbers(vendorFromRepository.Id));

            await repository.Add(invoiceEntity);
            await repository.SaveChanges();

            return Created(
                new Uri($"{BasePath}/{invoiceEntity.Id}", UriKind.Relative),
                new { invoiceEntity.Id });
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            var invoiceFromRepository = await repository.GetEntity(id);

            if (invoiceFromRepository is null)
                return NotFound($"Could not find Vendor Invoice in the database to delete with Id: {id}.");

            repository.Delete(invoiceFromRepository);

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
