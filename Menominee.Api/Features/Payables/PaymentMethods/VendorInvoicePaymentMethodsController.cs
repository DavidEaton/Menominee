using Menominee.Api.Features.Payables.Vendors;
using Menominee.Domain.Entities.Payables;
using Menominee.Shared.Models.Http;
using Menominee.Shared.Models.Payables.Invoices.Payments;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menominee.Api.Features.Payables.PaymentMethods
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
            var result = await repository.GetListAsync();

            return result is null
                ? NotFound()
                : Ok(result);
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<VendorInvoicePaymentMethodToRead>> GetAsync(long id)
        {
            var result = await repository.GetAsync(id);

            return result is null
                ? NotFound()
                : Ok(result);
        }

        [HttpPut("{id:long}")]
        public async Task<ActionResult> UpdateAsync(VendorInvoicePaymentMethodRequest paymentMethodToUpdate)
        {
            var paymentMethodFromRepository = await repository.GetEntityAsync(paymentMethodToUpdate.Id);
            if (paymentMethodFromRepository is null)
                return NotFound($"Could not find Vendor Invoice Payment Method {paymentMethodToUpdate.Name} to update with Id = {paymentMethodToUpdate.Id}.");

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
                {
                    var reconcilingVendorResult = await vendorRepository.GetEntityAsync(reconcilingVendorFromCaller.Id);
                    if (reconcilingVendorResult.IsFailure)
                        return NotFound($"Could not find Reconciling Vendor '{reconcilingVendorFromCaller.Name}' with Id: {reconcilingVendorFromCaller.Id}.");
                }

                paymentMethodFromRepository.SetReconcilingVendor(reconcilingVendor);
            }

            await repository.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<PostResponse>> AddAsync(VendorInvoicePaymentMethodRequest payMethodToAdd)
        {
            var paymentMethodNames =
                await repository.GetPaymentMethodNamesAsync();

            var reconcilingVendorFromCaller =
                payMethodToAdd.ReconcilingVendor is not null
                ? payMethodToAdd.ReconcilingVendor
                : null;

            Vendor reconcilingVendor = null;

            if (reconcilingVendorFromCaller is not null)
            {
                var reconcilingVendorResult = await vendorRepository.GetEntityAsync(reconcilingVendorFromCaller.Id);
                if (reconcilingVendorResult.IsFailure)
                    return NotFound($"Could not find Reconciling Vendor '{reconcilingVendorFromCaller.Name}' with Id: {reconcilingVendorFromCaller.Id}.");
            }

            // No need to validate it here again, just call .Value right away
            var paymentMethod = VendorInvoicePaymentMethod.Create(
                paymentMethodNames,
                payMethodToAdd.Name,
                payMethodToAdd.IsActive,
                payMethodToAdd.PaymentType,
                reconcilingVendor).Value;

            repository.Add(paymentMethod);
            await repository.SaveChangesAsync();

            return Created(
               new Uri($"api/VendorInvoicePaymentMethodscontroller/{paymentMethod.Id}", UriKind.Relative),
               new { paymentMethod.Id });
        }

        [HttpDelete("{id:long}")]
        public async Task<ActionResult> DeleteAsync(long id)
        {
            var payMethodFromRepository = await repository.GetEntityAsync(id);

            if (payMethodFromRepository == null)
                return NotFound($"Could not find Vendor Invoice Payment Method in the database to delete with Id: {id}.");

            repository.Delete(payMethodFromRepository);
            await repository.SaveChangesAsync();

            return NoContent();
        }
    }
}
