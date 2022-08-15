using CustomerVehicleManagement.Domain.Entities.Payables;
using CustomerVehicleManagement.Shared.Models.Manufacturers;
using CustomerVehicleManagement.Shared.Models.SaleCodes;

namespace CustomerVehicleManagement.Shared.Models.Payables.Invoices.LineItems.Items
{
    public class VendorInvoiceItemHelper
    {
        public static VendorInvoiceItem ConvertWriteDtoToEntity(VendorInvoiceItemToWrite item)
        {
            return VendorInvoiceItem.Create(
                item.PartNumber,
                ManufacturerHelper.ConvertWriteDtoToEntity(item.Manufacturer),
                item.Description,
                SaleCodeHelper.ConvertWriteDtoToEntity(item.SaleCode))
                .Value;
        }

        public static VendorInvoiceItemToWrite ConvertReadDtoToWriteDto(VendorInvoiceItemToRead item)
        {
            return new VendorInvoiceItemToWrite()
            {
                Description = item.Description,
                Manufacturer = ManufacturerHelper.ConvertReadToWriteDto(item.Manufacturer),
                PartNumber = item.PartNumber,
                SaleCode = SaleCodeHelper.ConvertReadToWriteDto(item.SaleCode)
            };
        }
    }
}
