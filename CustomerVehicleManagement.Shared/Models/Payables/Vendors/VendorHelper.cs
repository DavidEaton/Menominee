using CustomerVehicleManagement.Domain.Entities.Payables;

namespace CustomerVehicleManagement.Shared.Models.Payables.Vendors
{
    public class VendorHelper
    {
        public static Vendor ConvertWriteDtoToEntity(VendorToWrite vendor)
        {
            if (vendor is null)
                return null;

            return new()
            {
                Name = vendor.Name,
                VendorCode = vendor.VendorCode.ToUpper(),
                IsActive = vendor.IsActive
            };
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
