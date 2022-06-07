using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Domain.Entities.Inventory;
using CustomerVehicleManagement.Shared.Models.Manufacturers;
using CustomerVehicleManagement.Shared.Models.SaleCodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Shared.Models.ProductCodes
{
    public class ProductCodeToWrite
    {
        public long Id { get; set; }
        public ManufacturerToWrite Manufacturer { get; set; }
        public string Code { get; set; }
        public SaleCodeToWrite SaleCode { get; set; }
        public string Name { get; set; }

    }
}
