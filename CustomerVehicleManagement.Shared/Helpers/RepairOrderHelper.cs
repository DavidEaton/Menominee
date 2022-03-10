using CustomerVehicleManagement.Shared.Models.RepairOrders;
using CustomerVehicleManagement.Shared.Models.RepairOrders.Items;
using CustomerVehicleManagement.Shared.Models.RepairOrders.Payments;
using CustomerVehicleManagement.Shared.Models.RepairOrders.SerialNumbers;
using CustomerVehicleManagement.Shared.Models.RepairOrders.Services;
using CustomerVehicleManagement.Shared.Models.RepairOrders.Taxes;
using CustomerVehicleManagement.Shared.Models.RepairOrders.Techs;
using CustomerVehicleManagement.Shared.Models.RepairOrders.Warranties;

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
                DateModified = repairOrder.DateModified          
            };

            if (repairOrder?.Services?.Count > 0)
            {
                ServicesReadDtoToWriteDto(repairOrder, repairOrderToWrite);
            }

            if (repairOrder?.Payments.Count > 0)
            {
                PaymentsReadDtoToWriteDto(repairOrder, repairOrderToWrite);
            }

            return repairOrderToWrite;
        }

        private static void PaymentsReadDtoToWriteDto(RepairOrderToRead repairOrder, RepairOrderToWrite repairOrderToWrite)
        {
            foreach (var payment in repairOrder?.Payments)
            {
                repairOrderToWrite.Payments.Add(new RepairOrderPaymentToWrite()
                {
                    Id = payment.Id,
                    RepairOrderId = payment.RepairOrderId,
                    PaymentMethod = payment.PaymentMethod,
                    Amount = payment.Amount
                });
            }
        }

        private static void ServicesReadDtoToWriteDto(RepairOrderToRead repairOrder, RepairOrderToWrite repairOrderToWrite)
        {
            foreach (var service in repairOrder?.Services)
            {
                RepairOrderServiceToWrite serviceToWrite = ReadDtoToWriteDto(service);

                if (service.Items?.Count > 0)
                    ItemsReadDtoToWriteDto(service, serviceToWrite);

                if (service.Techs?.Count > 0)
                    TechsReadDtoToWriteDto(service, serviceToWrite);

                if (service.Taxes?.Count > 0)
                    TaxesReadDtoToWriteDto(service, serviceToWrite);

                repairOrderToWrite.Services.Add(serviceToWrite);
            }
        }

        private static void TechsReadDtoToWriteDto(RepairOrderServiceToRead service, RepairOrderServiceToWrite serviceToWrite)
        {
            foreach (var tech in service?.Techs)
            {
                serviceToWrite.Techs.Add(new RepairOrderTechToWrite()
                {
                    Id = tech.Id,
                    RepairOrderServiceId = tech.RepairOrderServiceId,
                    TechnicianId = tech.TechnicianId
                });
            }
        }

        private static void TaxesReadDtoToWriteDto(RepairOrderServiceToRead service, RepairOrderServiceToWrite serviceToWrite)
        {
            foreach (var tax in service?.Taxes)
            {
                serviceToWrite.Taxes.Add(new RepairOrderServiceTaxToWrite()
                {
                    Id = tax.Id,
                    RepairOrderServiceId = tax.RepairOrderServiceId,
                    SequenceNumber = tax.SequenceNumber,
                    TaxId = tax.TaxId,
                    PartTaxRate = tax.PartTaxRate,
                    LaborTaxRate = tax.LaborTaxRate,
                    PartTax = tax.PartTax,
                    LaborTax = tax.LaborTax
                });
            }
        }

        private static void ItemsReadDtoToWriteDto(RepairOrderServiceToRead service, RepairOrderServiceToWrite serviceToWrite)
        {
            foreach (var item in service?.Items)
            {
                RepairOrderItemToWrite itemToWrite = ReadDtoToWriteDto(item);

                if (item.SerialNumbers?.Count > 0)
                    SerialNumbersReadDtoToWriteDto(item, itemToWrite);

                if (item.Taxes?.Count > 0)
                    TaxesReadDtoToWriteDto(item, itemToWrite);

                if (item.Warranties?.Count > 0)
                    WarrantiesReadDtoToWriteDto(item, itemToWrite);

                serviceToWrite.Items.Add(itemToWrite);
            }
        }

        private static void WarrantiesReadDtoToWriteDto(RepairOrderItemToRead item, RepairOrderItemToWrite itemToWrite)
        {
            foreach (var warranty in item?.Warranties)
            {
                itemToWrite.Warranties.Add(new RepairOrderWarrantyToWrite()
                {
                    Id = warranty.Id,
                    RepairOrderItemId = warranty.RepairOrderItemId,
                    SequenceNumber = warranty.SequenceNumber,
                    Quantity = warranty.Quantity,
                    Type = warranty.Type,
                    NewWarranty = warranty.NewWarranty,
                    OriginalWarranty = warranty.OriginalWarranty,
                    OriginalInvoiceId = warranty.OriginalInvoiceId
                });
            }
        }

        private static void TaxesReadDtoToWriteDto(RepairOrderItemToRead item, RepairOrderItemToWrite itemToWrite)
        {
            foreach (var tax in item?.Taxes)
            {
                itemToWrite.Taxes.Add(new RepairOrderItemTaxToWrite()
                {
                    Id = tax.Id,
                    RepairOrderItemId = tax.RepairOrderItemId,
                    SequenceNumber = tax.SequenceNumber,
                    TaxId = tax.TaxId,
                    PartTaxRate = tax.PartTaxRate,
                    LaborTaxRate = tax.LaborTaxRate,
                    PartTax = tax.PartTax,
                    LaborTax = tax.LaborTax
                });
            }
        }

        private static void SerialNumbersReadDtoToWriteDto(RepairOrderItemToRead item, RepairOrderItemToWrite itemToWrite)
        {
            foreach (var serialNumber in item?.SerialNumbers)
            {
                itemToWrite.SerialNumbers.Add(new RepairOrderSerialNumberToWrite()
                {
                    RepairOrderItemId = serialNumber.RepairOrderItemId,
                    SerialNumber = serialNumber.SerialNumber
                });
            }
        }

        private static RepairOrderItemToWrite ReadDtoToWriteDto(RepairOrderItemToRead repairOrderItem)
        {
            return new RepairOrderItemToWrite
            {
                Id = repairOrderItem.Id,
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
                Total = repairOrderItem.Total
            };
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
                Total = repairOrderService.Total
            };
        }
    }
}
