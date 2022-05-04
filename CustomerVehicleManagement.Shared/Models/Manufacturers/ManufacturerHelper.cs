using CustomerVehicleManagement.Domain.Entities.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Shared.Models.Manufacturers
{
    public class ManufacturerHelper
    {
        public static ManufacturerToWrite TransformManufacturer(ManufacturerToRead manufacturer)
        {
            if (manufacturer is null)
                return new ManufacturerToWrite();

            return new ManufacturerToWrite
            {
                Code = manufacturer.Code,
                Name = manufacturer.Name
            };
        }

        public static ManufacturerToRead TransformManufacturer(Manufacturer manufacturer)
        {
            if (manufacturer is null)
                return null;

            return new ManufacturerToRead()
            {
                    Id = manufacturer.Id,
                    Code = manufacturer.Code,
                    Prefix = manufacturer.Prefix,
                    Name = manufacturer.Name
            };
        }
    }
}
