using CustomerVehicleManagement.Domain.Entities.RepairOrders;
using CustomerVehicleManagement.Shared.Models.Manufacturers;
using CustomerVehicleManagement.Shared.Models.ProductCodes;
using CustomerVehicleManagement.Shared.Models.RepairOrders.Items;
using CustomerVehicleManagement.Shared.Models.RepairOrders.Payments;
using CustomerVehicleManagement.Shared.Models.RepairOrders.Purchases;
using CustomerVehicleManagement.Shared.Models.RepairOrders.SerialNumbers;
using CustomerVehicleManagement.Shared.Models.RepairOrders.Services;
using CustomerVehicleManagement.Shared.Models.RepairOrders.Taxes;
using CustomerVehicleManagement.Shared.Models.RepairOrders.Techs;
using CustomerVehicleManagement.Shared.Models.RepairOrders.Warranties;
using CustomerVehicleManagement.Shared.Models.SaleCodes;
using Menominee.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CustomerVehicleManagement.Shared.Models.RepairOrders
{
    public class RepairOrderHelper
    {
        // TODO: Move this logic down into the domain aggregate class: Domain.Entities.RepairOrders.RepairOrderItem.cs
        private static bool WarrantyRequired(RepairOrderItemToWrite item)
        {
            if ((item.PartType == PartType.Part || item.PartType == PartType.Tire) && item.QuantitySold > 0)
            {
                // TODO: check if this part's product code requires warranty
                // if (ProductCodeRequireWwarranty(item))
                return true;
            }
            return false;
        }

        // TODO: Move this logic down into the domain aggregate class: Domain.Entities.RepairOrders.RepairOrderItem.cs
        private static bool PurchaseInfoRequired(RepairOrderItemToWrite item)
        {
            return ((item.PartType == PartType.Part || item.PartType == PartType.Tire) && item.QuantitySold > 0 /*&& item.IsBuyout*/);
        }

        // TODO: Move "Missing" methods into separate class
        public static int WarrantyRequiredMissingCount(IList<RepairOrderServiceToWrite> services)
        {
            int missingWarrantyCount = 0;

            foreach (var service in services)
            {
                foreach (var item in service?.Items)
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
                foreach (var item in service?.Items)
                {
                    if (item?.SerialNumbers is null || !SerialNumberRequired(item))
                        continue;

                    // If QuantitySold is fractional, and part requires serial number,
                    // that's an invalid state we must prevent.
                    // TODO: This is a business rule. Business rules should live in the domain layer.
                    if (IsFractional(item.QuantitySold))
                        continue;

                    missingSerialNumberCount += item.SerialNumbers.Count(
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
                foreach (var item in service?.Items)
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
                foreach (var item in service.Items)
                {
                    foreach (var serialNumber in item.SerialNumbers)
                    {
                        list.Add(new SerialNumberListItem()
                        {
                            ItemId = item.Id,
                            Description = item.Description,
                            PartNumber = item.PartNumber,
                            RepairOrderItemId = item.RepairOrderServiceId,
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
                foreach (var item in service.Items)
                {
                    foreach (var warranty in item.Warranties)
                    {
                        list.Add(new WarrantyListItem()
                        {
                            Type = warranty.Type,
                            Description = item.Description,
                            PartNumber = item.PartNumber,
                            RepairOrderItemId = warranty.RepairOrderItemId,
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
                foreach (var item in service.Items)
                {
                    // check if purchase info is required on this item
                    if (PurchaseInfoRequired(item))

                        foreach (var purchase in item.Purchases)
                        {
                            list.Add(new PurchaseListItem()
                            {
                                Description = item.Description,
                                VendorCost = item.Cost,
                                RepairOrderItemId = purchase.RepairOrderItemId,
                                PartNumber = item.PartNumber,
                                VendorName = purchase.VendorId.ToString(),
                                Quantity = item.QuantitySold,
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

        public static RepairOrderToWrite CovertReadToWriteDto(RepairOrderToRead repairOrder)
        {
            var repairOrderToWrite = new RepairOrderToWrite
            {
                RepairOrderNumber = repairOrder.RepairOrderNumber,
                InvoiceNumber = repairOrder.InvoiceNumber,
                CustomerName = repairOrder.CustomerName,
                Vehicle = repairOrder.Vehicle,
                PartsTotal = repairOrder.PartsTotal,
                LaborTotal = repairOrder.LaborTotal,
                DiscountTotal = repairOrder.DiscountTotal,
                HazMatTotal = repairOrder.HazMatTotal,
                TaxTotal = repairOrder.TaxTotal,
                ShopSuppliesTotal = repairOrder.ShopSuppliesTotal,
                Total = repairOrder.Total,
                DateCreated = repairOrder.DateCreated,
                DateInvoiced = repairOrder.DateInvoiced,
                DateModified = repairOrder.DateModified,
                Services = ProjectServices(repairOrder.Services),
                Payments = ProjectPayments(repairOrder.Payments),
                Taxes = ProjectTaxes(repairOrder.Taxes)
            };

            return repairOrderToWrite;
        }

        // TODO: Move "Missing" methods into separate class
        private static IEnumerable<RepairOrderPurchaseToRead> MissingRequiredPurchases(RepairOrderItemToRead item)
        {
            var list = new List<RepairOrderPurchaseToRead>();

            if (IsFractional(item.QuantitySold))
                return list;

            int missingCount = (int)item.QuantitySold - item.Purchases.Count;

            for (var i = 0; i < missingCount; i++)
            {
                var newPurchase = new RepairOrderPurchaseToRead()
                {
                    RepairOrderItemId = item.Id
                    //Quantity = 0
                };

                list.Add(newPurchase);
            }

            return list;
        }

        // TODO: Move "Missing" methods into separate class
        private static List<RepairOrderWarrantyToRead> MissingRequiredWarranties(RepairOrderItemToRead item)
        {
            var list = new List<RepairOrderWarrantyToRead>();

            if (IsFractional(item.QuantitySold))
                return list;

            int missingCount = (int)item.QuantitySold - item.Warranties.Count;

            for (var i = 0; i < missingCount; i++)
            {
                var newWarranty = new RepairOrderWarrantyToRead()
                {
                    RepairOrderItemId = item.Id,
                    Quantity = 0
                };

                list.Add(newWarranty);
            }

            return list;
        }

        // TODO: Move "Missing" methods into separate class
        private static List<RepairOrderSerialNumberToRead> MissingRequiredSerialNumbers(RepairOrderItemToRead item)
        {
            var list = new List<RepairOrderSerialNumberToRead>();
            // If QuantitySold is fractional, and part requires serial number,
            // that's an invalid state we must prevent.
            // TODO: This is a business rule. Business rules shouuld live in the domain layer.
            if (IsFractional(item.QuantitySold))
                return list;

            int missingCount = (int)item.QuantitySold - item.SerialNumbers.Count;

            for (var i = 0; i < missingCount; i++)
            {
                var newSerialNumber = new RepairOrderSerialNumberToRead()
                {
                    RepairOrderItemId = item.Id,
                    SerialNumber = string.Empty
                };

                list.Add(newSerialNumber);
            }

            return list;
        }

        // TODO: Move this logic down into the domain aggregate class: Domain.Entities.RepairOrders.RepairOrderItem.cs
        private static bool SerialNumberRequired(RepairOrderItemToWrite item)
        {
            if ((item.PartType == PartType.Part || item.PartType == PartType.Tire) && item.QuantitySold > 0)
            {
                // check if this part's product code requires serial numbers
                // if (ProductCodeRequiresSerialNumber(item))
                // TODO: Implement ProductCodeRequiresSerialNumber(item)
                return true;
            }
            return false;
        }

        public static RepairOrder ConvertWriteDtoToEntity(RepairOrderToWrite repairOrder)
        {
            if (repairOrder is null)
                return null;

            var result = new RepairOrder()
            {
                CustomerName = repairOrder.CustomerName,
                DiscountTotal = repairOrder.DiscountTotal,
                HazMatTotal = repairOrder.HazMatTotal,
                InvoiceNumber = repairOrder.InvoiceNumber,
                LaborTotal = repairOrder.LaborTotal,
                PartsTotal = repairOrder.PartsTotal,
                RepairOrderNumber = repairOrder.RepairOrderNumber,
                ShopSuppliesTotal = repairOrder.ShopSuppliesTotal,
                TaxTotal = repairOrder.TaxTotal,
                Total = repairOrder.Total,
                Vehicle = repairOrder.Vehicle
            };

            if (repairOrder.DateCreated.HasValue)
                result.DateCreated = repairOrder.DateCreated.Value;

            if (repairOrder.DateInvoiced.HasValue)
                result.DateInvoiced = repairOrder.DateInvoiced.Value;

            if (repairOrder.DateModified.HasValue)
                result.DateModified = repairOrder.DateModified.Value;

            if (repairOrder?.Payments.Count > 0)
                result.Payments = ProjectPayments(repairOrder.Payments);

            if (repairOrder?.Services.Count > 0)
                result.Services = ProjectServices(repairOrder.Services);

            if (repairOrder?.Taxes.Count > 0)
                result.Taxes = ProjectTaxes(repairOrder.Taxes);

            return result;
        }

        private static List<RepairOrderService> ProjectServices(List<RepairOrderServiceToWrite> services)
        {
            return services?.Select(TransformService()).ToList()
                ?? new List<RepairOrderService>();
        }

        private static Func<RepairOrderServiceToWrite, RepairOrderService> TransformService()
        {
            return service =>
                            new RepairOrderService()
                            {
                                DiscountTotal = service.DiscountTotal,
                                IsCounterSale = service.IsCounterSale,
                                IsDeclined = service.IsDeclined,
                                LaborTotal = service.LaborTotal,
                                PartsTotal = service.PartsTotal,
                                RepairOrderId = service.RepairOrderId,
                                SaleCode = service.SaleCode,
                                ServiceName = service.ServiceName,
                                ShopSuppliesTotal = service.ShopSuppliesTotal,
                                TaxTotal = service.TaxTotal,
                                Total = service.Total,
                                Items = ProjectServiceItems(service.Items),
                                Taxes = ProjectServiceTaxes(service.Taxes),
                                Techs = ProjectServiceTechnicians(service.Techs)
                            };
        }

        private static List<RepairOrderTax> ProjectTaxes(List<RepairOrderTaxToWrite> taxes)
        {
            return taxes?.Select(TransformTax()).ToList()
                ?? new List<RepairOrderTax>();
        }

        private static Func<RepairOrderTaxToWrite, RepairOrderTax> TransformTax()
        {
            return tax =>
                            new RepairOrderTax()
                            {
                                LaborTax = tax.LaborTax,
                                LaborTaxRate = tax.LaborTaxRate,
                                PartTax = tax.PartTax,
                                PartTaxRate = tax.PartTaxRate,
                                RepairOrderId = tax.RepairOrderId,
                                TaxId = tax.TaxId
                            };
        }

        private static List<RepairOrderPayment> ProjectPayments(List<RepairOrderPaymentToWrite> payments)
        {
            return payments?.Select(TransformPayment()).ToList()
                ?? new List<RepairOrderPayment>();
        }

        private static Func<RepairOrderPaymentToWrite, RepairOrderPayment> TransformPayment()
        {
            return payment =>
                            new RepairOrderPayment()
                            {
                                Amount = payment.Amount,
                                PaymentMethod = payment.PaymentMethod,
                                RepairOrderId = payment.RepairOrderId
                            };
        }

        private static Func<RepairOrderItemToWrite, RepairOrderItem> TransformServiceItem()
        {
            return item =>
                            new RepairOrderItem()
                            {
                                Core = item.Core,
                                Cost = item.Cost,
                                Description = item.Description,
                                DiscountEach = item.DiscountEach,
                                DiscountType = item.DiscountType,
                                IsCounterSale = item.IsCounterSale,
                                IsDeclined = item.IsDeclined,
                                LaborEach = item.LaborEach,
                                LaborType = item.LaborType,
                                //Manufacturer = item.Manufacturer,
                                ManufacturerId = item.ManufacturerId,
                                PartNumber = item.PartNumber,
                                PartType = item.PartType,
                                ProductCode = ProductCodeHelper.ConvertWriteDtoToEntity(item.ProductCode),
                                ProductCodeId = item.ProductCodeId,
                                QuantitySold = item.QuantitySold,
                                RepairOrderServiceId = item.RepairOrderServiceId,
                                SaleCode = SaleCodeHelper.ConvertWriteDtoToEntity(item.SaleCode),
                                SaleCodeId = item.SaleCodeId,
                                SaleType = item.SaleType,
                                SellingPrice = item.SellingPrice,
                                Total = item.Total,
                                SerialNumbers = ProjectServiceItemSerialNumbers(item.SerialNumbers),
                                Taxes = ProjectServiceItemTaxes(item.Taxes),
                                Warranties = ProjectServiceItemWarranties(item.Warranties),
                                Purchases = ProjectServiceItemPurchases(item.Purchases)
                            };
        }

        private static List<RepairOrderTech> ProjectServiceTechnicians(IList<RepairOrderTechToWrite> technicians)
        {
            return technicians?.Select(TransformServiceTechnician()).ToList()
                ?? new List<RepairOrderTech>();
        }

        private static Func<RepairOrderTechToWrite, RepairOrderTech> TransformServiceTechnician()
        {
            return technician =>
                            new RepairOrderTech()
                            {
                                RepairOrderServiceId = technician.RepairOrderServiceId,
                                TechnicianId = technician.TechnicianId
                            };
        }

        private static List<RepairOrderServiceTax> ProjectServiceTaxes(IList<RepairOrderServiceTaxToWrite> taxes)
        {
            return taxes?.Select(TransformServiceTax()).ToList()
                ?? new List<RepairOrderServiceTax>();
        }

        private static Func<RepairOrderServiceTaxToWrite, RepairOrderServiceTax> TransformServiceTax()
        {
            return tax =>
                            new RepairOrderServiceTax()
                            {
                                LaborTax = tax.LaborTax,
                                LaborTaxRate = tax.LaborTaxRate,
                                PartTax = tax.PartTax,
                                PartTaxRate = tax.PartTaxRate,
                                RepairOrderServiceId = tax.RepairOrderServiceId,
                                TaxId = tax.TaxId
                            };
        }

        private static List<RepairOrderPurchase> ProjectServiceItemPurchases(IList<RepairOrderPurchaseToWrite> purchases)
        {
            return purchases?.Select(TransformServiceItemPurchase()).ToList()
                ?? new List<RepairOrderPurchase>();
        }

        private static Func<RepairOrderPurchaseToWrite, RepairOrderPurchase> TransformServiceItemPurchase()
        {
            return purchase =>
                            new RepairOrderPurchase()
                            {
                                PONumber = purchase.PONumber,
                                PurchaseDate = purchase.PurchaseDate.Value,
                                RepairOrderItemId = purchase.RepairOrderItemId,
                                VendorId = purchase.VendorId,
                                VendorInvoiceNumber = purchase.VendorInvoiceNumber,
                                VendorPartNumber = purchase.VendorPartNumber
                            };
        }

        private static Func<RepairOrderPurchase, RepairOrderPurchaseToRead> TransformServiceItemPurchaseToRead()
        {
            return purchase =>
                            new RepairOrderPurchaseToRead()
                            {
                                Id = purchase.Id,
                                PONumber = purchase.PONumber,
                                PurchaseDate = purchase.PurchaseDate,
                                RepairOrderItemId = purchase.RepairOrderItemId,
                                VendorId = purchase.VendorId,
                                VendorInvoiceNumber = purchase.VendorInvoiceNumber,
                                VendorPartNumber = purchase.VendorPartNumber
                            };
        }

        private static List<RepairOrderWarranty> ProjectServiceItemWarranties(IList<RepairOrderWarrantyToWrite> warranties)
        {
            return warranties?.Select(TransformServiceItemWarranty()).ToList()
                ?? new List<RepairOrderWarranty>();
        }

        private static Func<RepairOrderWarrantyToWrite, RepairOrderWarranty> TransformServiceItemWarranty()
        {
            return warranty =>
                            new RepairOrderWarranty()
                            {
                                NewWarranty = warranty.NewWarranty,
                                OriginalInvoiceId = warranty.OriginalInvoiceId,
                                OriginalWarranty = warranty.OriginalWarranty,
                                Quantity = warranty.Quantity,
                                RepairOrderItemId = warranty.RepairOrderItemId,
                                Type = warranty.Type
                            };
        }

        private static List<RepairOrderItemTax> ProjectServiceItemTaxes(IList<RepairOrderItemTaxToWrite> taxes)
        {
            return taxes?.Select(tax =>
                                new RepairOrderItemTax()
                                {
                                    LaborTax = tax.LaborTax,
                                }).ToList()
                ?? new List<RepairOrderItemTax>();
        }

        private static List<RepairOrderSerialNumber> ProjectServiceItemSerialNumbers(IList<RepairOrderSerialNumberToWrite> serialNumbers)
        {
            return serialNumbers?.Select(serialNumber =>
                                        new RepairOrderSerialNumber()
                                        {
                                            RepairOrderItemId = serialNumber.RepairOrderItemId,
                                            SerialNumber = serialNumber.SerialNumber
                                        }).ToList()
                ?? new List<RepairOrderSerialNumber>();
        }

        private static List<RepairOrderTaxToWrite> ProjectTaxes(List<RepairOrderTaxToRead> taxes)
        {
            return taxes?.Select(TransformTaxToWrite()).ToList()
                ?? new List<RepairOrderTaxToWrite>();
        }

        private static Func<RepairOrderTaxToRead, RepairOrderTaxToWrite> TransformTaxToWrite()
        {
            return tax =>
                            new RepairOrderTaxToWrite()
                            {
                                LaborTax = tax.LaborTax,
                                LaborTaxRate = tax.LaborTaxRate,
                                PartTax = tax.PartTax,
                                PartTaxRate = tax.PartTaxRate,
                                RepairOrderId = tax.RepairOrderId,
                                TaxId = tax.TaxId
                            };
        }

        private static List<RepairOrderPaymentToWrite> ProjectPayments(List<RepairOrderPaymentToRead> payments)
        {
            return payments?.Select(TransformPaymentToWrite()).ToList()
                ?? new List<RepairOrderPaymentToWrite>();
        }

        private static Func<RepairOrderPaymentToRead, RepairOrderPaymentToWrite> TransformPaymentToWrite()
        {
            return payment =>
                            new RepairOrderPaymentToWrite()
                            {
                                Id = payment.Id,
                                RepairOrderId = payment.RepairOrderId,
                                PaymentMethod = payment.PaymentMethod,
                                Amount = payment.Amount

                            };
        }

        private static List<RepairOrderWarrantyToWrite> ProjectServiceItemWarranties(IList<RepairOrderWarrantyToRead> warranties)
        {
            return warranties?.Select(TransformServiceItemWarrantyToWrite()).ToList()
                ?? new List<RepairOrderWarrantyToWrite>();
        }

        private static Func<RepairOrderWarrantyToRead, RepairOrderWarrantyToWrite> TransformServiceItemWarrantyToWrite()
        {
            return warranty =>
                            new RepairOrderWarrantyToWrite()
                            {
                                Id = warranty.Id,
                                NewWarranty = warranty.NewWarranty,
                                OriginalInvoiceId = warranty.OriginalInvoiceId,
                                OriginalWarranty = warranty.OriginalWarranty,
                                Quantity = warranty.Quantity,
                                RepairOrderItemId = warranty.RepairOrderItemId,
                                Type = warranty.Type
                            };
        }

        private static List<RepairOrderItemTaxToWrite> ProjectServiceItemTaxes(IList<RepairOrderItemTaxToRead> taxes)
        {
            return taxes?.Select(TransformServiceItemTaxToWrite()).ToList()
                ?? new List<RepairOrderItemTaxToWrite>();
        }

        private static Func<RepairOrderItemTaxToRead, RepairOrderItemTaxToWrite> TransformServiceItemTaxToWrite()
        {
            return tax =>
                            new RepairOrderItemTaxToWrite()
                            {
                                LaborTax = tax.LaborTax,
                                LaborTaxRate = tax.LaborTaxRate,
                                PartTax = tax.PartTax,
                                PartTaxRate = tax.PartTaxRate,
                                RepairOrderItemId = tax.RepairOrderItemId,
                                TaxId = tax.TaxId
                            };
        }

        private static List<RepairOrderPurchaseToWrite> ProjectServiceItemPurchases(IList<RepairOrderPurchaseToRead> purchases)
        {
            return purchases?.Select(TransformServiceItemPurchaseToWrite()).ToList()
                ?? new List<RepairOrderPurchaseToWrite>();
        }

        private static Func<RepairOrderPurchaseToRead, RepairOrderPurchaseToWrite> TransformServiceItemPurchaseToWrite()
        {
            return purchase =>
                            new RepairOrderPurchaseToWrite()
                            {
                                Id = purchase.Id,
                                RepairOrderItemId = purchase.RepairOrderItemId,
                                PONumber = purchase.PONumber,
                                PurchaseDate = purchase.PurchaseDate,
                                VendorId = purchase.VendorId,
                                VendorInvoiceNumber = purchase.VendorInvoiceNumber,
                                VendorPartNumber = purchase.VendorPartNumber
                            };
        }

        private static List<RepairOrderSerialNumberToWrite> ProjectServiceItemSerialNumbers(IList<RepairOrderSerialNumberToRead> serialNumbers)
        {
            return serialNumbers?.Select(TransformServiceItemSerialNumberToWrite()).ToList()
                ?? new List<RepairOrderSerialNumberToWrite>();
        }

        private static Func<RepairOrderSerialNumberToRead, RepairOrderSerialNumberToWrite> TransformServiceItemSerialNumberToWrite()
        {
            return serialNumber =>
                            new RepairOrderSerialNumberToWrite()
                            {
                                Id = serialNumber.Id,
                                RepairOrderItemId = serialNumber.RepairOrderItemId,
                                SerialNumber = serialNumber.SerialNumber
                            };
        }

        private static List<RepairOrderServiceToWrite> ProjectServices(List<RepairOrderServiceToRead> services)
        {
            return services?.Select(TransformServiceToWrite()).ToList()
                ?? new List<RepairOrderServiceToWrite>();
        }

        private static Func<RepairOrderServiceToRead, RepairOrderServiceToWrite> TransformServiceToWrite()
        {
            return service =>
                            new RepairOrderServiceToWrite()
                            {
                                Id = service.Id,
                                RepairOrderId = service.RepairOrderId,
                                ServiceName = service.ServiceName,
                                SaleCode = service.SaleCode,
                                IsCounterSale = service.IsCounterSale,
                                IsDeclined = service.IsDeclined,
                                PartsTotal = service.PartsTotal,
                                LaborTotal = service.LaborTotal,
                                DiscountTotal = service.DiscountTotal,
                                TaxTotal = service.TaxTotal,
                                ShopSuppliesTotal = service.ShopSuppliesTotal,
                                Total = service.Total,
                                Items = ProjectServiceItems(service.Items)
                            };
        }

        private static List<RepairOrderItemToWrite> ProjectServiceItems(IList<RepairOrderItemToRead> items)
        {
            if (items is null)
                return new List<RepairOrderItemToWrite>();

            foreach (var item in items)
            {
                item.SerialNumbers.AddRange(MissingRequiredSerialNumbers(item));
                item.Warranties.AddRange(MissingRequiredWarranties(item));
                item.Purchases.AddRange(MissingRequiredPurchases(item));
            }

            return items.Select(TransformServiceItemToWrite()).ToList();
        }

        private static Func<RepairOrderItemToRead, RepairOrderItemToWrite> TransformServiceItemToWrite()
        {
            return item =>
                            new RepairOrderItemToWrite()
                            {
                                Id = item.Id,
                                Core = item.Core,
                                Cost = item.Cost,
                                Description = item.Description,
                                DiscountEach = item.DiscountEach,
                                DiscountType = item.DiscountType,
                                IsCounterSale = item.IsCounterSale,
                                IsDeclined = item.IsDeclined,
                                LaborEach = item.LaborEach,
                                LaborType = item.LaborType,
                                Manufacturer = ManufacturerHelper.ConvertReadToWriteDto(item.Manufacturer),
                                ManufacturerId = item.ManufacturerId,
                                PartNumber = item.PartNumber,
                                PartType = item.PartType,
                                ProductCode = ProductCodeHelper.ConvertReadToWriteDto(item.ProductCode),
                                ProductCodeId = item.ProductCodeId,
                                QuantitySold = item.QuantitySold,
                                RepairOrderServiceId = item.RepairOrderServiceId,
                                SaleCode = SaleCodeHelper.ConvertReadToWriteDto(item.SaleCode),
                                SaleCodeId = item.SaleCodeId,
                                SaleType = item.SaleType,
                                SellingPrice = item.SellingPrice,
                                Total = item.Total,
                                SerialNumbers = ProjectServiceItemSerialNumbers(item.SerialNumbers),
                                Warranties = ProjectServiceItemWarranties(item.Warranties),
                                Taxes = ProjectServiceItemTaxes(item.Taxes),
                                Purchases = ProjectServiceItemPurchases(item.Purchases)
                            };
        }

        private static IList<RepairOrderTechToRead> ProjectServiceTechnicians(IList<RepairOrderTech> technicians)
        {
            return technicians?.Select(TransformServiceTechnicianToRead()).ToList()
                ?? new List<RepairOrderTechToRead>();
        }

        private static Func<RepairOrderTech, RepairOrderTechToRead> TransformServiceTechnicianToRead()
        {
            return technician =>
                            new RepairOrderTechToRead()
                            {
                                Id = technician.Id,
                                RepairOrderServiceId = technician.RepairOrderServiceId,
                                TechnicianId = technician.TechnicianId
                            };
        }

        private static List<RepairOrderItem> ProjectServiceItems(List<RepairOrderItemToWrite> items)
        {
            return items?.Select(TransformServiceItem()).ToList()
                ?? new List<RepairOrderItem>();
        }

        private static List<RepairOrderItemToRead> ProjectServiceItems(IList<RepairOrderItem> items)
        {
            return items?.Select(TransformServiceItemToRead()).ToList()
                ?? new List<RepairOrderItemToRead>();
        }

        private static Func<RepairOrderItem, RepairOrderItemToRead> TransformServiceItemToRead()
        {
            return item =>
                            new RepairOrderItemToRead()
                            {
                                Id = item.Id,
                                RepairOrderServiceId = item.RepairOrderServiceId,
                                Manufacturer = ManufacturerHelper.ConvertEntityToReadDto(item.Manufacturer),
                                ManufacturerId = item.ManufacturerId,
                                PartNumber = item.PartNumber,
                                Description = item.Description,
                                SaleCode = SaleCodeHelper.ConvertEntityToReadDto(item.SaleCode),
                                SaleCodeId = item.SaleCodeId,
                                ProductCode = ProductCodeHelper.ConvertEntityToReadDto(item.ProductCode),
                                ProductCodeId = item.ProductCodeId,
                                SaleType = item.SaleType,
                                PartType = item.PartType,
                                IsDeclined = item.IsDeclined,
                                IsCounterSale = item.IsCounterSale,
                                QuantitySold = item.QuantitySold,
                                SellingPrice = item.SellingPrice,
                                LaborType = item.LaborType,
                                LaborEach = item.LaborEach,
                                DiscountType = item.DiscountType,
                                DiscountEach = item.DiscountEach,
                                Cost = item.Cost,
                                Core = item.Core,
                                Total = item.Total,
                                SerialNumbers = ProjectServiceItemSerialNumbers(item.SerialNumbers),
                                Warranties = ProjectServiceItemWarranties(item.Warranties),
                                Taxes = ProjectServiceItemTaxes(item.Taxes),
                                Purchases = ProjectServiceItemPurchases(item.Purchases)
                            };
        }

        private static List<RepairOrderPurchaseToRead> ProjectServiceItemPurchases(IList<RepairOrderPurchase> purchases)
        {
            return purchases?.Select(TransformServiceItemPurchaseToRead()).ToList()
                ?? new List<RepairOrderPurchaseToRead>();
        }

        private static List<RepairOrderServiceToRead> ProjectServices(List<RepairOrderService> services)
        {
            return services?.Select(TransformServiceToRead()).ToList()
                ?? new List<RepairOrderServiceToRead>();
        }

        private static Func<RepairOrderService, RepairOrderServiceToRead> TransformServiceToRead()
        {
            return service =>
                            new RepairOrderServiceToRead()
                            {
                                Id = service.Id,
                                RepairOrderId = service.RepairOrderId,
                                ServiceName = service.ServiceName,
                                SaleCode = service.SaleCode,
                                IsCounterSale = service.IsCounterSale,
                                IsDeclined = service.IsDeclined,
                                PartsTotal = service.PartsTotal,
                                LaborTotal = service.LaborTotal,
                                DiscountTotal = service.DiscountTotal,
                                TaxTotal = service.TaxTotal,
                                ShopSuppliesTotal = service.ShopSuppliesTotal,
                                Total = service.Total,
                                Items = ProjectServiceItems(service.Items),
                                Techs = ProjectServiceTechnicians(service.Techs),
                                Taxes = ProjectServiceTaxes(service.Taxes)
                            };
        }

        public static RepairOrderToRead Transform(RepairOrder repairOrder)
        {
            if (repairOrder is not null)
            {
                return new RepairOrderToRead()
                {
                    Id = repairOrder.Id,
                    RepairOrderNumber = repairOrder.RepairOrderNumber,
                    InvoiceNumber = repairOrder.InvoiceNumber,
                    CustomerName = repairOrder.CustomerName,
                    Vehicle = repairOrder.Vehicle,
                    PartsTotal = repairOrder.PartsTotal,
                    LaborTotal = repairOrder.LaborTotal,
                    DiscountTotal = repairOrder.DiscountTotal,
                    TaxTotal = repairOrder.TaxTotal,
                    HazMatTotal = repairOrder.HazMatTotal,
                    ShopSuppliesTotal = repairOrder.ShopSuppliesTotal,
                    Total = repairOrder.Total,
                    DateCreated = repairOrder.DateCreated,
                    DateModified = repairOrder.DateModified,
                    DateInvoiced = repairOrder.DateInvoiced,
                    Services = ProjectServices(repairOrder.Services),
                    Taxes = ProjectTaxes(repairOrder.Taxes),
                    Payments = ProjectPayments(repairOrder.Payments)
                };
            }

            return null;
        }

        private static List<RepairOrderItemTaxToRead> ProjectServiceItemTaxes(IList<RepairOrderItemTax> taxes)
        {
            return taxes?.Select(TransformServiceItemTax()).ToList()
                ?? new List<RepairOrderItemTaxToRead>();
        }

        private static Func<RepairOrderItemTax, RepairOrderItemTaxToRead> TransformServiceItemTax()
        {
            return tax =>
                            new RepairOrderItemTaxToRead()
                            {
                                Id = tax.Id,
                                RepairOrderItemId = tax.RepairOrderItemId,
                                TaxId = tax.TaxId,
                                PartTaxRate = tax.PartTaxRate,
                                LaborTaxRate = tax.LaborTaxRate,
                                PartTax = tax.PartTax,
                                LaborTax = tax.LaborTax
                            };
        }

        private static List<RepairOrderWarrantyToRead> ProjectServiceItemWarranties(IList<RepairOrderWarranty> warranties)
        {
            return warranties?.Select(TransformServiceItemWarrantyToRead()).ToList()
                ?? new List<RepairOrderWarrantyToRead>();
        }

        private static Func<RepairOrderWarranty, RepairOrderWarrantyToRead> TransformServiceItemWarrantyToRead()
        {
            return warranty =>
                            new RepairOrderWarrantyToRead()
                            {
                                Id = warranty.Id,
                                RepairOrderItemId = warranty.RepairOrderItemId,
                                Quantity = warranty.Quantity,
                                Type = warranty.Type,
                                NewWarranty = warranty.NewWarranty,
                                OriginalWarranty = warranty.OriginalWarranty,
                                OriginalInvoiceId = warranty.OriginalInvoiceId
                            };
        }

        private static List<RepairOrderSerialNumberToRead> ProjectServiceItemSerialNumbers(IList<RepairOrderSerialNumber> serialNumbers)
        {
            return serialNumbers?.Select(TransformServiceItemSerialNumber()).ToList()
                ?? new List<RepairOrderSerialNumberToRead>();
        }

        private static Func<RepairOrderSerialNumber, RepairOrderSerialNumberToRead> TransformServiceItemSerialNumber()
        {
            return serialNumber =>
                            new RepairOrderSerialNumberToRead()
                            {
                                Id = serialNumber.Id,
                                RepairOrderItemId = serialNumber.RepairOrderItemId,
                                SerialNumber = serialNumber.SerialNumber
                            };
        }

        private static List<RepairOrderTaxToRead> ProjectTaxes(IList<RepairOrderTax> taxes)
        {
            return taxes?.Select(TransformTaxToRead()).ToList()
                ?? new List<RepairOrderTaxToRead>();
        }

        private static Func<RepairOrderTax, RepairOrderTaxToRead> TransformTaxToRead()
        {
            return tax =>
                        new RepairOrderTaxToRead()
                        {
                            Id = tax.Id,
                            RepairOrderId = tax.RepairOrderId,
                            TaxId = tax.TaxId,
                            PartTaxRate = tax.PartTaxRate,
                            LaborTaxRate = tax.LaborTaxRate,
                            PartTax = tax.PartTax,
                            LaborTax = tax.LaborTax
                        };
        }

        private static IList<RepairOrderServiceTaxToRead> ProjectServiceTaxes(IList<RepairOrderServiceTax> taxes)
        {
            return taxes?.Select(TransformServiceTaxToRead()).ToList()
                ?? new List<RepairOrderServiceTaxToRead>();
        }

        private static Func<RepairOrderServiceTax, RepairOrderServiceTaxToRead> TransformServiceTaxToRead()
        {
            return tax =>
                            new RepairOrderServiceTaxToRead()
                            {
                                Id = tax.Id,
                                RepairOrderServiceId = tax.RepairOrderServiceId,
                                TaxId = tax.TaxId,
                                PartTaxRate = tax.PartTaxRate,
                                LaborTaxRate = tax.LaborTaxRate,
                                PartTax = tax.PartTax,
                                LaborTax = tax.LaborTax
                            };
        }

        private static List<RepairOrderPaymentToRead> ProjectPayments(IList<RepairOrderPayment> payments)
        {
            return payments?.Select(TransformPaymentToRead()).ToList()
                ?? new List<RepairOrderPaymentToRead>();
        }

        private static Func<RepairOrderPayment, RepairOrderPaymentToRead> TransformPaymentToRead()
        {
            return payment =>
                            new RepairOrderPaymentToRead()
                            {
                                Id = payment.Id,
                                RepairOrderId = payment.RepairOrderId,
                                PaymentMethod = payment.PaymentMethod,
                                Amount = payment.Amount
                            };
        }

        public static RepairOrderToReadInList TransformInList(RepairOrder repairOrder)
        {
            if (repairOrder is not null)
            {
                return new RepairOrderToReadInList
                {
                    Id = repairOrder.Id,
                    RepairOrderNumber = repairOrder.RepairOrderNumber,
                    InvoiceNumber = repairOrder.InvoiceNumber,
                    CustomerName = repairOrder.CustomerName,
                    Vehicle = repairOrder.Vehicle,
                    PartsTotal = repairOrder.PartsTotal,
                    LaborTotal = repairOrder.LaborTotal,
                    DiscountTotal = repairOrder.DiscountTotal,
                    TaxTotal = repairOrder.TaxTotal,
                    HazMatTotal = repairOrder.HazMatTotal,
                    ShopSuppliesTotal = repairOrder.ShopSuppliesTotal,
                    Total = repairOrder.Total,
                    DateCreated = repairOrder.DateCreated,
                    DateModified = repairOrder.DateModified,
                    DateInvoiced = repairOrder.DateInvoiced
                };
            }

            return null;
        }
    }
}