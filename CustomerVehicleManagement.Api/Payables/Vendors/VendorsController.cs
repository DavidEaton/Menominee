using CSharpFunctionalExtensions;
using CustomerVehicleManagement.Api.Data;
using CustomerVehicleManagement.Api.Payables.PaymentMethods;
using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Domain.Entities.Payables;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices.LineItems.Items;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices.Payments;
using CustomerVehicleManagement.Shared.Models.Payables.Vendors;
using Menominee.Common.ValueObjects;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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
                vendorFromRepository.SetNote(vendorFromCaller.Notes);

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
                    DefaultPaymentMethod defaultPaymentMethod = DefaultPaymentMethod.Create(
                        await paymentMethodRepository.GetPaymentMethodEntityAsync(
                            vendorFromCaller.DefaultPaymentMethod.PaymentMethod.Id),
                            vendorFromCaller.DefaultPaymentMethod.AutoCompleteDocuments).Value;

                    vendorFromRepository.SetDefaultPaymentMethod(defaultPaymentMethod);
                }

            if (vendorFromCaller?.DefaultPaymentMethod is null)
                vendorFromRepository.ClearDefaultPaymentMethod();

            /* TODO: Centralize Update code that gets repeated for Persons, Customers,
            Vendor, Employees and Organizations (all classes deriiving from Contactable):
            Address
            Phones
            Email   */
            if (vendorFromCaller?.Address is not null)
            {
                // Since VendorValidator runs AddressValidator before request even gets
                // here in tyhe controller, no need to check Result.IsFailure, just
                // return the Value from the Create factory method:
                vendorFromRepository.SetAddress(
                    Address.Create(
                        vendorFromCaller.Address.AddressLine,
                        vendorFromCaller.Address.City,
                        vendorFromCaller.Address.State,
                        vendorFromCaller.Address.PostalCode)
                    .Value);
            }

            if (vendorFromCaller?.Address is null)
                vendorFromRepository.ClearAddress();

            // Phones
            foreach (var phoneFromCaller in vendorFromCaller?.Phones)
            {
                // Added
                if (phoneFromCaller.Id == 0)
                    vendorFromRepository.AddPhone(
                        Phone.Create(phoneFromCaller.Number, phoneFromCaller.PhoneType, phoneFromCaller.IsPrimary)
                        .Value);
                // Updated
                if (phoneFromCaller.Id != 0)
                {
                    var phoneFromRepository = vendorFromRepository?.Phones.FirstOrDefault(
                        contextPhone =>
                        contextPhone.Id == phoneFromCaller.Id);

                    if (phoneFromRepository.Number != phoneFromCaller.Number)
                        phoneFromRepository.SetNumber(phoneFromCaller.Number);

                    if (phoneFromRepository.PhoneType != phoneFromCaller.PhoneType)
                        phoneFromRepository.SetPhoneType(phoneFromCaller.PhoneType);

                    if (phoneFromRepository.IsPrimary != phoneFromCaller.IsPrimary)
                        phoneFromRepository.SetIsPrimary(phoneFromCaller.IsPrimary);
                }
                // TODO: Deleted
            }

            // Emails
            foreach (var emailFromCaller in vendorFromCaller?.Emails)
            {
                // Added
                if (emailFromCaller.Id == 0)
                    vendorFromRepository.AddEmail(
                        Email.Create(emailFromCaller.Address, emailFromCaller.IsPrimary)
                        .Value);
                // Updated
                if (emailFromCaller.Id != 0)
                {
                    var emailFromRepository = vendorFromRepository?.Emails.FirstOrDefault(
                        contextEmail =>
                        contextEmail.Id == emailFromCaller.Id);

                    if (emailFromRepository.Address != emailFromCaller.Address)
                        emailFromRepository.SetAddress(emailFromCaller.Address);

                    if (emailFromRepository.IsPrimary != emailFromCaller.IsPrimary)
                        emailFromRepository.SetIsPrimary(emailFromCaller.IsPrimary);
                }
                // TODO: Deleted
            }

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
