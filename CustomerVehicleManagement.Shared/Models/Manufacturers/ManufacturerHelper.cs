using CustomerVehicleManagement.Domain.Entities.Inventory;

namespace CustomerVehicleManagement.Shared.Models.Manufacturers
{
    public class ManufacturerHelper
    {
        public static ManufacturerToRead ConvertToReadDto(Manufacturer manufacturer)
        {
            if (manufacturer != null)
            {
                return new ManufacturerToRead()
                {
                    Id = manufacturer.Id,
                    Code = manufacturer.Code,
                    Prefix = manufacturer.Prefix,
                    Name = manufacturer.Name
                };
            }

            return null;
        }

        public static ManufacturerToReadInList ConvertToReadInListDto(Manufacturer manufacturer)
        {
            if (manufacturer != null)
            {
                return new ManufacturerToReadInList
                {
                    Id = manufacturer.Id,
                    Code = manufacturer.Code,
                    Prefix = manufacturer.Prefix,
                    Name = manufacturer.Name
                };
            }

            return null;
        }
    }
}
