using CustomerVehicleManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Shared.Models.ProductCodes
{
    public class ProductCodeToWrite
    {
        public Manufacturer Manufacturer { get; set; }
        public string Code { get; set; }
        public SaleCode SaleCode { get; set; }
        public string Name { get; set; }

    }
}
