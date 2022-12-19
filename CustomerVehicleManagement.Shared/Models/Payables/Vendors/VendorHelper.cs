using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Domain.Entities.Payables;
using CustomerVehicleManagement.Shared.Models.Addresses;
using CustomerVehicleManagement.Shared.Models.Contactable;
using CustomerVehicleManagement.Shared.Models.Persons;
using Menominee.Common.ValueObjects;
using System;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Shared.Models.Payables.Vendors
{
    public class VendorHelper
    {
        public static Vendor ConvertWriteDtoToEntity(VendorToWrite vendor)
        {
            if (vendor == null) 
                return null;

            Address address = null;
            List<Phone> phones = new();
            List<Email> emails = new();

            if (vendor?.Address is not null)
                address = Address.Create(
                    vendor.Address.AddressLine,
                    vendor.Address.City,
                    vendor.Address.State,
                    vendor.Address.PostalCode).Value;

            if (vendor?.Phones?.Count > 0)
                foreach (var phone in vendor.Phones)
                    phones.Add(Phone.Create(phone.Number, phone.PhoneType, phone.IsPrimary).Value);

            if (vendor?.Emails?.Count > 0)
                foreach (var email in vendor.Emails)
                    emails.Add(Email.Create(email.Address, email.IsPrimary).Value);

            return Vendor.Create(vendor.Name, 
                                vendor.VendorCode.ToUpper(), 
                                vendor.DefaultPaymentMethod,
                                //vendor.Notes,
                                address,
                                emails,
                                phones
                                ).Value;
        }

        public static Vendor ConvertReadDtoToEntity(VendorToRead vendor)
        {
            if (vendor == null)
                return null;

            Address address = null;
            List<Phone> phones = new();
            List<Email> emails = new();

            if (vendor?.Address is not null)
                address = Address.Create(
                    vendor.Address.AddressLine,
                    vendor.Address.City,
                    vendor.Address.State,
                    vendor.Address.PostalCode).Value;

            if (vendor?.Phones?.Count > 0)
                foreach (var phone in vendor.Phones)
                    phones.Add(Phone.Create(phone.Number, phone.PhoneType, phone.IsPrimary).Value);

            if (vendor?.Emails?.Count > 0)
                foreach (var email in vendor.Emails)
                    emails.Add(Email.Create(email.Address, email.IsPrimary).Value);

            return Vendor.Create(vendor.Name,
                                vendor.VendorCode.ToUpper(),
                                vendor.DefaultPaymentMethod,
                                //vendor.Notes,
                                address,
                                emails,
                                phones
                                ).Value;
        }

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
                    Notes = vendor.Notes,
                    Address = AddressHelper.ConvertReadToWriteDto(vendor.Address),
                    Phones = PhoneHelper.ConvertReadToWriteDtos(vendor.Phones),
                    Emails = EmailHelper.ConvertReadToWriteDto(vendor.Emails),
                }
                : null;
        }

        public static VendorToRead ConvertEntityToReadDto(Vendor vendor)
        {
            return vendor is not null
                ? new VendorToRead()
                {
                    Id = vendor.Id,
                    Name = vendor.Name,
                    VendorCode = vendor.VendorCode,
                    IsActive = vendor.IsActive,
                    DefaultPaymentMethod = vendor.DefaultPaymentMethod,
                    //Notes = vendor.Notes,
                    Address = AddressHelper.ConvertToDto(vendor.Address),
                    Phones = PhoneHelper.ConvertEntitiesToReadDtos(vendor.Phones),
                    Emails = EmailHelper.ConvertEntitiesToReadDtos(vendor.Emails),
                }
                : null;
        }

        public static VendorToReadInList ConvertReadToReadInListDto(VendorToRead vendor)
        {
            return vendor is not null
                ? new()
                {
                    Id = vendor.Id,
                    Name = vendor.Name,
                    VendorCode = vendor.VendorCode,
                    IsActive = vendor.IsActive
                }
                : null;
        }
    }
}
