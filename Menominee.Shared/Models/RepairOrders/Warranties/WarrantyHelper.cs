using Menominee.Domain.Entities.RepairOrders;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Menominee.Shared.Models.RepairOrders.Warranties
{
    public class WarrantyHelper
    {
        public static List<RepairOrderWarrantyToWrite> ConvertReadToWriteDtos(IReadOnlyList<RepairOrderWarrantyToRead> warranties)
        {
            return warranties?.Select(
                warranty =>
                new RepairOrderWarrantyToWrite()
                {
                    Id = warranty.Id,
                    NewWarranty = warranty.NewWarranty,
                    OriginalInvoiceId = warranty.OriginalInvoiceId,
                    OriginalWarranty = warranty.OriginalWarranty,
                    Quantity = warranty.Quantity,
                    Type = warranty.Type
                }).ToList()
            ?? new List<RepairOrderWarrantyToWrite>();
        }

        public static IReadOnlyList<RepairOrderWarrantyToRead> ConvertToReadDtos(IReadOnlyList<RepairOrderWarranty> warranties)
        {
            return warranties?.Select(
                warranty =>
                new RepairOrderWarrantyToRead()
                {
                    Id = warranty.Id,
                    NewWarranty = warranty.NewWarranty,
                    OriginalInvoiceId = warranty.OriginalInvoiceId,
                    OriginalWarranty = warranty.OriginalWarranty,
                    Quantity = warranty.Quantity,
                    Type = warranty.Type
                }).ToList()
            ?? new List<RepairOrderWarrantyToRead>();
        }

        internal static List<RepairOrderWarrantyToWrite> ConvertToWriteDtos(IReadOnlyList<RepairOrderWarranty> warranties)
        {
            return warranties?.Select(
                warranty =>
                new RepairOrderWarrantyToWrite()
                {
                    Id = warranty.Id,
                    NewWarranty = warranty.NewWarranty,
                    OriginalInvoiceId = warranty.OriginalInvoiceId,
                    OriginalWarranty = warranty.OriginalWarranty,
                    Quantity = warranty.Quantity,
                    Type = warranty.Type
                }).ToList()
            ?? new List<RepairOrderWarrantyToWrite>();
        }
    }
}
