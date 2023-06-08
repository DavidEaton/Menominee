using CustomerVehicleManagement.Domain.Entities.RepairOrders;
using CustomerVehicleManagement.Shared.Models.Payables.Vendors;
using System.Collections.Generic;
using System.Linq;

namespace CustomerVehicleManagement.Shared.Models.RepairOrders.Purchases
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
    }
}
