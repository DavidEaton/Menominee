using Menominee.Api.Features.Contactables;
using Menominee.Api.Features.Payables.PaymentMethods;
using Menominee.Domain.Entities.Payables;
using Menominee.Shared.Models.Http;
using Menominee.Shared.Models.Payables.Invoices.Payments;
using Menominee.Shared.Models.Payables.Vendors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menominee.Api.Features.Payables.Vendors
{
    public class VendorsController : BaseApplicationController<VendorsController>
    {
        private readonly IVendorRepository repository;
        private readonly IVendorInvoicePaymentMethodRepository paymentMethodRepository;

        public VendorsController(
            IVendorRepository repository,
            IVendorInvoicePaymentMethodRepository paymentMethodRepository,
            ILogger<VendorsController> logger) : base(logger)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.paymentMethodRepository = paymentMethodRepository ?? throw new ArgumentNullException(nameof(paymentMethodRepository));
        }

        [HttpGet("list")]
        public async Task<ActionResult<IReadOnlyList<VendorToRead>>> GetAsync()
        {
            var result = await repository.GetAllAsync();

            return result is null
                ? NotFound()
                : Ok(result);
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<VendorToRead>> GetAsync(long id)
        {
            var result = await repository.GetAsync(id);

            return result is null
                ? NotFound()
                : Ok(result);
        }

        [HttpPut("{id:long}")]
        public async Task<ActionResult> UpdateVendorAsync(VendorToWrite vendorFromCaller)
        {
            var result = await repository.GetEntityAsync(vendorFromCaller.Id);

            if (result.IsFailure)
                return NotFound($"Could not find Vendor to update: {vendorFromCaller.Name}");

            var vendorFromRepository = result.Value;

            await UpdateVendor(vendorFromCaller, vendorFromRepository);

            UpdateContactDetails(vendorFromCaller, vendorFromRepository);

            await repository.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<PostResponse>> AddAsync(VendorToWrite vendorToAdd)
        {
            // no need to validate it here again, just call .Value right away
            var vendor = Vendor.Create(
                vendorToAdd.Name,
                vendorToAdd.VendorCode.ToUpper(),
                vendorToAdd.VendorRole,
                vendorToAdd.Notes).Value;

            if (vendorToAdd?.DefaultPaymentMethod?.PaymentMethod is not null)
            {
                var paymentMethod = await paymentMethodRepository.GetEntityAsync(
                    vendorToAdd.DefaultPaymentMethod.PaymentMethod.Id);

                if (paymentMethod is null)
                    return NotFound($"Could not add new Vendor '{vendorToAdd.Name}'. Vendor default payment method {vendorToAdd.DefaultPaymentMethod.PaymentMethod.Name} was not found.");

                var setDefaultPaymentMethodResult = vendor.SetDefaultPaymentMethod(DefaultPaymentMethod.Create(paymentMethod, vendorToAdd.DefaultPaymentMethod.AutoCompleteDocuments).Value);

                if (setDefaultPaymentMethodResult.IsFailure)
                    return NotFound($"Could not add new Vendor '{vendorToAdd.Name}': {setDefaultPaymentMethodResult.Error}");
            }

            vendor.UpdateContactDetails(ContactDetailsFactory.Create(
                vendorToAdd.Phones, vendorToAdd.Emails, vendorToAdd.Address).Value);

            repository.Add(vendor);
            await repository.SaveChangesAsync();

            return Created(new Uri($"/api/vendorsController/{vendor.Id}",
                UriKind.Relative),
                new { vendor.Id });
        }

        [HttpDelete("{id:long}")]
        public async Task<ActionResult> DeleteVendorAsync(long id)
        {
            var result = await repository.GetEntityAsync(id);

            if (result.IsFailure)
                return NotFound($"Failed to delete Vendor with Id: {id}.");

            var vendorFromRepository = result.Value;

            repository.Delete(vendorFromRepository);
            await repository.SaveChangesAsync();

            return NoContent();
        }

        // TODO: move this to a common location and reuse in Person, Business, and Vendor
        private static void UpdateContactDetails(VendorToWrite vendorFromCaller, Vendor vendorFromRepository)
        {
            var contactDetails = ContactDetailsFactory.Create(
                vendorFromCaller.Phones, vendorFromCaller.Emails, vendorFromCaller.Address).Value;

            vendorFromRepository.UpdateContactDetails(contactDetails);
        }

        private async Task UpdateVendor(VendorToWrite vendorFromCaller, Vendor vendorFromRepository)
        {
            if (vendorFromRepository.VendorCode != vendorFromCaller.VendorCode)
                vendorFromRepository.SetVendorCode(vendorFromCaller.VendorCode);

            if (vendorFromRepository.Name != vendorFromCaller.Name)
                vendorFromRepository.SetName(vendorFromCaller.Name);

            if (vendorFromRepository.Notes != vendorFromCaller.Notes)
                vendorFromRepository.SetNotes(vendorFromCaller.Notes);

            if (vendorFromCaller.IsActive.HasValue)
            {
                if (vendorFromCaller.IsActive.Value.Equals(true))
                    vendorFromRepository.Enable();

                if (vendorFromCaller.IsActive.Value.Equals(false))
                    vendorFromRepository.Disable();
            }

            if (vendorFromRepository.VendorRole != vendorFromCaller.VendorRole)
                vendorFromRepository.SetVendorRole(vendorFromCaller.VendorRole);

            if (DefaultPaymentMethodHasEdits(vendorFromRepository?.DefaultPaymentMethod, vendorFromCaller?.DefaultPaymentMethod))
                if (vendorFromCaller?.DefaultPaymentMethod is not null)
                {
                    vendorFromRepository.SetDefaultPaymentMethod(
                        DefaultPaymentMethod.Create(
                        await paymentMethodRepository.GetEntityAsync(
                            vendorFromCaller.DefaultPaymentMethod.PaymentMethod.Id),
                            vendorFromCaller.DefaultPaymentMethod.AutoCompleteDocuments).Value);
                }

            if (vendorFromCaller?.DefaultPaymentMethod is null)
                vendorFromRepository.ClearDefaultPaymentMethod();
        }

        private static bool DefaultPaymentMethodHasEdits(DefaultPaymentMethod defaultPaymentMethodFromRepository, DefaultPaymentMethodToRead defaultPaymentMethodToUpdate)
        {
            if (defaultPaymentMethodFromRepository is null && defaultPaymentMethodToUpdate is null)
                return false;

            if (defaultPaymentMethodFromRepository is null || defaultPaymentMethodToUpdate is null)
                return true;

            var hasPaymentMethodEdits = defaultPaymentMethodFromRepository.PaymentMethod.Id != defaultPaymentMethodToUpdate.PaymentMethod.Id;
            var hasAutoCompleteDocumentsEdits = defaultPaymentMethodFromRepository.AutoCompleteDocuments != defaultPaymentMethodToUpdate.AutoCompleteDocuments;

            return hasPaymentMethodEdits || hasAutoCompleteDocumentsEdits;
        }
    }
}
