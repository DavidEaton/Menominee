using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CustomerVehicleManagement.Api.Common;
using CustomerVehicleManagement.Api.Payables.PaymentMethods;
using CustomerVehicleManagement.Domain.Entities.Payables;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices.Payments;
using CustomerVehicleManagement.Shared.Models.Payables.Vendors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CustomerVehicleManagement.Api.Payables.Vendors
{
    public class VendorsController : BaseApplicationController<VendorsController>
    {
        private readonly IVendorRepository repository;
        private readonly IVendorInvoicePaymentMethodRepository paymentMethodRepository;
        private readonly string BasePath = "/api/vendors";

        public VendorsController(
            IVendorRepository repository
            , IVendorInvoicePaymentMethodRepository paymentMethodRepository
            , ILogger<VendorsController> logger) : base(logger)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.paymentMethodRepository = paymentMethodRepository ?? throw new ArgumentNullException(nameof(paymentMethodRepository));
        }

        // api/vendors
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<VendorToRead>>> GetVendorsAsync()
        {
            var result = await repository.GetVendorsAsync();
            return Ok(result);
        }

        // api/vendors/1
        [HttpGet("{id:long}", Name = "GetVendorAsync")]
        public async Task<ActionResult<VendorToRead>> GetVendorAsync(long id)
        {
            var result = await repository.GetVendorAsync(id);

            if (result == null)
                return NotFound();

            return result;
        }

        // api/vendors/1
        [HttpPut("{id:long}")]
        public async Task<IActionResult> UpdateVendorAsync(long id, VendorToWrite vendorFromCaller)
        {
            var notFoundMessage = $"Could not find Vendor to update: {vendorFromCaller.Name}";

            if (!await repository.VendorExistsAsync(id))
                return NotFound(notFoundMessage);

            var vendorFromRepository = await repository.GetVendorEntityAsync(id);

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
                        await paymentMethodRepository.GetPaymentMethodEntityAsync(
                            vendorFromCaller.DefaultPaymentMethod.PaymentMethod.Id),
                            vendorFromCaller.DefaultPaymentMethod.AutoCompleteDocuments).Value);
                }

            if (vendorFromCaller?.DefaultPaymentMethod is null)
                vendorFromRepository.ClearDefaultPaymentMethod();

            var contactDetails = ContactDetailsFactory.Create(
                vendorFromCaller.Phones, vendorFromCaller.Emails, vendorFromCaller.Address).Value;

            vendorFromRepository.UpdateContactDetails(contactDetails);

            await repository.SaveChangesAsync();

            return NoContent();
        }

        private static bool DefaultPaymentMethodHasEdits(DefaultPaymentMethod defaultPaymentMethodFromRepository, DefaultPaymentMethodToRead defaultPaymentMethodToUpdate)
        {
            if (defaultPaymentMethodFromRepository is null && defaultPaymentMethodToUpdate is null)
                return false;

            if ((defaultPaymentMethodFromRepository is not null && defaultPaymentMethodToUpdate is null)
                ||
                (defaultPaymentMethodFromRepository is null && defaultPaymentMethodToUpdate is not null))
                return true;

            return
                  defaultPaymentMethodFromRepository.PaymentMethod.Id != defaultPaymentMethodToUpdate.PaymentMethod.Id
                || defaultPaymentMethodFromRepository.AutoCompleteDocuments != defaultPaymentMethodToUpdate.AutoCompleteDocuments;
        }

        [HttpPost]
        public async Task<IActionResult> AddVendorAsync(VendorToWrite vendorToAdd)
        {
            // VK Im.2: no need to validate it here again, just call .Value right away
            var vendor = Vendor.Create(
                vendorToAdd.Name,
                vendorToAdd.VendorCode.ToUpper(),
                vendorToAdd.VendorRole,
                vendorToAdd.Notes).Value;

            if (vendorToAdd?.DefaultPaymentMethod?.PaymentMethod is not null)
            {
                var paymentMethod = await paymentMethodRepository.GetPaymentMethodEntityAsync(
                    vendorToAdd.DefaultPaymentMethod.PaymentMethod.Id);

                if (paymentMethod is null)
                    return NotFound($"Could not add new Vendor '{vendorToAdd.Name}'. Vendor default payment method {vendorToAdd.DefaultPaymentMethod.PaymentMethod.Name} was not found.");

                var setDefaultPaymentMethodResult = vendor.SetDefaultPaymentMethod(DefaultPaymentMethod.Create(paymentMethod, vendorToAdd.DefaultPaymentMethod.AutoCompleteDocuments).Value);

                if (setDefaultPaymentMethodResult.IsFailure)
                    return NotFound($"Could not add new Vendor '{vendorToAdd.Name}': {setDefaultPaymentMethodResult.Error}");
            }

            vendor.UpdateContactDetails(ContactDetailsFactory.Create(
                vendorToAdd.Phones, vendorToAdd.Emails, vendorToAdd.Address).Value);

            await repository.AddVendorAsync(vendor);
            await repository.SaveChangesAsync();

            return Created(new Uri($"{BasePath}/{vendor.Id}",
                               UriKind.Relative),
                               new { vendor.Id });
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeleteVendorAsync(long id)
        {
            var vendorFromRepository = await repository.GetVendorEntityAsync(id);

            if (vendorFromRepository is null)
                return NotFound($"Could not find Vendor in the database to delete with Id: {id}.");

            repository.DeleteVendor(vendorFromRepository);

            await repository.SaveChangesAsync();

            return NoContent();
        }
    }
}
