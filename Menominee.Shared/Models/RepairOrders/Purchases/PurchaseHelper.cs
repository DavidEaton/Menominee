using Menominee.Domain.Entities.RepairOrders;
using Menominee.Shared.Models.Payables.Vendors;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Menominee.Shared.Models.RepairOrders.Purchases
{
    public class PurchaseHelper
    {
        public static List<RepairOrderPurchaseToWrite> ConvertReadToWriteDtos(IReadOnlyList<RepairOrderPurchaseToRead> purchases)
        {
            return purchases?.Select(
                purchase =>
                new RepairOrderPurchaseToWrite()
                {
                    Id = purchase.Id,
                    PONumber = purchase.PONumber,
                    PurchaseDate = purchase.PurchaseDate,
                    Vendor = purchase.Vendor,
                    VendorInvoiceNumber = purchase.VendorInvoiceNumber,
                    VendorPartNumber = purchase.VendorPartNumber
                }).ToList()
            ?? new List<RepairOrderPurchaseToWrite>();

        }

        public static List<RepairOrderPurchaseToWrite> ConvertWriteToReadDtos(IReadOnlyList<RepairOrderPurchaseToRead> purchases)
        {
            return purchases?.Select(
                purchase =>
                new RepairOrderPurchaseToWrite()
                {
                    Id = purchase.Id,
                    PONumber = purchase.PONumber,
                    PurchaseDate = purchase.PurchaseDate,
                    Vendor = purchase.Vendor,
                    VendorInvoiceNumber = purchase.VendorInvoiceNumber,
                    VendorPartNumber = purchase.VendorPartNumber
                }).ToList()
            ?? new List<RepairOrderPurchaseToWrite>();
        }

        public static IReadOnlyList<RepairOrderPurchaseToRead> ConvertToReadDtos(IReadOnlyList<RepairOrderPurchase> purchases)
        {
            return purchases?.Select(
                purchase =>
                new RepairOrderPurchaseToRead()
                {
                    Id = purchase.Id,
                    PONumber = purchase.PONumber,
                    PurchaseDate = purchase.PurchaseDate,
                    VendorInvoiceNumber = purchase.VendorInvoiceNumber,
                    VendorPartNumber = purchase.VendorPartNumber,
                    Vendor = VendorHelper.ConvertToReadDto(purchase.Vendor)
                }).ToList()
            ?? new List<RepairOrderPurchaseToRead>();
        }

        internal static List<RepairOrderPurchaseToWrite> ConvertToWriteDtos(IReadOnlyList<RepairOrderPurchase> purchases)
        {
            return purchases?.Select(
                purchase =>
                new RepairOrderPurchaseToWrite()
                {
                    Id = purchase.Id,
                    PONumber = purchase.PONumber,
                    PurchaseDate = purchase.PurchaseDate,
                    Vendor = VendorHelper.ConvertToReadDto(purchase.Vendor),
                    VendorInvoiceNumber = purchase.VendorInvoiceNumber,
                    VendorPartNumber = purchase.VendorPartNumber
                }).ToList()
            ?? new List<RepairOrderPurchaseToWrite>();
        }
    }
}
