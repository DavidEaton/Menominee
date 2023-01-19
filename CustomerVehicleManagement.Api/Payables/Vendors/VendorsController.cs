using CSharpFunctionalExtensions;
using CustomerVehicleManagement.Api.Data;
using CustomerVehicleManagement.Api.Payables.PaymentMethods;
using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Domain.Entities.Payables;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices.Payments;
using CustomerVehicleManagement.Shared.Models.Payables.Vendors;
using Menominee.Common.ValueObjects;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.Payables.Vendors
{
    public class VendorsController : ApplicationController
    {
        private readonly IVendorRepository repository;
        private readonly IVendorInvoicePaymentMethodRepository paymentMethodRepository;
        private readonly string BasePath = "/api/payables/vendors";

        public VendorsController(IVendorRepository repository, IVendorInvoicePaymentMethodRepository paymentMethodRepository)
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
        public async Task<IActionResult> UpdateVendorAsync(long id, VendorToWrite vendorToUpdate)
        {
            var notFoundMessage = $"Could not find Vendor to update: {vendorToUpdate.Name}";

            if (!await repository.VendorExistsAsync(id))
                return NotFound(notFoundMessage);

            var vendorFromRepository = await repository.GetVendorEntityAsync(id);

            if (vendorFromRepository.VendorCode != vendorToUpdate.VendorCode)
                vendorFromRepository.SetVendorCode(vendorToUpdate.VendorCode);

            if (vendorFromRepository.Name != vendorToUpdate.Name)
                vendorFromRepository.SetName(vendorToUpdate.Name);

            if (vendorFromRepository.Notes != vendorToUpdate.Notes)
                vendorFromRepository.SetNote(vendorToUpdate.Notes);

            if (vendorToUpdate.IsActive.HasValue)
            {
                if (vendorToUpdate.IsActive.Value.Equals(true))
                    vendorFromRepository.Enable();

                if (vendorToUpdate.IsActive.Value.Equals(false))
                    vendorFromRepository.Disable();
            }

            if (vendorFromRepository.VendorRole != vendorToUpdate.VendorRole)
                vendorFromRepository.SetVendorRole(vendorToUpdate.VendorRole);

            if (HasEdits(vendorFromRepository?.DefaultPaymentMethod, vendorToUpdate?.DefaultPaymentMethod))
                if (vendorToUpdate?.DefaultPaymentMethod is not null)
                {
                    DefaultPaymentMethod defaultPaymentMethod = DefaultPaymentMethod.Create(
                        await paymentMethodRepository.GetPaymentMethodEntityAsync(
                            vendorToUpdate.DefaultPaymentMethod.PaymentMethod.Id),
                            vendorToUpdate.DefaultPaymentMethod.AutoCompleteDocuments).Value;

                    vendorFromRepository.SetDefaultPaymentMethod(defaultPaymentMethod);
                }

            if (vendorToUpdate?.DefaultPaymentMethod is null)
                vendorFromRepository.ClearDefaultPaymentMethod();


            /* TODO: Can we centralize Update code that gets repeated for Persons and
             Vendors and Organizations (all classes deriiving from Contactable)?
            */

            //Address
            //Notes
            //Phones
            //Email


            await repository.SaveChangesAsync();

            return NoContent();
        }

        private static bool HasEdits(DefaultPaymentMethod defaultPaymentMethodFromRepository, DefaultPaymentMethodToRead defaultPaymentMethodToUpdate)
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
            var vendorOrError = Vendor.Create(
                vendorToAdd.Name,
                vendorToAdd.VendorCode.ToUpper(),
                vendorToAdd.VendorRole,
                vendorToAdd.Notes);

            if (vendorOrError.IsFailure)
                return NotFound($"Could not add new Vendor '{vendorToAdd.Name}'.");

            Vendor vendor = vendorOrError.Value;
            VendorInvoicePaymentMethod paymentMethod = null;
            Result<Address> addressOrError = null;

            if (vendorToAdd.DefaultPaymentMethod is not null)
                paymentMethod = await paymentMethodRepository.GetPaymentMethodEntityAsync(
                    vendorToAdd.DefaultPaymentMethod.PaymentMethod.Id);

            if (paymentMethod is null)
                return NotFound($"Could not add new Vendor '{vendorToAdd.Name}'. Vendor default payment method {vendorToAdd.DefaultPaymentMethod.PaymentMethod.Name} was not found.");

            var defaultPaymentMethodOrError = DefaultPaymentMethod.Create(paymentMethod, vendorToAdd.DefaultPaymentMethod.AutoCompleteDocuments);

            if (defaultPaymentMethodOrError.IsFailure)
                return NotFound($"Could not add new Vendor '{vendorToAdd.Name}': {defaultPaymentMethodOrError.Error}");

            var setDefaultPaymentMethodResult = vendor.SetDefaultPaymentMethod(defaultPaymentMethodOrError.Value);

            if (setDefaultPaymentMethodResult.IsFailure)
                return NotFound($"Could not add new Vendor '{vendorToAdd.Name}': {setDefaultPaymentMethodResult.Error}");

            // TODO: Can we centralize this code that gets repeated for Persons and
            // Organizations (all classes deriiving from Contactable)?
            // ∨∨∨∨∨∨∨∨∨∨∨∨∨∨∨∨∨∨∨∨∨∨∨∨∨∨∨∨∨∨∨∨∨∨∨∨∨∨∨∨∨∨∨∨∨∨∨∨∨∨
            if (vendorToAdd.Address is not null)
                addressOrError = Address.Create(vendorToAdd.Address.AddressLine, vendorToAdd.Address.City, vendorToAdd.Address.State, vendorToAdd.Address.PostalCode);

            if (addressOrError.IsFailure)
                return NotFound($"Could not add new Vendor '{vendorToAdd.Name}': {addressOrError.Error}");

            var setAddressResult = vendor.SetAddress(addressOrError.Value);

            if (setAddressResult.IsFailure)
                return NotFound($"Could not add new Vendor '{vendorToAdd.Name}': {setAddressResult.Error}");

            if (vendorToAdd?.Phones.Count > 0)
                foreach (var phone in vendorToAdd.Phones)
                    vendor.AddPhone(
                        Phone.Create(phone.Number, phone.PhoneType, phone.IsPrimary).Value);

            if (vendorToAdd?.Emails.Count > 0)
                foreach (var email in vendorToAdd.Emails)
                    vendor.AddEmail(
                        Email.Create(email.Address, email.IsPrimary).Value);

            // ∧∧∧∧∧∧∧∧∧∧∧∧∧∧∧∧∧∧∧∧∧∧∧∧∧∧∧∧∧∧∧∧∧∧∧∧∧∧∧∧∧∧∧∧∧∧∧∧∧∧

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
