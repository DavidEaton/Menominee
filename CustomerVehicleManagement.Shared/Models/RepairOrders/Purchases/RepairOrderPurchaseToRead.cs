using CustomerVehicleManagement.Domain.Entities.RepairOrders;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CustomerVehicleManagement.Shared.Models.RepairOrders.Purchases
{
    public class RepairOrderPurchaseToRead
    {
        public long Id { get; set; }
        public long RepairOrderItemId { get; set; }
        public long VendorId { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public string PONumber { get; set; }
        public string VendorInvoiceNumber { get; set; }
        public string VendorPartNumber { get; set; }

        public static IReadOnlyList<RepairOrderPurchaseToRead> ConvertToDto(IList<RepairOrderPurchase> purchases)
        {
            return purchases
                .Select(purchase =>
                        ConvertToDto(purchase))
                .ToList();
        }

        private static RepairOrderPurchaseToRead ConvertToDto(RepairOrderPurchase purchase)
        {
            if (purchase != null)
            {
                return new RepairOrderPurchaseToRead()
                {
                    Id = purchase.Id,
                    RepairOrderItemId = purchase.RepairOrderItemId,
                    VendorId = purchase.VendorId,
                    PurchaseDate = purchase.PurchaseDate,
                    PONumber = purchase.PONumber,
                    VendorInvoiceNumber = purchase.VendorInvoiceNumber,
                    VendorPartNumber = purchase.VendorPartNumber
                };
            }

            return null;
        }
    }
}