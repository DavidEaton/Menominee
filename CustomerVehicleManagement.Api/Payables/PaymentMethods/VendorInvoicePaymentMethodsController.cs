using CustomerVehicleManagement.Api.Data;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices.Payments;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.Payables.PaymentMethods
{
    public class VendorInvoicePaymentMethodsController : ApplicationController
    {
        private readonly IVendorInvoicePaymentMethodRepository repository;

        public VendorInvoicePaymentMethodsController(IVendorInvoicePaymentMethodRepository repository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        // GET: api/vendorinvoicepaymentmethods/listing
        [Route("listing")]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<VendorInvoicePaymentMethodToReadInList>>> GetPaymentMethodListAsync()
        {
            var payMethods = await repository.GetPaymentMethodListAsync();

            if (payMethods == null)
                return NotFound();

            return Ok(payMethods);
        }

        // GET: api/vendorinvoicepaymentmethods/1
        [HttpGet("{id:long}", Name = "GetPaymentMethodAsync")]
        public async Task<ActionResult<VendorInvoicePaymentMethodToRead>> GetPaymentMethodAsync(long id)
        {
            var payMethod = await repository.GetPaymentMethodAsync(id);

            if (payMethod == null)
                return NotFound();

            return Ok(payMethod);
        }

        // PUT: api/vendorinvoicepaymentmethods/1
        [HttpPut("{id:long}")]
        public async Task<IActionResult> UpdatePaymentMethodAsync(VendorInvoicePaymentMethodToWrite paymentMethod, long id)
        {
            var notFoundMessage = $"Could not find Vendor Invoice Payment Method to update with Id = {id}.";

            if (!await repository.PaymentMethodExistsAsync(id))
                return NotFound(notFoundMessage);

            var paymentMethodFromRepository = await repository.GetPaymentMethodEntityAsync(id);
            if (paymentMethodFromRepository is null)
                return NotFound(notFoundMessage);

            paymentMethodFromRepository = VendorInvoicePaymentMethodHelper.ConvertWriteDtoToEntity(
                paymentMethod,
                await repository.GetPaymentMethodNamesAsync());

            paymentMethodFromRepository.SetTrackingState(TrackingState.Modified);
            repository.FixTrackingState();

            await repository.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/vendorinvoicepaymentmethods
        [HttpPost]
        public async Task<ActionResult<VendorInvoicePaymentMethodToRead>> AddPaymentMethodAsync(VendorInvoicePaymentMethodToWrite payMethodToAdd)
        {
            var payMethod = VendorInvoicePaymentMethodHelper.ConvertWriteDtoToEntity(
                payMethodToAdd,
                await repository.GetPaymentMethodNamesAsync());

            await repository.AddPaymentMethodAsync(payMethod);
            await repository.SaveChangesAsync();

            return CreatedAtRoute("GetPaymentMethodAsync",
                                  new { id = payMethod.Id },
                                  VendorInvoicePaymentMethodHelper.ConvertEntityToReadDto(payMethod));
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeletePaymentMethodAsync(long id)
        {
            var payMethodFromRepository = await repository.GetPaymentMethodEntityAsync(id);

            if (payMethodFromRepository == null)
                return NotFound($"Could not find Vendor Invoice Payment Method in the database to delete with Id: {id}.");

            repository.DeletePaymentMethod(payMethodFromRepository);
            await repository.SaveChangesAsync();

            return NoContent();
        }
    }
}
