using CustomerVehicleManagement.Shared.Models.RepairOrders;
using CustomerVehicleManagement.Shared.Models.RepairOrders.Items;
using CustomerVehicleManagement.Shared.Models.RepairOrders.Payments;
using CustomerVehicleManagement.Shared.Models.RepairOrders.SerialNumbers;
using CustomerVehicleManagement.Shared.Models.RepairOrders.Services;
using CustomerVehicleManagement.Shared.Models.RepairOrders.Taxes;
using CustomerVehicleManagement.Shared.Models.RepairOrders.Warranties;
using Menominee.Common.Enums;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Shared.Helpers
{
    public class RepairOrderHelper
    {
        public static RepairOrderToWrite ConvertReadDtoToWriteDto(RepairOrderToRead repairOrder)
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
                Services = ServicesReadDtoToWriteDto(repairOrder),
                Payments = PaymentsReadDtoToWriteDto(repairOrder),
                Taxes = TaxesReadDtoToWriteDto(repairOrder)
            };

            return repairOrderToWrite;
        }

        private static IList<RepairOrderTaxToWrite> TaxesReadDtoToWriteDto(RepairOrderToRead repairOrder)
        {
            var result = new List<RepairOrderTaxToWrite>();
            foreach (var tax in repairOrder?.Taxes)
                result.Add(ReadDtoToWriteDto(tax));

            return result;
        }

        private static RepairOrderTaxToWrite ReadDtoToWriteDto(RepairOrderTaxToRead tax)
        {
            return new RepairOrderTaxToWrite()
            {
                LaborTax = tax.LaborTax,
                LaborTaxRate = tax.LaborTaxRate,
                PartTax = tax.PartTax,
                PartTaxRate = tax.PartTaxRate,
                RepairOrderId = tax.RepairOrderId,
                SequenceNumber = tax.SequenceNumber,
                TaxId = tax.TaxId
            };
        }

        private static IList<RepairOrderPaymentToWrite> PaymentsReadDtoToWriteDto(RepairOrderToRead repairOrder)
        {
            var result = new List<RepairOrderPaymentToWrite>();
            foreach (var payment in repairOrder?.Payments)
                result.Add(ReadDtoToWriteDto(payment));

            return result;
        }

        private static RepairOrderPaymentToWrite ReadDtoToWriteDto(RepairOrderPaymentToRead payment)
        {
            return new RepairOrderPaymentToWrite()
            {
                Id = payment.Id,
                RepairOrderId = payment.RepairOrderId,
                PaymentMethod = payment.PaymentMethod,
                Amount = payment.Amount
            };
        }

        private static IList<RepairOrderServiceToWrite> ServicesReadDtoToWriteDto(RepairOrderToRead repairOrder)
        {
            var result = new List<RepairOrderServiceToWrite>();
            foreach (var service in repairOrder?.Services)
                result.Add(ReadDtoToWriteDto(service));

            return result;
        }

        private static void AddMissingSerialNumbers(RepairOrderItemToWrite item)
        {
            if (item?.SerialNumbers is null || !SerialNumberRequired(item))
                return;

            // If QuantitySold is fractional, and part requires serial number,
            // that's an invalid state we must prevent.
            // TODO: This is a business rule. Business rules shouuld live in the domain layer.
            if (IsFractional(item.QuantitySold))
                return;

            int quantitySold = (int)item.QuantitySold;

            int matchingItemSerialNumbersCount = item.SerialNumbers.Count;

            int missingItemSerialNumbersCount = quantitySold - matchingItemSerialNumbersCount;
            for (var i = 0; i < missingItemSerialNumbersCount; i++)
            {
                var serialNumber = new RepairOrderSerialNumberToWrite
                {
                    RepairOrderItemId = item.RepairOrderServiceId
                };

                item.SerialNumbers.Add(serialNumber);
            }
        }

        private static bool IsFractional(double quantitySold)
        {
            return !((quantitySold % 1) == 0);
        }

        //private static void WarrantiesReadDtoToWriteDto(RepairOrderItemToRead item, RepairOrderItemToWrite itemToWrite)
        //{
        //    foreach (var warranty in item?.Warranties)
        //    {
        //        itemToWrite.Warranties.Add(new RepairOrderWarrantyToWrite()
        //        {
        //            Id = warranty.Id,
        //            RepairOrderItemId = warranty.RepairOrderItemId,
        //            SequenceNumber = warranty.SequenceNumber,
        //            Quantity = warranty.Quantity,
        //            Type = warranty.Type,
        //            NewWarranty = warranty.NewWarranty,
        //            OriginalWarranty = warranty.OriginalWarranty,
        //            OriginalInvoiceId = warranty.OriginalInvoiceId
        //        });
        //    }
        //}

        //private static void ReadDtoToWriteDto(RepairOrderItemToRead item, RepairOrderItemToWrite itemToWrite)
        //{
        //    foreach (var tax in item?.Taxes)
        //    {
        //        itemToWrite.Taxes.Add(new RepairOrderItemTaxToWrite()
        //        {
        //            RepairOrderItemId = tax.RepairOrderItemId,
        //            SequenceNumber = tax.SequenceNumber,
        //            TaxId = tax.TaxId,
        //            PartTaxRate = tax.PartTaxRate,
        //            LaborTaxRate = tax.LaborTaxRate,
        //            PartTax = tax.PartTax,
        //            LaborTax = tax.LaborTax
        //        });
        //    }
        //}

        //private static void SerialNumbersReadDtoToWriteDto(RepairOrderItemToRead item, RepairOrderItemToWrite itemToWrite)
        //{
        //    foreach (var serialNumber in item?.SerialNumbers)
        //    {
        //        itemToWrite.SerialNumbers.Add(new RepairOrderSerialNumberToWrite()
        //        {
        //            RepairOrderItemId = serialNumber.RepairOrderItemId,
        //            SerialNumber = serialNumber.SerialNumber
        //        });
        //    }
        //}

        private static RepairOrderItemToWrite ReadDtoToWriteDto(RepairOrderItemToRead repairOrderItem)
        {
            return new RepairOrderItemToWrite
            {
                RepairOrderServiceId = repairOrderItem.RepairOrderServiceId,
                SequenceNumber = repairOrderItem.SequenceNumber,
                //Manufacturer = item.Manufacturer,
                ManufacturerId = repairOrderItem.ManufacturerId,
                PartNumber = repairOrderItem.PartNumber,
                Description = repairOrderItem.Description,
                //SaleCode = item.SaleCode,
                SaleCodeId = repairOrderItem.SaleCodeId,
                //ProductCode = item.ProductCode,
                ProductCodeId = repairOrderItem.ProductCodeId,
                SaleType = repairOrderItem.SaleType,
                PartType = repairOrderItem.PartType,
                IsDeclined = repairOrderItem.IsDeclined,
                IsCounterSale = repairOrderItem.IsCounterSale,
                QuantitySold = repairOrderItem.QuantitySold,
                SellingPrice = repairOrderItem.SellingPrice,
                LaborType = repairOrderItem.LaborType,
                LaborEach = repairOrderItem.LaborEach,
                DiscountType = repairOrderItem.DiscountType,
                DiscountEach = repairOrderItem.DiscountEach,
                Cost = repairOrderItem.Cost,
                Core = repairOrderItem.Core,
                Total = repairOrderItem.Total,
                SerialNumbers = ReadDtoToWriteDto(repairOrderItem.SerialNumbers),
                Taxes = ReadDtoToWriteDto(repairOrderItem.Taxes),
                Warranties = ReadDtoToWriteDto(repairOrderItem.Warranties)
            };
        }

        private static IList<RepairOrderWarrantyToWrite> ReadDtoToWriteDto(IReadOnlyList<RepairOrderWarrantyToRead> warranties)
        {
            var result = new List<RepairOrderWarrantyToWrite>();

            foreach (var warranty in warranties)
            {
                result.Add(new RepairOrderWarrantyToWrite()
                {
                    NewWarranty = warranty.NewWarranty,
                    OriginalInvoiceId = warranty.OriginalInvoiceId,
                    OriginalWarranty = warranty.OriginalWarranty,
                    Quantity = warranty.Quantity,
                    RepairOrderItemId = warranty.RepairOrderItemId,
                    SequenceNumber = warranty.SequenceNumber,
                    Type = warranty.Type
                });
            }

            return result;
        }

        private static IList<RepairOrderItemTaxToWrite> ReadDtoToWriteDto(IReadOnlyList<RepairOrderItemTaxToRead> taxes)
        {
            var result = new List<RepairOrderItemTaxToWrite>();

            foreach (var tax in taxes)
            {
                result.Add(new RepairOrderItemTaxToWrite()
                {
                    LaborTax = tax.LaborTax,
                    LaborTaxRate = tax.LaborTaxRate,
                    PartTax = tax.PartTax,
                    PartTaxRate = tax.PartTaxRate,
                    RepairOrderItemId = tax.RepairOrderItemId,
                    SequenceNumber = tax.SequenceNumber,
                    TaxId = tax.TaxId
                });
            }

            return result;
        }

        private static IList<RepairOrderSerialNumberToWrite> ReadDtoToWriteDto(IReadOnlyList<RepairOrderSerialNumberToRead> serialNumbers)
        {
            var result = new List<RepairOrderSerialNumberToWrite>();

            foreach (var serialNumber in serialNumbers)
            {
                result.Add(new RepairOrderSerialNumberToWrite()
                {
                    RepairOrderItemId = serialNumber.RepairOrderItemId,
                    SerialNumber = serialNumber.SerialNumber
                });
            }

            return result;
        }

        private static RepairOrderServiceToWrite ReadDtoToWriteDto(RepairOrderServiceToRead repairOrderService)
        {
            return new RepairOrderServiceToWrite
            {
                RepairOrderId = repairOrderService.RepairOrderId,
                SequenceNumber = repairOrderService.SequenceNumber,
                ServiceName = repairOrderService.ServiceName,
                SaleCode = repairOrderService.SaleCode,
                IsCounterSale = repairOrderService.IsCounterSale,
                IsDeclined = repairOrderService.IsDeclined,
                PartsTotal = repairOrderService.PartsTotal,
                LaborTotal = repairOrderService.LaborTotal,
                DiscountTotal = repairOrderService.DiscountTotal,
                TaxTotal = repairOrderService.TaxTotal,
                ShopSuppliesTotal = repairOrderService.ShopSuppliesTotal,
                Total = repairOrderService.Total,
                Items = ReadDtoToWriteDto(repairOrderService.Items)
            };
        }

        private static IList<RepairOrderItemToWrite> ReadDtoToWriteDto(IReadOnlyList<RepairOrderItemToRead> items)
        {
            var result = new List<RepairOrderItemToWrite>();

            foreach (var item in items)
            {
                result.Add(new RepairOrderItemToWrite()
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
                    //ProductCode = item.ProductCode,
                    ProductCodeId = item.ProductCodeId,
                    QuantitySold = item.QuantitySold,
                    RepairOrderServiceId = item.RepairOrderServiceId,
                    //SaleCode = item.SaleCode,
                    SaleCodeId = item.SaleCodeId,
                    SaleType = item.SaleType,
                    SellingPrice = item.SellingPrice,
                    SequenceNumber = item.SequenceNumber,
                    Total = item.Total,
                    SerialNumbers = ReadDtoToWriteDto(item.SerialNumbers),
                    Taxes = ReadDtoToWriteDto(item.Taxes),
                    Warranties = ReadDtoToWriteDto(item.Warranties)
                });
            }

            return result;
        }


        // TODO: Move this logic down into the domain aggregate class: Domain.Entities.RepairOrders.RepairOrderItem.cs
        private static bool SerialNumberRequired(RepairOrderItemToWrite item)
        {
            if ((item.PartType == PartType.Part || item.PartType == PartType.Tire) && item.QuantitySold > 0)
            {
                // check if this part's product code requires serial numbers
                // if (ProductCodeRequiresSerialNumber())
                return true;
            }
            return false;
        }

        // TODO: Move this logic down into the domain aggregate class: Domain.Entities.RepairOrders.RepairOrderItem.cs
        private static bool SerialNumberRequired(RepairOrderItemToRead item)
        {
            if ((item.PartType == PartType.Part || item.PartType == PartType.Tire) && item.QuantitySold > 0)
            {
                // check if this part's product code requires serial numbers
                // if (ProductCodeRequiresSerialNumber())
                return true;
            }
            return false;
        }

        public static int MissingSerialNumberCount(IList<RepairOrderServiceToWrite> servicesToWrite)
        {
            int missingSerialNumberCount = 0;

            foreach (var service in servicesToWrite)
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

                    int quantitySold = (int)item.QuantitySold;

                    int matchingItemSerialNumbersCount = item.SerialNumbers.Count;

                    missingSerialNumberCount += quantitySold - matchingItemSerialNumbersCount;
                }
            }

            return missingSerialNumberCount;
        }

        public static int MissingSerialNumberCount(IList<RepairOrderServiceToRead> services)
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

                    int quantitySold = (int)item.QuantitySold;

                    int matchingItemSerialNumbersCount = item.SerialNumbers.Count;

                    missingSerialNumberCount += quantitySold - matchingItemSerialNumbersCount;
                }
            }

            return missingSerialNumberCount;
        }
    }
}
