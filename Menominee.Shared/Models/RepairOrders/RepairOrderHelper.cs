using Menominee.Domain.Entities.RepairOrders;
using Menominee.Domain.Enums;
using Menominee.Shared.Models.Customers;
using Menominee.Shared.Models.RepairOrders.Items;
using Menominee.Shared.Models.RepairOrders.LineItems.Item;
using Menominee.Shared.Models.RepairOrders.Payments;
using Menominee.Shared.Models.RepairOrders.Purchases;
using Menominee.Shared.Models.RepairOrders.SerialNumbers;
using Menominee.Shared.Models.RepairOrders.Services;
using Menominee.Shared.Models.RepairOrders.Statuses;
using Menominee.Shared.Models.RepairOrders.Taxes;
using Menominee.Shared.Models.RepairOrders.Warranties;
using Menominee.Shared.Models.Vehicles;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Menominee.Shared.Models.RepairOrders
{
    public class RepairOrderHelper
    {
        // TODO: Move this logic down into the domain aggregate class: Domain.Entities.RepairOrders.RepairOrderItem.cs
        private static bool WarrantyRequired(RepairOrderLineItemToWrite lineItem)
        {
            if ((lineItem.Item.PartType == PartType.Part || lineItem.Item.PartType == PartType.Tire) && lineItem.QuantitySold > 0)
            {
                // TODO: check if this part's product code requires warranty
                // if (ProductCodeRequireWwarranty(item))
                return true;
            }
            return false;
        }

        // TODO: Move this logic down into the domain aggregate class: Domain.Entities.RepairOrders.RepairOrderItem.cs
        private static bool PurchaseInfoRequired(RepairOrderLineItemToWrite lineItem)
        {
            return lineItem.Item.PartType == PartType.Part
                || ((lineItem.Item.PartType == PartType.Tire)
                && (lineItem.QuantitySold > 0 /*&& item.IsBuyout*/));
        }

        // TODO: Move "Missing" methods into separate class
        public static int WarrantyRequiredMissingCount(IList<RepairOrderServiceToWrite> services)
        {
            int missingWarrantyCount = 0;

            foreach (var service in services)
            {
                foreach (var item in service?.LineItems)
                {
                    if (item?.Warranties is null || !WarrantyRequired(item))
                        continue;

                    if (IsFractional(item.QuantitySold))
                        continue;

                    missingWarrantyCount += item.Warranties.Count(
                        warranty =>
                        warranty.Quantity == 0);
                }
            }

            return missingWarrantyCount;
        }

        // TODO: Move "Missing" methods into separate class
        public static int SerialNumberRequiredMissingCount(IList<RepairOrderServiceToWrite> services)
        {
            int missingSerialNumberCount = 0;

            foreach (var service in services)
            {
                foreach (var lineItem in service?.LineItems)
                {
                    if (lineItem?.SerialNumbers is null || !SerialNumberRequired(lineItem))
                        continue;

                    // If QuantitySold is fractional, and part requires serial number,
                    // that's an invalid state we must prevent.
                    // TODO: This is a business rule. Business rules should live in the domain layer.
                    if (IsFractional(lineItem.QuantitySold))
                        continue;

                    missingSerialNumberCount += lineItem.SerialNumbers.Count(
                        serialNumber =>
                        string.IsNullOrWhiteSpace(serialNumber.SerialNumber));
                }
            }

            return missingSerialNumberCount;
        }

        // TODO: Move "Missing" methods into separate class
        public static int PurchaseRequiredMissingCount(IList<RepairOrderServiceToWrite> services)
        {
            int missingSerialNumberCount = 0;

            foreach (var service in services)
            {
                foreach (var item in service?.LineItems)
                {
                    if (item?.Warranties is null || !PurchaseInfoRequired(item))
                        continue;

                    if (IsFractional(item.QuantitySold))
                        continue;

                    missingSerialNumberCount += item.SerialNumbers.Count(
                        serialNumber =>
                        string.IsNullOrWhiteSpace(serialNumber.SerialNumber));
                }
            }

            return missingSerialNumberCount;
        }

        // TODO: Move "Build" methods into separate class
        public static List<SerialNumberListItem> BuildSerialNumberList(IList<RepairOrderServiceToWrite> services)
        {
            var list = new List<SerialNumberListItem>();
            foreach (var service in services)
            {
                foreach (var lineItem in service.LineItems)
                {
                    foreach (var serialNumber in lineItem.SerialNumbers)
                    {
                        list.Add(new SerialNumberListItem()
                        {
                            Description = lineItem.Item.Description,
                            PartNumber = lineItem.Item.PartNumber,
                            SerialNumberType = serialNumber
                        });
                    }
                }
            }

            return list;
        }

        // TODO: Move "Build" methods into separate class
        public static List<WarrantyListItem> BuildWarrantyList(List<RepairOrderServiceToWrite> services)
        {
            var list = new List<WarrantyListItem>();
            foreach (var service in services)
            {
                foreach (var lineItem in service.LineItems)
                {
                    foreach (var warranty in lineItem.Warranties)
                    {
                        list.Add(new WarrantyListItem()
                        {
                            Type = warranty.Type,
                            Description = lineItem.Item.Description,
                            PartNumber = lineItem.Item.PartNumber,
                            WarrantyType = warranty,
                            Quantity = warranty.Quantity
                        });
                    }
                }
            }

            return list;
        }

        // TODO: Move "Build" methods into separate class
        public static List<PurchaseListItem> BuildPurchaseList(List<RepairOrderServiceToWrite> services)
        {
            var list = new List<PurchaseListItem>();
            foreach (var service in services)
            {
                foreach (var lineItem in service.LineItems)
                {
                    // check if purchase info is required on this item
                    if (PurchaseInfoRequired(lineItem))

                        foreach (var purchase in lineItem.Purchases)
                        {
                            list.Add(new PurchaseListItem()
                            {
                                Description = lineItem.Item.Description,
                                VendorCost = lineItem.Cost,
                                PartNumber = lineItem.Item.PartNumber,
                                Vendor = purchase.Vendor,
                                Quantity = lineItem.QuantitySold,
                                PurchaseDate = DateTime.Today
                            }); ;
                        }
                }
            }

            return list;
        }

        private static bool IsFractional(double quantitySold)
        {
            return !(quantitySold % 1 == 0);
        }

        public static RepairOrderToWrite ConvertReadToWriteDto(RepairOrderToRead repairOrder)
        {
            return new RepairOrderToWrite()
            {
                Id = repairOrder.Id,
                RepairOrderNumber = repairOrder.RepairOrderNumber,
                InvoiceNumber = repairOrder.InvoiceNumber,
                Customer = CustomerHelper.ConvertReadToWriteDto(repairOrder.Customer),
                Vehicle = VehicleHelper.ConvertReadToWriteDto(repairOrder.Vehicle),
                PartsTotal = repairOrder.PartsTotal,
                LaborTotal = repairOrder.LaborTotal,
                DiscountTotal = repairOrder.DiscountTotal,
                HazMatTotal = repairOrder.HazMatTotal,
                TaxTotal = repairOrder.TaxTotal,
                ShopSuppliesTotal = repairOrder.ShopSuppliesTotal,
                Total = repairOrder.Total,
                DateCreated = repairOrder.DateCreated,
                DateModified = repairOrder.DateModified,
                Statuses = StatusHelper.ConvertReadToWriteDtos(repairOrder.Statuses),
                Services = ServiceHelper.ConvertReadToWriteDtos(repairOrder.Services),
                Payments = PaymentHelper.ConvertReadToWriteDtos(repairOrder.Payments),
                Taxes = RepairOrderTaxHelper.ConvertReadToWriteDtos(repairOrder.Taxes)
            };
        }

        // TODO: Move "Missing" methods into separate class
        private static IEnumerable<RepairOrderPurchaseToRead> MissingRequiredPurchases(RepairOrderItemToRead item)
        {
            var list = new List<RepairOrderPurchaseToRead>();

            //if (IsFractional(item.QuantitySold))
            //    return list;

            //int missingCount = (int)item.QuantitySold - item.Purchases.Count;

            //for (var i = 0; i < missingCount; i++)
            //{
            //    var newPurchase = new RepairOrderPurchaseToRead()
            //    {
            //        RepairOrderItemId = item.Id
            //        //Quantity = 0
            //    };

            //    list.Add(newPurchase);
            //}

            return list;
        }

        // TODO: Move "Missing" methods into separate class
        private static List<RepairOrderWarrantyToRead> MissingRequiredWarranties(RepairOrderItemToRead item)
        {
            var list = new List<RepairOrderWarrantyToRead>();

            //if (IsFractional(item.QuantitySold))
            //    return list;

            //int missingCount = (int)item.QuantitySold - item.Warranties.Count;

            //for (var i = 0; i < missingCount; i++)
            //{
            //    var newWarranty = new RepairOrderWarrantyToRead()
            //    {
            //        RepairOrderItemId = item.Id,
            //        Quantity = 0
            //    };

            //    list.Add(newWarranty);
            //}

            return list;
        }

        // TODO: Move "Missing" methods into separate class
        private static List<RepairOrderSerialNumberToRead> MissingRequiredSerialNumbers(RepairOrderItemToRead item)
        {
            var list = new List<RepairOrderSerialNumberToRead>();
            // If QuantitySold is fractional, and part requires serial number,
            // that's an invalid state we must prevent.
            // TODO: This is a business rule. Business rules shouuld live in the domain layer.
            //if (IsFractional(item.QuantitySold))
            //    return list;

            //int missingCount = (int)item.QuantitySold - item.SerialNumbers.Count;

            //for (var i = 0; i < missingCount; i++)
            //{
            //    var newSerialNumber = new RepairOrderSerialNumberToRead()
            //    {
            //        RepairOrderItemId = item.Id,
            //        SerialNumber = string.Empty
            //    };

            //    list.Add(newSerialNumber);
            //}

            return list;
        }

        // TODO: Move this logic down into the domain aggregate class: Domain.Entities.RepairOrders.RepairOrderItem.cs
        private static bool SerialNumberRequired(RepairOrderLineItemToWrite lineItem)
        {
            if ((lineItem.Item.PartType == PartType.Part || lineItem.Item.PartType == PartType.Tire) && lineItem.QuantitySold > 0)
            {
                // check if this part's product code requires serial numbers
                // if (ProductCodeRequiresSerialNumber(item))
                // TODO: Implement ProductCodeRequiresSerialNumber(item)
                return true;
            }
            return false;
        }

        public static RepairOrderToRead ConvertToReadDto(RepairOrder repairOrder)
        {
            return repairOrder is null
                ? new RepairOrderToRead()
                : new RepairOrderToRead()
                {
                    Id = repairOrder.Id,
                    RepairOrderNumber = repairOrder.RepairOrderNumber,
                    InvoiceNumber = repairOrder.InvoiceNumber,
                    Customer = CustomerHelper.ConvertToReadDto(repairOrder.Customer),
                    Vehicle = VehicleHelper.ConvertToReadDto(repairOrder.Vehicle),
                    PartsTotal = repairOrder.PartsTotal,
                    LaborTotal = repairOrder.LaborTotal,
                    DiscountTotal = repairOrder.DiscountTotal,
                    TaxTotal = repairOrder.TaxTotal,
                    HazMatTotal = repairOrder.ExciseFeesTotal,
                    ShopSuppliesTotal = repairOrder.ShopSuppliesTotal,
                    Total = repairOrder.Total,
                    DateCreated = repairOrder.DateCreated,
                    DateModified = repairOrder.DateModified,
                    Statuses = StatusHelper.ConvertToReadDtos(repairOrder.Statuses),
                    Services = ServiceHelper.ConvertToReadDtos(repairOrder.Services),
                    Taxes = RepairOrderTaxHelper.ConvertToReadDtos(repairOrder.Taxes),
                    Payments = PaymentHelper.ConvertToReadDtos(repairOrder.Payments)
                };
        }

        public static RepairOrderToReadInList ConvertToReadInListDto(RepairOrder repairOrder)
        {
            return repairOrder is null
                ? new RepairOrderToReadInList()
                : new RepairOrderToReadInList()
                {
                    Id = repairOrder.Id,
                    RepairOrderNumber = repairOrder.RepairOrderNumber,
                    InvoiceNumber = repairOrder.InvoiceNumber,
                    Customer = CustomerHelper.ConvertToReadDto(repairOrder.Customer),
                    Vehicle = VehicleHelper.ConvertToReadDto(repairOrder.Vehicle),
                    PartsTotal = repairOrder.PartsTotal,
                    LaborTotal = repairOrder.LaborTotal,
                    DiscountTotal = repairOrder.DiscountTotal,
                    TaxTotal = repairOrder.TaxTotal,
                    HazMatTotal = repairOrder.ExciseFeesTotal,
                    ShopSuppliesTotal = repairOrder.ShopSuppliesTotal,
                    Total = repairOrder.Total,
                    DateCreated = repairOrder.DateCreated,
                    DateModified = repairOrder.DateModified,
                    DateInvoiced = repairOrder.DateInvoiced
                };
        }
    }
}