using CustomerVehicleManagement.Domain.Entities.Inventory;
using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Domain.Entities.Payables;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices.LineItems.Items;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CustomerVehicleManagement.Shared.Models.Payables.Invoices.LineItems
{
    public class VendorInvoiceLineItemHelper
    {
        public static List<VendorInvoiceLineItem> ConvertWriteDtosToEntities(
            IList<VendorInvoiceLineItemToWrite> lineItems,
            IReadOnlyList<Manufacturer> manufacturer,
            IReadOnlyList<SaleCode> saleCode)
        {
            return lineItems?.Select(ConvertWriteDtoToEntity(
                manufacturer,
                saleCode)).ToList()
                ??
                new List<VendorInvoiceLineItem>();
        }

        public static Func<VendorInvoiceLineItemToWrite, VendorInvoiceLineItem> ConvertWriteDtoToEntity(
            IReadOnlyList<Manufacturer> manufacturers,
            IReadOnlyList<SaleCode> saleCodes)
        {
            return lineItem =>
                VendorInvoiceLineItem.Create(
                    lineItem.Type,
                    VendorInvoiceItemHelper.ConvertWriteDtoToEntity(lineItem.Item, manufacturers, saleCodes),
                    lineItem.Quantity,
                    lineItem.Cost,
                    lineItem.Core,
                    lineItem.PONumber,
                    lineItem.TransactionDate
                    ).Value;
        }

        public static IList<VendorInvoiceLineItemToWrite> ConvertReadDtosToWriteDtos(IReadOnlyList<VendorInvoiceLineItemToRead> lineItems)
        {
            return lineItems?.Select(ConvertReadDtoToWriteDto()).ToList()
                ?? new List<VendorInvoiceLineItemToWrite>();
        }

        private static Func<VendorInvoiceLineItemToRead, VendorInvoiceLineItemToWrite> ConvertReadDtoToWriteDto()
        {
            return lineItem =>
                new VendorInvoiceLineItemToWrite()
                {
                    Id = lineItem.Id,
                    Type = lineItem.Type,
                    Item = VendorInvoiceItemHelper.ConvertReadDtoToWriteDto(lineItem.Item),
                    Quantity = lineItem.Quantity,
                    Cost = lineItem.Cost,
                    Core = lineItem.Core,
                    PONumber = lineItem.PONumber,
                    TransactionDate = lineItem.TransactionDate
                };
        }
    }
}