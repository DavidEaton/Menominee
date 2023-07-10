using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Menominee.Api.Common;
using Menominee.Api.Payables.Vendors;
using Menominee.Domain.Entities.Payables;
using Menominee.Shared.Models.Payables.Invoices.Payments;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Menominee.Api.Payables.PaymentMethods
{
    public class VendorInvoicePaymentMethodsController : BaseApplicationController<VendorInvoicePaymentMethodsController>
    {
        private readonly IVendorInvoicePaymentMethodRepository repository;
        private readonly IVendorRepository vendorRepository;
        private readonly string BasePath = "/api/vendorinvoicepaymentmethods";

        public VendorInvoicePaymentMethodsController(
            IVendorInvoicePaymentMethodRepository repository
            , IVendorRepository vendorRepository
            , ILogger<VendorInvoicePaymentMethodsController> logger) : base(logger)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.vendorRepository = vendorRepository ?? throw new ArgumentNullException(nameof(vendorRepository));
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
        public async Task<IActionResult> UpdatePaymentMethodAsync(VendorInvoicePaymentMethodToWrite paymentMethodToUpdate, long id)
        {
            var notFoundMessage = $"Could not find Vendor Invoice Payment Method {paymentMethodToUpdate.Name} to update with Id = {id}.";

            var paymentMethodFromRepository = await repository.GetPaymentMethodEntityAsync(id);
            if (paymentMethodFromRepository is null)
                return NotFound(notFoundMessage);

            var paymentMethodNames = await repository.GetPaymentMethodNamesAsync();

            if (paymentMethodToUpdate.Name != paymentMethodFromRepository.Name)
                paymentMethodFromRepository.SetName(paymentMethodToUpdate.Name, paymentMethodNames);

            if (paymentMethodToUpdate.IsActive != paymentMethodFromRepository.IsActive)
            {
                if (paymentMethodToUpdate.IsActive)
                    paymentMethodFromRepository.Activate();

                if (!paymentMethodToUpdate.IsActive)
                    paymentMethodFromRepository.Deactivate();
            }

            if (paymentMethodToUpdate.PaymentType != paymentMethodFromRepository.PaymentType)
                paymentMethodFromRepository.SetPaymentType(paymentMethodToUpdate.PaymentType);

            if (paymentMethodToUpdate?.ReconcilingVendor?.Id != paymentMethodFromRepository?.ReconcilingVendor?.Id)
            {
                var reconcilingVendorFromCaller = paymentMethodToUpdate.ReconcilingVendor is not null ? paymentMethodToUpdate.ReconcilingVendor : null;
                Vendor reconcilingVendor = null;

                if (reconcilingVendorFromCaller is not null)
                    reconcilingVendor = await vendorRepository.GetVendorEntityAsync(reconcilingVendorFromCaller.Id);

                paymentMethodFromRepository.SetReconcilingVendor(reconcilingVendor);
            }

            await repository.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/vendorinvoicepaymentmethods
        [HttpPost]
        public async Task<ActionResult> AddPaymentMethodAsync(VendorInvoicePaymentMethodToWrite payMethodToAdd)
        {
            var paymentMethodNames =
                (IReadOnlyList<string>)await repository.GetPaymentMethodNamesAsync();

            var reconcilingVendorFromCaller = payMethodToAdd.ReconcilingVendor is not null ? payMethodToAdd.ReconcilingVendor : null;
            Vendor reconcilingVendor = null;

            if (reconcilingVendorFromCaller is not null)
                reconcilingVendor = await vendorRepository.GetVendorEntityAsync(reconcilingVendorFromCaller.Id);

            var paymentMethodOrError = VendorInvoicePaymentMethod.Create(
                paymentMethodNames,
                payMethodToAdd.Name,
                payMethodToAdd.IsActive,
                payMethodToAdd.PaymentType,
                reconcilingVendor);

            if (paymentMethodOrError.IsFailure)
                return BadRequest($"Could not add new Payment Method '{payMethodToAdd.Name}': {paymentMethodOrError.Error}.");

            await repository.AddPaymentMethodAsync(paymentMethodOrError.Value);
            await repository.SaveChangesAsync();

            return Created(
                new Uri($"{BasePath}/{paymentMethodOrError.Value.Id}",
                UriKind.Relative),
                new
                {
                    paymentMethodOrError.Value.Id
                });
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
