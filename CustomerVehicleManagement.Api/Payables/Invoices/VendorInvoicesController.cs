using CustomerVehicleManagement.Api.Data;
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

        public VendorInvoicesController(IVendorInvoiceRepository repository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
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
        public async Task<IActionResult> UpdateInvoiceAsync(long id, VendorInvoiceToWrite invoiceToEdit)
        {
            var notFoundMessage = $"Could not find Vendor Invoice to update with Id = {id}.";

            if (!await repository.InvoiceExistsAsync(id))
                return NotFound(notFoundMessage);

            // 1) Get domain entity from repository
            var invoice = repository.GetInvoiceEntityAsync(id).Result;

            if (invoice is null)
                return NotFound(notFoundMessage);

            // 2) Update domain entity with data in data transfer object(DTO)
            //invoice.Vendor = ????
            invoice.Date = invoiceToEdit.Date;
            invoice.DatePosted = invoiceToEdit.DatePosted;
            invoice.Status = invoiceToEdit.Status;
            invoice.InvoiceNumber = invoiceToEdit.InvoiceNumber;
            invoice.Total = invoiceToEdit.Total;

            if (invoiceToEdit?.LineItems?.Count > 0)
                invoice.SetItems(invoiceToEdit.LineItems.Select(item =>
                    new VendorInvoiceItem()
                    {
                        InvoiceId = item.InvoiceId,
                        Type = item.Type,
                        PartNumber = item.PartNumber,
                        MfrId = item.MfrId,
                        Description = item.Description,
                        Quantity = item.Quantity,
                        Cost = item.Cost,
                        Core = item.Core,
                        PONumber = item.PONumber,
                        InvoiceNumber = item.InvoiceNumber,
                        TransactionDate = item.TransactionDate
                    }).ToList());

            if (invoiceToEdit?.Payments?.Count > 0)
                invoice.SetPayments(invoiceToEdit.Payments.Select(payment =>
                    new VendorInvoicePayment()
                    {
                        InvoiceId = payment.InvoiceId,
                        PaymentMethod = payment.PaymentMethod,
                        Amount = payment.Amount
                    }).ToList());

            if (invoiceToEdit?.Taxes?.Count > 0)
                invoice.SetTaxes(invoiceToEdit.Taxes.Select(tax =>
                new VendorInvoiceTax()
                {
                    InvoiceId = tax.InvoiceId,
                    Order = tax.Order,
                    TaxId = tax.TaxId,
                    TaxName = tax.TaxName,
                    Amount = tax.Amount
                }).ToList());

            // Update the objects ObjectState and sych the EF Change Tracker
            // 3) Set entity's TrackingState to Modified
            invoice.SetTrackingState(TrackingState.Modified);

            // 4) FixTrackingState: moves entity state tracking into the context
            repository.FixTrackingState();

            repository.UpdateInvoiceAsync(invoice);

            /* Returning the updated resource is acceptible, for example:
                 return Ok(personFromRepository);
               even preferred over returning NoContent if updated resource
               contains properties that are mutated by the data store
               (which they are not in this case).

               Instead, our app will:
                 return NoContent();
               ... and let the caller decide whether to get the updated resource,
               which is also acceptible. The HTTP specification (RFC 2616) has a
               number of recommendations that are applicable:
            HTTP status code 200 OK for a successful PUT of an update to an existing resource. No response body needed.
            HTTP status code 201 Created for a successful PUT of a new resource
            HTTP status code 409 Conflict for a PUT that is unsuccessful due to a 3rd-party modification
            HTTP status code 400 Bad Request for an unsuccessful PUT
            */

            if (await repository.SaveChangesAsync())
                return NoContent();

            return BadRequest($"Failed to update vendor invoice.  Id = {invoiceToEdit.Id}.");
        }

        // POST: api/vendorinvoices
        [HttpPost]
        public async Task<ActionResult<VendorInvoiceToRead>> AddInvoiceAsync(VendorInvoiceToWrite invoiceToAdd)
        {
            /*
                Web API controllers don't have to check ModelState.IsValid if they have the
                [ApiController] attribute. In that case, an automatic HTTP 400 response containing
                error details is returned when model state is invalid.*/

            /* Controller Pattern:
                1. Convert data contract/data transfer object (dto) to domain entity
                2. Add domain entity to repository
                3. Save changes on repository
                4. Return to consumer */

            // 1. Convert dto to domain entity
            var invoice = new VendorInvoice();

            if (invoiceToAdd?.Date != null)
                invoice.Date = invoiceToAdd.Date;

            if (invoiceToAdd?.DatePosted != null)
                invoice.DatePosted = invoiceToAdd.DatePosted;

            invoice.Status = invoiceToAdd.Status;
            invoice.InvoiceNumber = invoiceToAdd.InvoiceNumber;
            invoice.Total = invoiceToAdd.Total;

            if (invoiceToAdd.VendorId > 0)
                invoice.Vendor = await repository.GetVendorAsync(invoiceToAdd.VendorId);

            if (invoiceToAdd?.LineItems != null)
                invoice.SetItems(invoiceToAdd?.LineItems
                                               .Select(item =>
                                               new VendorInvoiceItem()
                                               {
                                                   InvoiceId = item.InvoiceId,
                                                   Type = item.Type,
                                                   PartNumber = item.PartNumber,
                                                   MfrId = item.MfrId,
                                                   Description = item.Description,
                                                   Quantity = item.Quantity,
                                                   Cost = item.Cost,
                                                   Core = item.Core,
                                                   PONumber = item.PONumber,
                                                   InvoiceNumber = item.InvoiceNumber,
                                                   TransactionDate = (item?.TransactionDate != null) ? item.TransactionDate : null
                                               })
                                               .ToList());

            if (invoiceToAdd?.Payments != null)
                invoice.SetPayments(invoiceToAdd?.Payments
                                               .Select(payment =>
                                               new VendorInvoicePayment()
                                               {
                                                   InvoiceId = payment.InvoiceId,
                                                   PaymentMethod = payment.PaymentMethod,
                                                   Amount = payment.Amount
                                               })
                                               .ToList());

            if (invoiceToAdd?.Taxes != null)
                invoice.SetTaxes(invoiceToAdd?.Taxes
                                               .Select(tax =>
                                               new VendorInvoiceTax()
                                               {
                                                   InvoiceId = tax.InvoiceId,
                                                   Order = tax.Order,
                                                   TaxId = tax.TaxId,
                                                   TaxName = tax.TaxName,
                                                   Amount = tax.Amount
                                               })
                                               .ToList());

            // 2. Add domain entity to repository
            await repository.AddInvoiceAsync(invoice);

            // 3. Save changes on repository
            await repository.SaveChangesAsync();

            // 4. Return new Id from database to consumer after save
            //return Created(new Uri($"{BasePath}/{invoice.Id}", UriKind.Relative), new { id = invoice.Id });
            return CreatedAtRoute("GetInvoiceAsync",
                                  new { id = invoice.Id },
                                  VendorInvoiceHelper.CreateVendorInvoice(invoice));
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteInvoiceAsync(int id)
        {
            var invoiceFromRepository = await repository.GetInvoiceAsync(id);

            if (invoiceFromRepository == null)
                return NotFound($"Could not find Vendor Invoice in the database to delete with Id: {id}.");

            await repository.DeleteInvoiceAsync(id);

            if (await repository.SaveChangesAsync())
                return NoContent();

            return BadRequest($"Failed to delete Vendor Invoice with Id of {id}.");
        }
    }
}
