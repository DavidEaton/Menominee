using Menominee.Api.Common;
using Menominee.Api.Payables.Vendors;
using Menominee.Domain.Entities.Payables;
using Menominee.Shared.Models.Payables.Invoices.Payments;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menominee.Api.Payables.PaymentMethods
{
    public class VendorInvoicePaymentMethodsController : BaseApplicationController<VendorInvoicePaymentMethodsController>
    {
        private readonly IVendorInvoicePaymentMethodRepository repository;
        private readonly IVendorRepository vendorRepository;

        public VendorInvoicePaymentMethodsController(
            IVendorInvoicePaymentMethodRepository repository,
            IVendorRepository vendorRepository,
            ILogger<VendorInvoicePaymentMethodsController> logger) : base(logger)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.vendorRepository = vendorRepository ?? throw new ArgumentNullException(nameof(vendorRepository));
        }

        [Route("list")]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<VendorInvoicePaymentMethodToReadInList>>> GetListAsync()
        {
            var payMethods = await repository.GetPaymentMethodListAsync();

            if (payMethods == null)
                return NotFound();

            return Ok(payMethods);
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<VendorInvoicePaymentMethodToRead>> GetAsync(long id)
        {
            var payMethod = await repository.GetPaymentMethodAsync(id);

            if (payMethod == null)
                return NotFound();

            return Ok(payMethod);
        }

        [HttpPut("{id:long}")]
        public async Task<ActionResult> UpdateAsync(VendorInvoicePaymentMethodToWrite paymentMethodToUpdate, long id)
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

        [HttpPost]
        public async Task<ActionResult> AddAsync(VendorInvoicePaymentMethodToWrite payMethodToAdd)
        {
            var paymentMethodNames =
                await repository.GetPaymentMethodNamesAsync();

            var reconcilingVendorFromCaller = payMethodToAdd.ReconcilingVendor is not null ? payMethodToAdd.ReconcilingVendor : null;
            Vendor reconcilingVendor = null;

            if (reconcilingVendorFromCaller is not null)
                reconcilingVendor = await vendorRepository.GetVendorEntityAsync(reconcilingVendorFromCaller.Id);

            var result = VendorInvoicePaymentMethod.Create(
                paymentMethodNames,
                payMethodToAdd.Name,
                payMethodToAdd.IsActive,
                payMethodToAdd.PaymentType,
                reconcilingVendor);

            if (result.IsFailure)
                return BadRequest($"Could not add new Payment Method '{payMethodToAdd.Name}': {result.Error}.");

            await repository.AddPaymentMethodAsync(result.Value);
            await repository.SaveChangesAsync();

            return CreatedAtAction(
            nameof(GetAsync),
            new { id = result.Value.Id },
                new { result.Value.Id });

        }

        [HttpDelete("{id:long}")]
        public async Task<ActionResult> DeleteAsync(long id)
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
