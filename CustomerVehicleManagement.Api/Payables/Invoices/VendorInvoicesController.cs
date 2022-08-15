using CustomerVehicleManagement.Api.Data;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.Payables.Invoices
{
    public class VendorInvoicesController : ApplicationController
    {
        private readonly IVendorInvoiceRepository repository;
        private readonly string BasePath = "/api/vendorinvoices";

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
        public async Task<IActionResult> UpdateInvoiceAsync(long id, VendorInvoiceToWrite invoice)
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
            // VS:
            // name.Trim();
            // Encode(name);
            // SaluteName(name);
            // That is, we can tell just from the method signature what the code is doing
            // without the need to read and understand the details of command internals
            // that mutate our objects and return void. 
            // Functions that return a value describe all possible outputs in their signatures.
            // Commands that return void are dishonest in that their method signatures don't
            // tell the reader everything they can do, and hide their side effects.
            invoiceFromRepository = VendorInvoiceHelper.ConvertWriteDtoToEntity(invoice);

            invoiceFromRepository.SetTrackingState(TrackingState.Modified);
            repository.FixTrackingState();

            if (await repository.SaveChangesAsync())
                return NoContent();

            return BadRequest($"Failed to update vendor invoice.  Id = {id}.");
        }

        // POST: api/vendorinvoices
        [HttpPost]
        public async Task<ActionResult<VendorInvoiceToRead>> AddInvoiceAsync(VendorInvoiceToWrite invoiceToAdd)
        {
            var invoice = VendorInvoiceHelper.ConvertWriteDtoToEntity(invoiceToAdd);

            await repository.AddInvoiceAsync(invoice);
            await repository.SaveChangesAsync();

            return CreatedAtRoute("GetInvoiceAsync",
                                  new { id = invoice.Id },
                                  VendorInvoiceHelper.ConvertEntityToReadDto(invoice));
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeleteInvoiceAsync(long id)
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
