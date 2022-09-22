using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Domain.Entities.Inventory;
using CustomerVehicleManagement.Domain.Entities.Payables;
using System.Collections.Generic;
using System.Linq;

namespace CustomerVehicleManagement.Shared.Models.Payables.Invoices.LineItems.Items
{
    public class VendorInvoiceItemHelper
    {
        public static VendorInvoiceItem ConvertWriteDtoToEntity(
            VendorInvoiceItemToWrite item, IReadOnlyList<Manufacturer> manufacturers, IReadOnlyList<SaleCode> saleCodes)
        {
            return VendorInvoiceItem.Create(
                item.PartNumber,
                item.Description,
                manufacturers.FirstOrDefault(manufacturer => manufacturer.Id == item.Manufacturer.Id),
                saleCodes.FirstOrDefault(saleCode => saleCode.Id == item.SaleCode.Id))
                .Value;
        }

        public static VendorInvoiceItemToWrite ConvertReadDtoToWriteDto(VendorInvoiceItemToRead item)
        {
            return new VendorInvoiceItemToWrite()
            {
                Description = item.Description,
                Manufacturer = item.Manufacturer,
                PartNumber = item.PartNumber,
                SaleCode = item.SaleCode
            };
        }
    }
}
