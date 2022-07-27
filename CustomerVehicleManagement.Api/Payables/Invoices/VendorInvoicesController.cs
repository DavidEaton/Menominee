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
        public async Task<IActionResult> UpdateInvoiceAsync(long id, VendorInvoiceToWrite invoiceToWrite)
        {
            var notFoundMessage = $"Could not find Vendor Invoice to update with Id = {id}.";

            if (!await repository.InvoiceExistsAsync(id))
                return NotFound(notFoundMessage);

            // 1) Get domain entity from repository
            var invoice = repository.GetInvoiceEntityAsync(id).Result;

            if (invoice is null)
                return NotFound(notFoundMessage);

            // 2) Update domain entity with data in data transfer object(DTO)
            VendorInvoiceHelper.CopyWriteDtoToEntity(invoiceToWrite, invoice);

            // Update the objects ObjectState and sych the EF Change Tracker
            // 3) Set entity's TrackingState to Modified
            invoice.SetTrackingState(TrackingState.Modified);

            // 4) FixTrackingState: moves entity state tracking into the context
            repository.FixTrackingState();

            repository.UpdateInvoiceAsync(invoice);

            if (await repository.SaveChangesAsync())
                return NoContent();

            return BadRequest($"Failed to update vendor invoice.  Id = {invoiceToWrite.Id}.");
        }

        // POST: api/vendorinvoices
        [HttpPost]
        public async Task<ActionResult<VendorInvoiceToRead>> AddInvoiceAsync(VendorInvoiceToWrite invoiceToAdd)
        {
            var invoice = VendorInvoiceHelper.ConvertWriteDtoToEntity(invoiceToAdd);

            // 2. Add domain entity to repository
            await repository.AddInvoiceAsync(invoice);

            // 3. Save changes on repository
            await repository.SaveChangesAsync();

            // 4. Return new Id from database to consumer after save
            //return Created(new Uri($"{BasePath}/{invoice.Id}", UriKind.Relative), new { id = invoice.Id });
            return CreatedAtRoute("GetInvoiceAsync",
                                  new { id = invoice.Id },
                                  VendorInvoiceHelper.ConvertEntityToReadDto(invoice));
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
