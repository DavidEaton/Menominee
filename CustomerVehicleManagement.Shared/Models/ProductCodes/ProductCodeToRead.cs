using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Domain.Entities.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Shared.Models.ProductCodes
{
    public class ProductCodeToRead
    {
        public long Id { get; set; }
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
                    Id = pc.Id,
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
