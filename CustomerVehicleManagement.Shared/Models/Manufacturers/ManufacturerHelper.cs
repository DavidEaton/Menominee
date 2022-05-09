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
        public static ManufacturerToWrite CreateManufacturer(ManufacturerToRead manufacturer)
        {
            if (manufacturer is null)
                return new ManufacturerToWrite();

            return new ManufacturerToWrite()
            {
                Code = manufacturer.Code,
                Prefix = manufacturer.Prefix,
                Name = manufacturer.Name
            };
        }

        public static ManufacturerToRead CreateManufacturer(Manufacturer manufacturer)
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

        public static ManufacturerToReadInList CreateManufacturerInList(Manufacturer manufacturer)
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
