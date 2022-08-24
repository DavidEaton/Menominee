using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Domain.Entities.Payables;
using Menominee.Common.ValueObjects;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Shared.Models.Payables.Vendors
{
    public class VendorHelper
    {
        public static Vendor ConvertWriteDtoToEntity(VendorToWrite vendor)
        {
            if (vendor is null)
                return null;
            
            return Vendor.Create(vendor.Name, vendor.VendorCode.ToUpper()).Value;
        }

        public static VendorToWrite ConvertReadToWriteDto(VendorToRead vendor)
        {
            if (vendor is null)
                return null;

            return new()
            {
                Id = vendor.Id,
                Name = vendor.Name,
                VendorCode = vendor.VendorCode,
                IsActive = vendor.IsActive
            };
        }

        public static VendorToRead ConvertEntityToReadDto(Vendor vendor)
        {
            if (vendor is null)
                return null;

            return new()
            {
                Id = vendor.Id,
                Name = vendor.Name,
                VendorCode = vendor.VendorCode,
                IsActive = vendor.IsActive
            };
        }
    }
}
