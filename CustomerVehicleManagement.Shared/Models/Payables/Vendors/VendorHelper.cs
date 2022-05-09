using CustomerVehicleManagement.Domain.Entities.Payables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Shared.Models.Payables.Vendors
{
    public class VendorHelper
    {
        public static Vendor CreateVendor(VendorToWrite vendor)
        {
            if (vendor is null)
                return null;

            return new()
            {
                Name = vendor.Name,
                VendorCode = vendor.VendorCode,
                IsActive = vendor.IsActive
            };
        }

        public static VendorToWrite CreateVendor(VendorToRead vendor)
        {
            if (vendor is null)
                return null;

            return new()
            {
                Name = vendor.Name,
                VendorCode = vendor.VendorCode,
                IsActive = vendor.IsActive
            };
        }

        public static VendorToRead CreateVendor(Vendor vendor)
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
