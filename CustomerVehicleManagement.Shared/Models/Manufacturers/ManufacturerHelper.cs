using CustomerVehicleManagement.Domain.Entities.Inventory;

namespace CustomerVehicleManagement.Shared.Models.Manufacturers
{
    public class ManufacturerHelper
    {
        public static ManufacturerToWrite ConvertReadToWriteDto(ManufacturerToRead manufacturer)
        {
            return
            manufacturer is null
                ? null
                : new ManufacturerToWrite()
                {
                    Id = manufacturer.Id,
                    Code = manufacturer.Code,
                    Prefix = manufacturer.Prefix,
                    Name = manufacturer.Name
                };
        }

        public static Manufacturer ConvertWriteDtoToEntity(ManufacturerToWrite manufacturer)
        {
            return
            manufacturer is null
                ? null
                : Manufacturer.Create(
                    manufacturer.Name,
                    manufacturer.Prefix,
                    manufacturer.Code)
                .Value;
        }

        public static ManufacturerToRead ConvertEntityToReadDto(Manufacturer manufacturer)
        {
            return
            manufacturer is null
                ? null
                : new()
                {
                    Id = manufacturer.Id,
                    Code = manufacturer.Code,
                    Prefix = manufacturer.Prefix,
                    Name = manufacturer.Name
                };
        }

        public static ManufacturerToReadInList ConvertEntityToReadInListDto(Manufacturer manufacturer)
        {
            return
            manufacturer is null
                ? null
                : new()
                {
                    Id = manufacturer.Id,
                Code = manufacturer.Code,
                Prefix = manufacturer.Prefix,
                Name = manufacturer.Name
            };
        }
    }
}
