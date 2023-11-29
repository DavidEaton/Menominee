using CSharpFunctionalExtensions;
using Menominee.Api.Common;
using Menominee.Api.Features.Manufacturers;
using Menominee.Api.Features.Payables.PaymentMethods;
using Menominee.Api.Features.Payables.Vendors;
using Menominee.Api.Features.SaleCodes;
using Menominee.Api.Features.Taxes;
using Menominee.Common.Http;
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

namespace Menominee.Api.Features.Payables.Invoices
{
    public class VendorInvoicesController : BaseApplicationController<VendorInvoicesController>
    {
        private readonly IVendorInvoiceRepository repository;
        private readonly IVendorRepository vendorRepository;
        private readonly IVendorInvoicePaymentMethodRepository paymentMethodRepository;
        private readonly ISalesTaxRepository salesTaxRepository;
        private readonly IManufacturerRepository manufacturerRepository;
        private readonly ISaleCodeRepository saleCodeRepository;

        public VendorInvoicesController(
            IVendorInvoiceRepository repository,
            IVendorRepository vendorRepository,
            IVendorInvoicePaymentMethodRepository paymentMethodRepository,
            ISalesTaxRepository salesTaxRepository,
            IManufacturerRepository manufacturerRepository,
            ISaleCodeRepository saleCodeRepository,
            ILogger<VendorInvoicesController> logger) : base(logger)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.vendorRepository = vendorRepository ?? throw new ArgumentNullException(nameof(vendorRepository)); ;
            this.paymentMethodRepository = paymentMethodRepository ?? throw new ArgumentNullException(nameof(paymentMethodRepository));
            this.salesTaxRepository = salesTaxRepository ?? throw new ArgumentNullException(nameof(salesTaxRepository));
            this.manufacturerRepository = manufacturerRepository ?? throw new ArgumentNullException(nameof(manufacturerRepository));
            this.saleCodeRepository = saleCodeRepository ?? throw new ArgumentNullException(nameof(saleCodeRepository));
        }

