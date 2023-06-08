using CSharpFunctionalExtensions;
using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Domain.Entities.Inventory;
using CustomerVehicleManagement.Domain.Entities.Payables;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices.LineItems.Items;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CustomerVehicleManagement.Shared.Models.Payables.Invoices.LineItems
{
    public class VendorInvoiceLineItemHelper
    {
        public static IReadOnlyList<VendorInvoiceLineItem> ConvertWriteDtosToEntities(IList<VendorInvoiceLineItemToWrite> lineItemsToWrite, IReadOnlyList<Manufacturer> manufacturers, IReadOnlyList<SaleCode> saleCodes)
        {
            return lineItemsToWrite.Select(item =>
            {
                var vendorInvoiceItem = VendorInvoiceItem.Create(
                    item.Item.PartNumber,
                    item.Item.Description,
                    item.Item.Manufacturer is null
                        ? null
                        : manufacturers.FirstOrDefault(manufacturer => manufacturer.Id == item.Item.Manufacturer.Id),
                    item.Item.SaleCode is null
                        ? null
                        : saleCodes.FirstOrDefault(saleCode => saleCode.Id == item.Item.SaleCode.Id))
                    .Value;

                return VendorInvoiceLineItem.Create(
                    item.Type,
                    vendorInvoiceItem,
                    item.Quantity,
                    item.Cost,
                    item.Core,
                    item.PONumber,
                    item.TransactionDate)
                    .Value;
            }).ToList();
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

        public static IReadOnlyList<VendorInvoiceLineItemToRead> ConvertWriteToReadDtos(IReadOnlyList<VendorInvoiceLineItemToWrite> updatedLineItems)
        {
            return
                updatedLineItems is null
                ? new List<VendorInvoiceLineItemToRead>()
                : updatedLineItems.Select(lineItem => new VendorInvoiceLineItemToRead()
                {
                    Core = lineItem.Core,
                    Cost = lineItem.Cost,
                    Id = lineItem.Id,
                    PONumber = lineItem?.PONumber,
                    Quantity = lineItem.Quantity,
                    TransactionDate = lineItem.TransactionDate,
                    Type = lineItem.Type,
                    Item = VendorInvoiceItemHelper.ConvertWriteToReadDto(lineItem.Item)
                }).ToList();
        }
    }
}