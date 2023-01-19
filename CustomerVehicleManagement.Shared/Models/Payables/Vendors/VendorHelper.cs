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
                    VendorRole = vendor.VendorRole,
                    IsActive = vendor.IsActive,
                    DefaultPaymentMethod = vendor.DefaultPaymentMethod,
                    Notes = vendor.Notes,
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

            DefaultPaymentMethodToRead defaultPaymentMethod = null;

            var vendorToRead = new VendorToRead
            {
                Id = vendor.Id,
                Name = vendor.Name,
                VendorCode = vendor.VendorCode,
                VendorRole = vendor.VendorRole,
                IsActive = vendor.IsActive,
                Notes = vendor.Notes
            };

            if (vendor.DefaultPaymentMethod?.PaymentMethod is not null)
            {
                defaultPaymentMethod = new DefaultPaymentMethodToRead()
                {
                    AutoCompleteDocuments = vendor.DefaultPaymentMethod.AutoCompleteDocuments,
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
