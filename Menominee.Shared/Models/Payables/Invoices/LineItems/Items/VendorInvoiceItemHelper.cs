using Menominee.Domain.Entities;
using Menominee.Domain.Entities.Inventory;
using Menominee.Domain.Entities.Payables;
using Menominee.Shared.Models.Manufacturers;
using Menominee.Shared.Models.SaleCodes;
using System.Collections.Generic;
using System.Linq;

namespace Menominee.Shared.Models.Payables.Invoices.LineItems.Items
{
    public class VendorInvoiceItemHelper
    {
        public static VendorInvoiceItem ConvertWriteDtoToEntity(
            VendorInvoiceItemToWrite item, IReadOnlyList<Manufacturer> manufacturers, IReadOnlyList<SaleCode> saleCodes)
        {
            return
                item is null
                ? null
                : VendorInvoiceItem.Create(
                    item.PartNumber,
                    item.Description,
                    item.Manufacturer is null
                        ? null
                        : manufacturers.FirstOrDefault(manufacturer => manufacturer.Id == item.Manufacturer.Id),
                    item.SaleCode is null
                        ? null
                        : saleCodes.FirstOrDefault(saleCode => saleCode.Id == item.SaleCode.Id))
                .Value;
        }

        public static VendorInvoiceItemToWrite ConvertReadDtoToWriteDto(VendorInvoiceItemToRead item)
        {
            return
                item is null
                ? null
                : new()
                {
                    Description = item.Description,
                    Manufacturer = item.Manufacturer,
                    PartNumber = item.PartNumber,
                    SaleCode = item.SaleCode
                };
        }

        internal static VendorInvoiceItemToWrite ConvertToWriteDto(VendorInvoiceItem item)
        {
            return
                item is null
                ? null
                : new()
                {
                    Description = item.Description,
                    Manufacturer =
                        item.Manufacturer is null
                        ? null
                        : ManufacturerHelper.ConvertToReadDto(item.Manufacturer),
                    PartNumber = item.PartNumber,
                    SaleCode =
                        item.SaleCode is null
                        ? null : SaleCodeHelper.ConvertToReadDto(item.SaleCode)
                };
        }

        internal static VendorInvoiceItemToRead ConvertWriteToReadDto(VendorInvoiceItemToWrite item)
        {
            return
                item is null
                ? null
                : new()
                {
                    Description = item.Description,
                    Manufacturer =
                        item.Manufacturer is null
                        ? null
                        : item.Manufacturer,
                    PartNumber = item.PartNumber,
                    SaleCode =
                        item.SaleCode is null
                        ? null : item.SaleCode
                };
        }

        public static VendorInvoiceItemToWrite ConvertToReadDto(VendorInvoiceItem item)
        {
            return
                item is null
                ? null
                : new()
                {
                    Description = item.Description,
                    Manufacturer =
                        item.Manufacturer is null
                        ? null
                        : ManufacturerHelper.ConvertToReadDto(item.Manufacturer),
                    PartNumber = item.PartNumber,
                    SaleCode =
                        item.SaleCode is null
                        ? null
                        : SaleCodeHelper.ConvertToReadDto(item.SaleCode)
                };
        }
    }
}
