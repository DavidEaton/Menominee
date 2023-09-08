using Menominee.Domain.Entities.Payables;
using Menominee.Shared.Models.Addresses;
using Menominee.Shared.Models.Contactable;
using Menominee.Shared.Models.Payables.Invoices.Payments;

namespace Menominee.Shared.Models.Payables.Vendors
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
                    Emails = EmailHelper.ConvertReadToWriteDtos(vendor.Emails),
                }
                : null;
        }

        public static VendorToRead ConvertToReadDto(Vendor vendor, int recursionDepth = 0)
        {
            if (vendor is null || recursionDepth > 4)
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
                        ReconcilingVendor = ConvertToReadDto(
                            vendor.DefaultPaymentMethod.PaymentMethod.ReconcilingVendor, ++recursionDepth)
                    }
                };
            }

            vendorToRead.DefaultPaymentMethod = defaultPaymentMethod;

            if (vendor.Address is not null)
                vendorToRead.Address = AddressHelper.ConvertToReadDto(vendor.Address);

            vendorToRead.Phones = PhoneHelper.ConvertToReadDtos(vendor.Phones);
            vendorToRead.Emails = EmailHelper.ConvertToReadDtos(vendor.Emails);

            return vendorToRead;
        }

        internal static Vendor ConvertReadToEntity(VendorToRead reconcilingVendor)
        {
            return Vendor.Create(
                name: reconcilingVendor.Name,
                vendorCode: reconcilingVendor.VendorCode,
                vendorRole: reconcilingVendor.VendorRole,
                defaultPaymentMethod: VendorInvoicePaymentMethodHelper
                    .ConvertReadDtoToEntity(reconcilingVendor.DefaultPaymentMethod))
                .Value;
        }
    }
}
