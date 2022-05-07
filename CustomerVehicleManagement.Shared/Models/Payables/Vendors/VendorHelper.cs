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
        public static Vendor Transform(VendorToWrite vendor)
        {
            if (vendor is null)
                return null;

            return new Vendor()
            {
                Name = vendor.Name,
                VendorCode = vendor.VendorCode,
                IsActive = vendor.IsActive
            };
        }

        public static VendorToWrite Transform(VendorToRead vendor)
        {
            if (vendor is null)
                return null;

            return new VendorToWrite
            {
                Name = vendor.Name,
                VendorCode = vendor.VendorCode,
                IsActive = vendor.IsActive
            };
        }

        public static VendorToRead Transform(Vendor vendor)
        {
            if (vendor is null)
                return null;

            return new VendorToRead
            {
                Id = vendor.Id,
                Name = vendor.Name,
                VendorCode = vendor.VendorCode,
                IsActive = vendor.IsActive
            };
        }
    }
}
