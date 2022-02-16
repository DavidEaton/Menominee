using CustomerVehicleManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Shared.Models.ProductCodes
{
    public class ProductCodeToRead
    {
        public Manufacturer Manufacturer { get; set; }
        public string Code { get; set; }
        public SaleCode SaleCode { get; set; }
        public string Name { get; set; }

        public static ProductCodeToRead ConvertToDto(ProductCode pc)
        {
            if (pc != null)
            {
                return new ProductCodeToRead
                {
                    Manufacturer = pc.Manufacturer,
                    Code = pc.Code,
                    SaleCode = pc.SaleCode,
                    Name = pc.Name
                };
            }

            return null;
        }
    }
}
