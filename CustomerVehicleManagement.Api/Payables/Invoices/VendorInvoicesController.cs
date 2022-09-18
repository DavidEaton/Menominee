using CustomerVehicleManagement.Api.Data;
using CustomerVehicleManagement.Api.Payables.PaymentMethods;
using CustomerVehicleManagement.Api.Payables.Vendors;
using CustomerVehicleManagement.Domain.Entities.Payables;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices;
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
        private readonly string BasePath = "/api/vendorinvoices";

        public VendorInvoicesController(
            IVendorInvoiceRepository repository,
            IVendorRepository vendorRepository,
            IVendorInvoicePaymentMethodRepository paymentMethodRepository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.vendorRepository = vendorRepository ?? throw new ArgumentNullException(nameof(vendorRepository)); ;
            this.paymentMethodRepository = paymentMethodRepository ?? throw new ArgumentNullException(nameof(paymentMethodRepository));
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

            // Favor functions that return a value over commands that mutate objects and
            // hide their side effects. Much easier to reason about and fix bugs when
            // methods signatures are "honest", especially when we can see the chain of
            // return values in a set of steps. For example, 
            // var trimmed = name.Trim();
            // var encoded = Encode(trimmed);
            // var salutedName = SaluteName(encoded);
            // vs.
            // name.Trim();
            // Encode(name);
            // SaluteName(name);
            // That is, we can tell just from the method signature what the code is doing
            // without the need to read and understand the details of command internals
            // that mutate our objects and return void. 
            // Functions that return a value describe all possible outputs in their signatures.
            // Commands that return void are dishonest in that their method signatures don't
            // tell the reader everything they can do, and hide their side effects.

            // Update each member of VendorInvoice
            //VendorInvoice
            var result = invoiceFromRepository.SetVendor(await vendorRepository.GetVendorEntityAsync(invoiceFromCaller.Vendor.Id));
            invoiceFromRepository.SetVendorInvoiceStatus(invoiceFromCaller.Status);
            invoiceFromRepository.SetTotal(invoiceFromCaller.Total);
            invoiceFromRepository.SetInvoiceNumber(invoiceFromCaller.InvoiceNumber);
            invoiceFromRepository.SetDate(invoiceFromCaller.Date);
            invoiceFromRepository.SetDatePosted(invoiceFromCaller.DatePosted);

            //IList<VendorInvoiceLineItem> LineItems
            foreach (var lineItem in invoiceFromRepository?.LineItems)
            {
                // Added
                if (lineItem.Id == 0 && lineItem.TrackingState == TrackingState.Added)
                    invoiceFromRepository.AddLineItem(
                        VendorInvoiceLineItem.Create(
                            lineItem.Type, lineItem.Item, lineItem.Quantity, lineItem.Cost, lineItem.Core, lineItem.PONumber, lineItem.TransactionDate)
                        .Value);
                // Updated
                if (lineItem.Id != 0 && lineItem.TrackingState == TrackingState.Modified)
                {
                    var contextLineItem = invoiceFromRepository?.LineItems.FirstOrDefault(contextLineItem => contextLineItem.Id == lineItem.Id);
                    contextLineItem.SetType(lineItem.Type);
                    contextLineItem.SetItem(lineItem.Item);
                    contextLineItem.SetQuantity(lineItem.Quantity);
                    contextLineItem.SetCost(lineItem.Cost);
                    contextLineItem.SetCore(lineItem.Core);
                    contextLineItem.SetPONumber(lineItem.PONumber);
                    contextLineItem.SetTransactionDate(lineItem.TransactionDate);
                    contextLineItem.SetTrackingState(TrackingState.Modified);
                }
                // Deleted
                if (lineItem.Id != 0 && lineItem.TrackingState == TrackingState.Deleted)
                    invoiceFromRepository.RemoveLineItem(
                        invoiceFromRepository.LineItems.FirstOrDefault(
                            contextLineItem =>
                            contextLineItem.Id == lineItem.Id));
            }
            //IList<VendorInvoicePayment> Payments
            foreach (var payment in invoiceFromRepository?.Payments)
            {
                // Added
                if (payment.Id == 0 && payment.TrackingState == TrackingState.Added)
                    invoiceFromRepository.AddPayment(
                        VendorInvoicePayment.Create(payment.PaymentMethod, payment.Amount).Value);
                // Updated
                if (payment.Id != 0 && payment.TrackingState == TrackingState.Modified)
                {
                    var contextPayment = invoiceFromRepository?.Payments.FirstOrDefault(contextPayment => contextPayment.Id == payment.Id);
                    contextPayment.SetPaymentMethod(payment.PaymentMethod);
                    contextPayment.SetAmount(payment.Amount);
                    contextPayment.SetTrackingState(TrackingState.Modified);
                }
                // Deleted
                if (payment.Id != 0 && payment.TrackingState == TrackingState.Deleted)
                    invoiceFromRepository.RemovePayment(
                        invoiceFromRepository.Payments.FirstOrDefault(
                            contextPayment =>
                            contextPayment.Id == payment.Id));
            }
            //IList<VendorInvoiceTax> Taxes
            foreach (var tax in invoiceFromRepository?.Taxes)
            {
                // Added
                if (tax.Id == 0 && tax.TrackingState == TrackingState.Added)
                    invoiceFromRepository.AddTax(
                        VendorInvoiceTax.Create(tax.SalesTax, tax.TaxId).Value);
                // Updated
                if (tax.Id != 0 && tax.TrackingState == TrackingState.Modified)
                {
                    var contextTax = invoiceFromRepository?.Taxes.FirstOrDefault(contextTax => contextTax.Id == tax.Id);
                    contextTax.SetSalesTax(tax.SalesTax);
                    contextTax.SetTaxId(tax.TaxId);
                    contextTax.SetTrackingState(TrackingState.Modified);
                }
                // Deleted
                if (tax.Id != 0 && tax.TrackingState == TrackingState.Deleted)
                    invoiceFromRepository.RemoveTax(
                        invoiceFromRepository.Taxes.FirstOrDefault(
                            contextTax =>
                            contextTax.Id == tax.Id));
            }
            invoiceFromRepository.SetTrackingState(TrackingState.Modified);

            repository.FixTrackingState();

            await repository.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/vendorinvoices
        [HttpPost]
        public async Task<ActionResult<VendorInvoiceToRead>> AddInvoiceAsync(VendorInvoiceToWrite invoiceToAdd)
        {
            var invoice = VendorInvoiceHelper.ConvertWriteDtoToEntity(
                invoiceToAdd,
                await paymentMethodRepository.GetPaymentMethodsAsync());

            await repository.AddInvoiceAsync(invoice);
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
    }
}