        [HttpGet("listing")]
        public async Task<ActionResult<IReadOnlyList<VendorInvoiceToReadInList>>> GetListByParametersAsync
            ([FromQuery] ResourceParameters resourceParameters)
        {
            var result = await repository.GetListByParametersAsync(resourceParameters);

            return result is null
                ? NotFound()
                : Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<VendorInvoiceToRead>>> GetByParametersAsync([FromQuery] ResourceParameters resourceParameters)
        {
            var result = await repository.GetByParametersAsync(resourceParameters);

            return result is null
                ? NotFound()
                : Ok(result);
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<VendorInvoiceToRead>> GetAsync(long id)
        {
            var result = await repository.GetAsync(id);

            return result is null
                ? NotFound()
                : Ok(result);
        }

        [HttpPut("{id:long}")]
        public async Task<ActionResult> UpdateAsync(VendorInvoiceToWrite invoiceFromCaller)
        {
            var invoiceFromRepository = await repository.GetEntityAsync(invoiceFromCaller.Id);

            if (invoiceFromRepository is null)
                return NotFound($"Could not find Invoice to update with Id = {invoiceFromCaller.Id}.");

            var containedEntitiesResult = await GetContainedEntitiesAsync(invoiceFromCaller);

            if (containedEntitiesResult.IsFailure)
                return BadRequest(containedEntitiesResult.Error);

            var (manufacturers, saleCodes, vendorFromRepository) = containedEntitiesResult.Value;

            var vendorInvoiceNumbers = await repository.GetVendorInvoiceNumbersAsync(invoiceFromCaller.Vendor.Id);

            var result = UpdateVendorInvoiceProperties(
                invoiceFromRepository, invoiceFromCaller, vendorInvoiceNumbers, vendorFromRepository);

            if (result.IsFailure)
                return BadRequest(result.Error);

            UpdateLineItemsAsync(invoiceFromCaller, invoiceFromRepository, manufacturers, saleCodes);
            await UpdatePaymentsAsync(invoiceFromCaller, invoiceFromRepository);
            await UpdateTaxesAsync(invoiceFromCaller, invoiceFromRepository);

            await repository.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<PostResponse>> AddAsync(VendorInvoiceToWrite invoiceToAdd)
        {
            var failureMessage = $"Could not add new Invoice, Number: {invoiceToAdd?.InvoiceNumber}. Vendor '{invoiceToAdd?.Vendor.Name}' not found.";

            var result = await vendorRepository.GetEntityAsync(invoiceToAdd.Vendor.Id);
            if (result.IsFailure)
                // TODO: log exception here
                return NotFound(new ApiError { Message = $"{failureMessage} - {result.Error}" });

            var vendorFromRepository = result.Value;

            var manufacturersResult = await manufacturerRepository.GetEntitiesAsync(
                invoiceToAdd.LineItems
                    .Select(lineItem => lineItem.Item.Manufacturer.Id)
                    .ToList());

            if (manufacturersResult.IsFailure)
                // TODO: log exception here
                return NotFound(new ApiError { Message = manufacturersResult.Error });


            var manufacturers = manufacturersResult.Value;

            var saleCodesResult = await saleCodeRepository.GetSaleCodeEntitiesAsync(
                invoiceToAdd.LineItems
                    .Select(lineItem => lineItem.Item.SaleCode.Id)
                    .ToList());

            if (saleCodesResult.IsFailure)
                // TODO: log exception here
                return NotFound(new ApiError { Message = saleCodesResult.Error });

            var saleCodes = saleCodesResult.Value;

            // No need to validate it here again, just call .Value right away
            var invoice = VendorInvoiceHelper.ConvertWriteToEntity(
                invoiceToAdd,
                vendorFromRepository,
                VendorInvoiceLineItemHelper.ConvertWriteDtosToEntities(invoiceToAdd.LineItems, manufacturers, saleCodes),
                VendorInvoicePaymentHelper.ConvertWriteDtosToEntities(invoiceToAdd.Payments),
                VendorInvoiceTaxHelper.ConvertWriteDtosToEntities(invoiceToAdd.Taxes),
                await repository.GetVendorInvoiceNumbersAsync(vendorFromRepository.Id));

            repository.Add(invoice);
            await repository.SaveChangesAsync();

            return Created(
               new Uri($"api/vendorinvoicescontroller/{invoice.Id}", UriKind.Relative),
               new { invoice.Id });
        }

        private async Task<Result<(IReadOnlyList<Manufacturer>, IReadOnlyList<SaleCode>, Vendor)>> GetContainedEntitiesAsync(VendorInvoiceToWrite invoiceFromCaller)
        {
            var manufacturersResult = await GetManufacturersInInvoiceAsync(invoiceFromCaller);
            var salesCodesResult = await GetSaleCodesInInvoiceAsync(invoiceFromCaller);
            var vendorResult = await vendorRepository.GetEntityAsync(invoiceFromCaller.Vendor.Id);

            if (manufacturersResult.IsFailure)
                return Result.Failure<(IReadOnlyList<Manufacturer>, IReadOnlyList<SaleCode>, Vendor)>("Failed to get manufacturers.");

            if (salesCodesResult.IsFailure)
                return Result.Failure<(IReadOnlyList<Manufacturer>, IReadOnlyList<SaleCode>, Vendor)>("Failed to get sale codes.");

            if (vendorResult.IsFailure)
                return Result.Failure<(IReadOnlyList<Manufacturer>, IReadOnlyList<SaleCode>, Vendor)>("Failed to get vendor.");

            return Result.Success((manufacturersResult.Value, salesCodesResult.Value, vendorResult.Value));
        }

        private async Task UpdateTaxesAsync(VendorInvoiceToWrite invoiceFromCaller, VendorInvoice invoiceFromRepository)
        {
            foreach (var tax in invoiceFromRepository.Taxes)
            {
                var taxFromCaller = invoiceFromCaller.Taxes.FirstOrDefault(taxFromCaller => taxFromCaller.Id == tax.Id);

                if (taxFromCaller is null) continue;

                if (tax.Amount != taxFromCaller.Amount)
                    tax.SetAmount(taxFromCaller.Amount);

                if (tax.SalesTax.Id != taxFromCaller.SalesTax.Id)
                    tax.SetSalesTax(
                        await salesTaxRepository.GetEntityAsync(taxFromCaller.SalesTax.Id));
            }
        }

        private async Task UpdatePaymentsAsync(VendorInvoiceToWrite invoiceFromCaller, VendorInvoice invoiceFromRepository)
        {
            foreach (var payment in invoiceFromRepository.Payments)
            {
                var paymentFromCaller = invoiceFromCaller.Payments.FirstOrDefault(paymentFromCaller => paymentFromCaller.Id == payment.Id);

                if (paymentFromCaller is null) continue;

                if (payment.Amount != paymentFromCaller.Amount)
                    payment.SetAmount(paymentFromCaller.Amount);

                if (payment.PaymentMethod.Id != paymentFromCaller.PaymentMethod.Id)
                    payment.SetPaymentMethod(
                        await paymentMethodRepository.GetEntityAsync(paymentFromCaller.PaymentMethod.Id));
            }
        }

        private static void UpdateLineItemsAsync(VendorInvoiceToWrite invoiceFromCaller, VendorInvoice invoiceFromRepository, IReadOnlyList<Manufacturer> manufacturers, IReadOnlyList<SaleCode> saleCodes)
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
            // TODO: We don't need to validate the vendor here, it was already
            // validated in the asp.net request pipeline
            return Result.Combine(
                vendor is not null && vendor.Id != invoiceFromRepository.Vendor.Id
                    ? invoiceFromRepository.SetVendor(vendor)
                    : Result.Success(),
                invoiceFromCaller.Status != invoiceFromRepository.Status
                    ? invoiceFromRepository.SetStatus(invoiceFromCaller.Status)
                        : Result.Success(),
                invoiceFromCaller.DocumentType != invoiceFromRepository.DocumentType
                    ? invoiceFromRepository.SetDocumentType(invoiceFromCaller.DocumentType)
                    : Result.Success(),
                invoiceFromCaller.DatePosted is not null && invoiceFromCaller.DatePosted != invoiceFromRepository.DatePosted
                    ? invoiceFromRepository.SetDatePosted(invoiceFromCaller.DatePosted)
                        : Result.Success(),
                invoiceFromCaller.Date is not null && invoiceFromCaller.Date != invoiceFromRepository.Date
                    ? invoiceFromRepository.SetDate(invoiceFromCaller.Date)
                    : Result.Success(),
                invoiceFromCaller.InvoiceNumber != invoiceFromRepository.InvoiceNumber
                    ? invoiceFromRepository.SetInvoiceNumber(invoiceFromCaller.InvoiceNumber, vendorInvoiceNumbers)
                    : Result.Success(),
                invoiceFromCaller.Total != invoiceFromRepository.Total
                    ? invoiceFromRepository.SetTotal(invoiceFromCaller.Total)
                    : Result.Success());
        }

        private async Task<Result<IReadOnlyList<Manufacturer>>> GetManufacturersInInvoiceAsync(VendorInvoiceToWrite invoice)
        {
            try
            {
                var manufacturerIds = invoice.LineItems
                                            .Where(lineItem => lineItem?.Item?.Manufacturer != null)
                                            .Select(lineItem => lineItem.Item.Manufacturer.Id)
                                            .Distinct()
                                            .ToList();

                if (!manufacturerIds.Any())
                {
                    return Result.Failure<IReadOnlyList<Manufacturer>>("No manufacturers found in the invoice.");
                }

                var manufacturersResult = await manufacturerRepository.GetEntitiesAsync(manufacturerIds);

                if (manufacturersResult.IsFailure)
                {
                    return Result.Failure<IReadOnlyList<Manufacturer>>(manufacturersResult.Error);
                }

                return manufacturersResult;
            }
            catch (Exception ex)
            {
                // TODO: Log the exception here
                return Result.Failure<IReadOnlyList<Manufacturer>>($"An error occurred while fetching manufacturers: {ex.Message}");
            }
        }

        private async Task<Result<IReadOnlyList<SaleCode>>> GetSaleCodesInInvoiceAsync(VendorInvoiceToWrite invoice)
        {
            try
            {
                if (invoice is null || invoice.LineItems is null)
                {
                    return Result.Failure<IReadOnlyList<SaleCode>>("Invoice or line items are null.");
                }

                var saleCodes = await saleCodeRepository.GetSaleCodeEntitiesAsync(
                    invoice.LineItems
                        .Where(lineItem =>
                            lineItem is not null
                            && lineItem.Item is not null
                            && lineItem.Item.SaleCode is not null)
                        .Select(lineItem => lineItem.Item.SaleCode.Id)
                        .Distinct()
                        .ToList());

                if (saleCodes.IsFailure)
                {
                    return saleCodes;
                }

                if (!saleCodes.Value.Any())
                {
                    return Result.Failure<IReadOnlyList<SaleCode>>("No sale codes found.");
                }

                return Result.Success(saleCodes.Value);
            }
            catch (Exception ex)
            {
                // TODO: log the exception here
                return Result.Failure<IReadOnlyList<SaleCode>>($"An error occurred while fetching sale codes: {ex.Message}");
            }
        }
    }
}