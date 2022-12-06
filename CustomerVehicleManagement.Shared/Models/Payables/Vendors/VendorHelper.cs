﻿using CustomerVehicleManagement.Domain.Entities.Payables;
using System;

namespace CustomerVehicleManagement.Shared.Models.Payables.Vendors
{
    public class VendorHelper
    {
        public static Vendor ConvertWriteDtoToEntity(VendorToWrite vendor)
        {
            return vendor is null
                ? null
                //: Vendor.Create(vendor.Name, vendor.VendorCode.ToUpper()).Value;
                : Vendor.Create(vendor.Name, vendor.VendorCode.ToUpper(), vendor.DefaultPaymentMethod).Value;
        }

        public static Vendor ConvertWriteDtoToEntity(VendorToRead vendor)
        {
            return vendor is null
                ? null
                //: Vendor.Create(vendor.Name, vendor.VendorCode.ToUpper()).Value;
                : Vendor.Create(vendor.Name, vendor.VendorCode.ToUpper(), vendor.DefaultPaymentMethod).Value;
        }

        public static VendorToWrite ConvertReadToWriteDto(VendorToRead vendor)
        {
            return vendor is null
                ? null
                : new()
                {
                    Id = vendor.Id,
                    Name = vendor.Name,
                    VendorCode = vendor.VendorCode,
                    IsActive = vendor.IsActive,
                    DefaultPaymentMethod = vendor.DefaultPaymentMethod
                };
        }

        public static VendorToRead ConvertEntityToReadDto(Vendor vendor)
        {
            return vendor is null
                ? null
                : new()
                {
                    Id = vendor.Id,
                    Name = vendor.Name,
                    VendorCode = vendor.VendorCode,
                    IsActive = vendor.IsActive,
                    DefaultPaymentMethod = vendor.DefaultPaymentMethod
                };
        }

        public static VendorToReadInList ConvertReadToReadInListDto(VendorToRead vendor)
        {
            return vendor is null
                ? null
                : new()
                {
                    Id = vendor.Id,
                    Name = vendor.Name,
                    VendorCode = vendor.VendorCode,
                    IsActive = vendor.IsActive
                };
        }
    }
}
