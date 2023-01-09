﻿using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Domain.Entities.Payables;
using CustomerVehicleManagement.Shared.Models.Addresses;
using CustomerVehicleManagement.Shared.Models.Contactable;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices.Payments;

namespace CustomerVehicleManagement.Shared.Models.Payables.Vendors
{
    public class VendorHelper
    {
        public static VendorToWrite ConvertReadToWriteDto(VendorToRead vendor)
        {
            return vendor is not null
                ? new VendorToWrite()
                {
                    Id = vendor.Id,
                    Name = vendor.Name,
                    VendorCode = vendor.VendorCode,
                    IsActive = vendor.IsActive,
                    DefaultPaymentMethod = vendor.DefaultPaymentMethod,
                    Note = vendor.Notes,
                    Address = AddressHelper.ConvertReadToWriteDto(vendor.Address),
                    Phones = PhoneHelper.ConvertReadToWriteDtos(vendor.Phones),
                    Emails = EmailHelper.ConvertReadToWriteDto(vendor.Emails),
                }
                : null;
        }

        public static VendorToRead ConvertEntityToReadDto(Vendor vendor)
        {
            if (vendor is null)
                return null;

            var result = new VendorToRead();
            var vendorToRead = new VendorToRead();
            DefaultPaymentMethodToRead defaultPaymentMethod = null;

            vendorToRead.Id = vendor.Id;
            vendorToRead.Name = vendor.Name;
            vendorToRead.VendorCode = vendor.VendorCode;
            vendorToRead.IsActive = vendor.IsActive;

            if (vendor.DefaultPaymentMethod is not null)
            {
                defaultPaymentMethod = new DefaultPaymentMethodToRead()
                {
                    PaymentMethod = new VendorInvoicePaymentMethodToRead()
                    {
                        Id = vendor.DefaultPaymentMethod.PaymentMethod.Id,
                        Name = vendor.DefaultPaymentMethod.PaymentMethod.Name,
                        IsActive = vendor.DefaultPaymentMethod.PaymentMethod.IsActive,
                        PaymentType = vendor.DefaultPaymentMethod.PaymentMethod.PaymentType,
                        ReconcilingVendor = ConvertEntityToReadDto(vendor.DefaultPaymentMethod.PaymentMethod.ReconcilingVendor)
                    }
                };
            }

            vendorToRead.DefaultPaymentMethod = defaultPaymentMethod;

            if (vendor.Address is not null)
                vendorToRead.Address = AddressHelper.ConvertEntityToReadDto(vendor.Address);

            vendorToRead.Phones = PhoneHelper.ConvertEntitiesToReadDtos(vendor.Phones);
            vendorToRead.Emails = EmailHelper.ConvertEntitiesToReadDtos(vendor.Emails);

            return vendorToRead;
        }
    }
}
