using MenomineePlayWASM.Server.Repository.Payables;
using MenomineePlayWASM.Shared.Dtos.Payables.Invoices;
using MenomineePlayWASM.Shared.Entities.Payables.Invoices;
using MenomineePlayWASM.Shared.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MenomineePlayWASM.Server.Controllers.Payables.Vendors.Invoices
{
    [ApiController]
    [Route("api/payables/[controller]")]
    //[Route("api/payables/invoices")]
    public class VendorInvoicesController : ControllerBase
    {
        private readonly IVendorInvoiceRepository repository;
        private readonly string BasePath = "/api/payables/invoices";

        public VendorInvoicesController(IVendorInvoiceRepository repository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        // GET: api/payables/invoices/list
        [Route("listing")]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<VendorInvoiceToReadInList>>> GetInvoiceListAsync()
        {
            var invoices = await repository.GetInvoiceListAsync();

            //if (invoices == null)
            //    return NotFound();

            return Ok(invoices);
        }

        // GET: api/payables/invoices
        //[HttpGet]
        //public async Task<ActionResult<List<VendorInvoiceToRead>>> GetInvoicesAsync()
        //{
        //    var invoices = await repository.GetInvoicesAsync();

        //    //if (invoices == null)
        //    //    return NotFound();

        //    return Ok(invoices);
        //}

        // GET: api/payables/invoices/1
        [HttpGet("{id:long}", Name = "GetInvoiceAsync")]
        public async Task<ActionResult<VendorInvoiceToRead>> GetInvoiceAsync(long id)
        {
            var invoice = await repository.GetInvoiceAsync(id);

            //if (invoice == null)
            //    return NotFound();

            return Ok(invoice);
        }

        // PUT: api/payables/invoices/1
        [HttpPut("{id:long}")]
        public async Task<IActionResult> UpdateInvoiceAsync(long id, VendorInvoiceToWrite invoiceToEdit)
        {
            if (!await repository.InvoiceExistsAsync(id))
                return NotFound($"Could not find Vendor Invoice to update with Id = {id}.");

            // 1) Get domain entity from repository
            var invoice = repository.GetInvoiceEntityAsync(id).Result;

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

        // POST: api/payables/invoices
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
            return Created(new Uri($"{BasePath}/{invoice.Id}", UriKind.Relative), new { id = invoice.Id });
        }

        //// POST: api/payables/invoices
        //[HttpPost]
        //public async Task<ActionResult<VendorInvoiceReadDto>> CreateInvoiceAsync(VendorInvoiceCreateDto invoiceCreateDto)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);

        //    VendorInvoice invoice = CreateNewInvoice(invoiceCreateDto);

        //    await repository.AddInvoiceAsync(invoice);

        //    if (await repository.SaveChangesAsync())
        //    {
        //        var invoiceFromRepository = repository.GetInvoiceAsync(invoice.Id).Result;

        //        return CreatedAtRoute("GetInvoiceAsync", new { id = invoice.Id }, invoiceFromRepository);
        //    }

        //    return BadRequest("Failed to add Vendor Invoice.");
        //}

        //private static VendorInvoice CreateNewInvoice(VendorInvoiceCreateDto invoiceCreateDto)
        //{
        //    var invoice = new VendorInvoice();

        //    invoice.Vendor.VendorId = invoiceCreateDto.VendorId;
        //    invoice.Date = invoiceCreateDto.Date;
        //    invoice.DatePosted = invoiceCreateDto.DatePosted;
        //    invoice.Status = invoiceCreateDto.Status;
        //    invoice.InvoiceNumber = invoiceCreateDto.InvoiceNumber;
        //    invoice.Total = invoiceCreateDto.Total;

        //    if (invoiceCreateDto?.LineItems?.Count > 0)
        //        foreach (var item in invoiceCreateDto.LineItems)
        //            invoice.AddItem(VendorInvoiceItemDtoHelper.CreateDtoToEntity(item));

        //    if (invoiceCreateDto?.Payments?.Count > 0)
        //        foreach (var payment in invoiceCreateDto.Payments)
        //            invoice.AddPayment(VendorInvoicePaymentDtoHelper.CreateDtoToEntity(payment));

        //    if (invoiceCreateDto?.Taxes?.Count > 0)
        //        foreach (var tax in invoiceCreateDto.Taxes)
        //            invoice.AddTax(VendorInvoiceTaxDtoHelper.CreateDtoToEntity(tax));

        //    return invoice;
        //}

        ////[HttpPut]
        ////public async Task<ActionResult> Put(VendorInvoiceReadDto vendorInvoice)
        ////{
        ////    context.Entry(vendorInvoice).State = EntityState.Modified;

        ////    foreach (var item in vendorInvoice.LineItems)
        ////    {
        ////        if (item.Id != 0)
        ////            context.Entry(item).State = EntityState.Modified;
        ////        else
        ////            context.Entry(item).State = EntityState.Added;
        ////    }
        ////    foreach (var tax in vendorInvoice.Taxes)
        ////    {
        ////        if (tax.Id != 0)
        ////            context.Entry(tax).State = EntityState.Modified;
        ////        else
        ////            context.Entry(tax).State = EntityState.Added;
        ////    }
        ////    foreach (var payment in vendorInvoice.Payments)
        ////    {
        ////        if (payment.Id != 0)
        ////            context.Entry(payment).State = EntityState.Modified;
        ////        else
        ////            context.Entry(payment).State = EntityState.Added;
        ////    }

        ////    var itemIds = vendorInvoice.LineItems.Select(x => x.Id).ToList();
        ////    var itemsToDelete = await context
        ////        .VendorInvoiceItems
        ////        .Where(x => !itemIds.Contains(x.Id) && x.InvoiceId == vendorInvoice.Id)
        ////        .ToListAsync();
        ////    context.RemoveRange(itemsToDelete);

        ////    var taxIds = vendorInvoice.Taxes.Select(x => x.Id).ToList();
        ////    var taxesToDelete = await context
        ////        .VendorInvoiceTaxes
        ////        .Where(x => !taxIds.Contains(x.Id) && x.InvoiceId == vendorInvoice.Id)
        ////        .ToListAsync();
        ////    context.RemoveRange(taxesToDelete);

        ////    var paymentIds = vendorInvoice.Payments.Select(x => x.Id).ToList();
        ////    var paymentsToDelete = await context
        ////        .VendorInvoicePayments
        ////        .Where(x => !paymentIds.Contains(x.Id) && x.InvoiceId == vendorInvoice.Id)
        ////        .ToListAsync();
        ////    context.RemoveRange(paymentsToDelete);

        ////    await context.SaveChangesAsync();

        ////    return NoContent();
        ////}


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
