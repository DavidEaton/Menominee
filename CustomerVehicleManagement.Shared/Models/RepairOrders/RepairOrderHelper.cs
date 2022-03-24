using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Domain.Entities.RepairOrders;
using CustomerVehicleManagement.Shared.Models.Manufacturers;
using CustomerVehicleManagement.Shared.Models.ProductCodes;
using CustomerVehicleManagement.Shared.Models.RepairOrders.Items;
using CustomerVehicleManagement.Shared.Models.RepairOrders.Payments;
using CustomerVehicleManagement.Shared.Models.RepairOrders.SerialNumbers;
using CustomerVehicleManagement.Shared.Models.RepairOrders.Services;
using CustomerVehicleManagement.Shared.Models.RepairOrders.Taxes;
using CustomerVehicleManagement.Shared.Models.RepairOrders.Techs;
using CustomerVehicleManagement.Shared.Models.RepairOrders.Warranties;
using CustomerVehicleManagement.Shared.Models.SaleCodes;
using Menominee.Common.Enums;
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
                // check if this part's product code requires warranty
                // if (ProductCodeRequiresSerialNumber())
                return true;
            }
            return false;
        }

        public static int WarrantyRequiredMissingCount(IList<RepairOrderServiceToWrite> services)
        {
            int missingWarrantiesCount = 0;

            foreach (var service in services)
            {
                foreach (var item in service?.Items)
                {
                    if (item?.Warranties is null || !WarrantyRequired(item))
                        continue;

                    // If QuantitySold is fractional, and part requires warranty, that's an invalid
                    // state we must prevent.
                    // TODO: This is a business rule. Business rules should live in the domain layer.
                    if (IsFractional(item.QuantitySold))
                        continue;

                    int quantitySold = (int)item.QuantitySold;

                    int matchingItemWarrantiesCount = item.Warranties.Count(
                        warranty =>
                        warranty.Quantity > 0);

                    missingWarrantiesCount += quantitySold - matchingItemWarrantiesCount;
                }
            }

            return missingWarrantiesCount;
        }

        public static int SerialNumbersRequiredMissingCount(IList<RepairOrderServiceToWrite> services)
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

                    int matchingItemSerialNumbersCount = item.SerialNumbers.Count(
                        serialNumber =>
                        !string.IsNullOrWhiteSpace(serialNumber.SerialNumber));

                    missingSerialNumberCount += quantitySold - matchingItemSerialNumbersCount;
                }
            }

            return missingSerialNumberCount;
        }

        private static void AddMissingRequiredSerialNumbers(RepairOrderItemToWrite item)
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
            return !(quantitySold % 1 == 0);
        }

        public static RepairOrderToWrite CreateRepairOrder(RepairOrderToRead repairOrder)
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
                Services = CreateServices(repairOrder.Services),
                Payments = CreatePayments(repairOrder.Payments),
                Taxes = CreateTaxes(repairOrder.Taxes)
            };

            foreach (var service in repairOrderToWrite?.Services)
            {
                foreach (var item in service?.Items)
                    AddMissingRequiredSerialNumbers(item);
            }

            foreach (var service in repairOrderToWrite?.Services)
            {
                foreach (var item in service?.Items)
                    AddMissingRequiredWarranties(item);
            }

            return repairOrderToWrite;
        }

        private static void AddMissingRequiredWarranties(RepairOrderItemToWrite item)
        {
            if (item?.Warranties is null || !WarrantyRequired(item))
                return;

            // If QuantitySold is fractional, and part requires warranty, that's an invalid
            // state we must prevent.
            // TODO: This is a business rule. Business rules shouuld live in the domain layer.
            if (IsFractional(item.QuantitySold))
                return;

            int quantitySold = (int)item.QuantitySold;

            int matchingItemWarrantiesCount = item.Warranties.Count;

            int missingItemWarrantiesCount = quantitySold - matchingItemWarrantiesCount;
            for (var i = 0; i < missingItemWarrantiesCount; i++)
            {
                var warranty = new RepairOrderWarrantyToWrite
                {
                    RepairOrderItemId = item.RepairOrderServiceId,
                    Quantity = 0,
                    Type = WarrantyType.NewWarranty
                };

                item.Warranties.Add(warranty);
            }
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

        private static SaleCode CreateSaleCode(SaleCodeToWrite saleCode)
        {
            if (saleCode == null)
                return null;

            return new SaleCode()
            {
                Code = saleCode.Code,
                Name = saleCode.Name
            };
        }

        public static RepairOrder CreateRepairOrder(RepairOrderToWrite repairOrder)
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

            if (repairOrder?.Payments?.Count > 0)
                result.Payments = RepairOrderHelper.CreatePayments(repairOrder.Payments);

            if (repairOrder?.Services?.Count > 0)
                result.Services = RepairOrderHelper.CreateServices(repairOrder.Services);

            if (repairOrder?.Taxes?.Count > 0)
                result.Taxes = RepairOrderHelper.CreateTaxes(repairOrder.Taxes);

            return result;
        }

        public static IList<RepairOrderTax> CreateTaxes(IList<RepairOrderTaxToWrite> taxes)
        {
            if (taxes is null)
                return new List<RepairOrderTax>();

            return taxes.Select(tax =>
                new RepairOrderTax()
                {
                    LaborTax = tax.LaborTax,
                    LaborTaxRate = tax.LaborTaxRate,
                    PartTax = tax.PartTax,
                    PartTaxRate = tax.PartTaxRate,
                    RepairOrderId = tax.RepairOrderId,
                    TaxId = tax.TaxId
                }).ToList();
        }

        public static IList<RepairOrderPayment> CreatePayments(IList<RepairOrderPaymentToWrite> payments)
        {
            if (payments is null)
                return new List<RepairOrderPayment>();

            return payments.Select(payment =>
                new RepairOrderPayment()
                {
                    Amount = payment.Amount,
                    PaymentMethod = payment.PaymentMethod,
                    RepairOrderId = payment.RepairOrderId
                }).ToList();
        }

        public static IList<RepairOrderService> CreateServices(IList<RepairOrderServiceToWrite> services)
        {
            if (services is null)
                return new List<RepairOrderService>();

            return services.Select(service =>
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
                    Items = CreateServiceItems(service.Items),
                    Taxes = CreateServiceTaxes(service.Taxes),
                    Techs = CreateTechnicians(service.Techs)
                }).ToList();
        }

        public static IList<RepairOrderItem> CreateServiceItems(IList<RepairOrderItemToWrite> items)
        {
            if (items is null)
                return new List<RepairOrderItem>();

            return items.Select(item =>
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
                    ProductCode = CreateProductCode(item.ProductCode),
                    ProductCodeId = item.ProductCodeId,
                    QuantitySold = item.QuantitySold,
                    RepairOrderServiceId = item.RepairOrderServiceId,
                    SaleCode = CreateSaleCode(item.SaleCode),
                    SaleCodeId = item.SaleCodeId,
                    SaleType = item.SaleType,
                    SellingPrice = item.SellingPrice,
                    Total = item.Total,
                    SerialNumbers = CreateSerialNumbers(item.SerialNumbers),
                    Taxes = CreateItemTaxes(item.Taxes),
                    Warranties = CreateWarranties(item.Warranties)
                }).ToList();
        }

        public static IList<RepairOrderTech> CreateTechnicians(IList<RepairOrderTechToWrite> technicians)
        {
            if (technicians == null)
                return new List<RepairOrderTech>();

            return technicians.Select(technician =>
                new RepairOrderTech()
                {
                    RepairOrderServiceId = technician.RepairOrderServiceId,
                    TechnicianId = technician.TechnicianId
                }).ToList();
        }

        public static IList<RepairOrderServiceTax> CreateServiceTaxes(IList<RepairOrderServiceTaxToWrite> taxes)
        {
            if (taxes == null)
                return new List<RepairOrderServiceTax>();

            return taxes.Select(tax =>
                new RepairOrderServiceTax()
                {
                    LaborTax = tax.LaborTax,
                    LaborTaxRate = tax.LaborTaxRate,
                    PartTax = tax.PartTax,
                    PartTaxRate = tax.PartTaxRate,
                    RepairOrderServiceId = tax.RepairOrderServiceId,
                    TaxId = tax.TaxId
                }).ToList();
        }

        private static IList<RepairOrderWarranty> CreateWarranties(IList<RepairOrderWarrantyToWrite> warranties)
        {
            if (warranties == null)
                return new List<RepairOrderWarranty>();

            return warranties.Select(warranty => new
                RepairOrderWarranty()
            {
                NewWarranty = warranty.NewWarranty,
                OriginalInvoiceId = warranty.OriginalInvoiceId,
                OriginalWarranty = warranty.OriginalWarranty,
                Quantity = warranty.Quantity,
                RepairOrderItemId = warranty.RepairOrderItemId,
                Type = warranty.Type
            }).ToList();
        }

        private static IList<RepairOrderItemTax> CreateItemTaxes(IList<RepairOrderItemTaxToWrite> taxes)
        {
            if (taxes == null)
                return new List<RepairOrderItemTax>();

            return taxes.Select(tax =>
                new RepairOrderItemTax()
                {
                    LaborTax = tax.LaborTax,
                }).ToList();
        }

        private static IList<RepairOrderSerialNumber> CreateSerialNumbers(IList<RepairOrderSerialNumberToWrite> serialNumbers)
        {
            if (serialNumbers == null)
                return new List<RepairOrderSerialNumber>();

            return serialNumbers.Select(serialNumber =>
                new RepairOrderSerialNumber()
                {
                    RepairOrderItemId = serialNumber.RepairOrderItemId,
                    SerialNumber = serialNumber.SerialNumber
                }).ToList();
        }

        public static SaleCodeToRead CreateSaleCode(SaleCode saleCode)
        {
            if (saleCode is null)
                return null;

            return new SaleCodeToRead
            {
                Id = saleCode.Id,
                Code = saleCode.Code,
                Name = saleCode.Name
            };
        }

        private static ProductCode CreateProductCode(ProductCodeToWrite productCode)
        {
            if (productCode == null)
                return null;

            return new ProductCode()
            {
                Code = productCode.Code,
                Manufacturer = productCode.Manufacturer,
                Name = productCode.Name,
                SaleCode = productCode.SaleCode
            };
        }

        private static IList<RepairOrderTaxToWrite> CreateTaxes(IList<RepairOrderTaxToRead> taxes)
        {
            if (taxes is null)
                return new List<RepairOrderTaxToWrite>();

            return taxes.Select(tax =>
                new RepairOrderTaxToWrite()
                {
                    LaborTax = tax.LaborTax,
                    LaborTaxRate = tax.LaborTaxRate,
                    PartTax = tax.PartTax,
                    PartTaxRate = tax.PartTaxRate,
                    RepairOrderId = tax.RepairOrderId,
                    TaxId = tax.TaxId
                }).ToList();
        }

        private static IList<RepairOrderPaymentToWrite> CreatePayments(IList<RepairOrderPaymentToRead> payments)
        {
            if (payments is null)
                return new List<RepairOrderPaymentToWrite>();

            return payments.Select(payment =>
                new RepairOrderPaymentToWrite()
                {
                    Id = payment.Id,
                    RepairOrderId = payment.RepairOrderId,
                    PaymentMethod = payment.PaymentMethod,
                    Amount = payment.Amount

                }).ToList();
        }

        private static IList<RepairOrderWarrantyToWrite> CreateWarranties(IList<RepairOrderWarrantyToRead> warranties)
        {
            if (warranties is null)
                return new List<RepairOrderWarrantyToWrite>();

            return warranties.Select(warranty =>
                new RepairOrderWarrantyToWrite()
                {
                    NewWarranty = warranty.NewWarranty,
                    OriginalInvoiceId = warranty.OriginalInvoiceId,
                    OriginalWarranty = warranty.OriginalWarranty,
                    Quantity = warranty.Quantity,
                    RepairOrderItemId = warranty.RepairOrderItemId,
                    Type = warranty.Type
                }).ToList();
        }

        private static IList<RepairOrderItemTaxToWrite> CreateItemTaxes(IList<RepairOrderItemTaxToRead> taxes)
        {
            if (taxes is null)
                return new List<RepairOrderItemTaxToWrite>();

            return taxes.Select(tax =>
                new RepairOrderItemTaxToWrite()
                {
                    LaborTax = tax.LaborTax,
                    LaborTaxRate = tax.LaborTaxRate,
                    PartTax = tax.PartTax,
                    PartTaxRate = tax.PartTaxRate,
                    RepairOrderItemId = tax.RepairOrderItemId,
                    TaxId = tax.TaxId
                }).ToList();
        }

        private static IList<RepairOrderSerialNumberToWrite> CreateSerialNumbers(IList<RepairOrderSerialNumberToRead> serialNumbers)
        {
            if (serialNumbers is null)
                return new List<RepairOrderSerialNumberToWrite>();

            return serialNumbers.Select(serialNumber =>
                new RepairOrderSerialNumberToWrite()
                {
                    RepairOrderItemId = serialNumber.RepairOrderItemId,
                    SerialNumber = serialNumber.SerialNumber
                }).ToList();
        }

        private static IList<RepairOrderServiceToWrite> CreateServices(IList<RepairOrderServiceToRead> services)
        {
            if (services is null)
                return new List<RepairOrderServiceToWrite>();

            return services.Select(service =>
                new RepairOrderServiceToWrite()
                {
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
                    Items = CreateItems(service.Items)
                }).ToList();
        }

        private static IList<RepairOrderItemToWrite> CreateItems(IList<RepairOrderItemToRead> items)
        {
            if (items is null)
                return new List<RepairOrderItemToWrite>();

            return items.Select(item =>
                new RepairOrderItemToWrite()
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
                    Total = item.Total,
                    SerialNumbers = CreateSerialNumbers(item.SerialNumbers),
                    Taxes = CreateItemTaxes(item.Taxes),
                    Warranties = CreateWarranties(item.Warranties)
                }).ToList();
        }

        private static IList<RepairOrderTechToRead> CreateServiceTechnicians(IList<RepairOrderTech> technicians)
        {
            if (technicians == null)
                return new List<RepairOrderTechToRead>();

            return technicians.Select(technician =>
                new RepairOrderTechToRead()
                {
                    Id = technician.Id,
                    RepairOrderServiceId = technician.RepairOrderServiceId,
                    TechnicianId = technician.TechnicianId
                }).ToList();
        }

        private static IList<RepairOrderItemToRead> CreateServiceItems(IList<RepairOrderItem> items)
        {
            if (items == null)
                return new List<RepairOrderItemToRead>();

            return items.Select(item =>
                new RepairOrderItemToRead()
                {
                    Id = item.Id,
                    RepairOrderServiceId = item.RepairOrderServiceId,
                    Manufacturer = ManufacturerToRead.ConvertToDto(item.Manufacturer),
                    ManufacturerId = item.ManufacturerId,
                    PartNumber = item.PartNumber,
                    Description = item.Description,
                    SaleCode = CreateSaleCode(item.SaleCode),
                    SaleCodeId = item.SaleCodeId,
                    ProductCode = ProductCodeToRead.ConvertToDto(item.ProductCode),
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
                    SerialNumbers = CreateSerialNumbers(item.SerialNumbers),
                    Warranties = CreateWarranties(item.Warranties),
                    Taxes = CreateItemTaxes(item.Taxes)
                }).ToList();
        }

        private static IList<RepairOrderServiceToRead> CreateServices(IList<RepairOrderService> services)
        {
            if (services == null)
                return new List<RepairOrderServiceToRead>();

            return services.Select(service =>
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
                    Items = CreateServiceItems(service.Items),
                    Techs = CreateServiceTechnicians(service.Techs),
                    Taxes = CreateServiceTaxes(service.Taxes)
                }).ToList();
        }

        public static RepairOrderToRead CreateRepairOrder(RepairOrder repairOrder)
        {
            if (repairOrder != null)
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
                    Services = CreateServices(repairOrder.Services),
                    Taxes = CreateTaxes(repairOrder.Taxes),
                    Payments = CreatePayments(repairOrder.Payments)
                };
            }

            return null;
        }

        private static IList<RepairOrderItemTaxToRead> CreateItemTaxes(IList<RepairOrderItemTax> taxes)
        {
            if (taxes == null)
                return new List<RepairOrderItemTaxToRead>();

            return taxes.Select(tax =>
                new RepairOrderItemTaxToRead()
                {
                    Id = tax.Id,
                    RepairOrderItemId = tax.RepairOrderItemId,
                    TaxId = tax.TaxId,
                    PartTaxRate = tax.PartTaxRate,
                    LaborTaxRate = tax.LaborTaxRate,
                    PartTax = tax.PartTax,
                    LaborTax = tax.LaborTax
                }).ToList();
        }

        private static IList<RepairOrderWarrantyToRead> CreateWarranties(IList<RepairOrderWarranty> warranties)
        {
            if (warranties == null)
                return new List<RepairOrderWarrantyToRead>();

            return warranties.Select(warranty =>
                new RepairOrderWarrantyToRead()
                {
                    Id = warranty.Id,
                    RepairOrderItemId = warranty.RepairOrderItemId,
                    Quantity = warranty.Quantity,
                    Type = warranty.Type,
                    NewWarranty = warranty.NewWarranty,
                    OriginalWarranty = warranty.OriginalWarranty,
                    OriginalInvoiceId = warranty.OriginalInvoiceId
                }).ToList();
        }

        private static IList<RepairOrderSerialNumberToRead> CreateSerialNumbers(IList<RepairOrderSerialNumber> serialNumbers)
        {
            if (serialNumbers == null)
                return new List<RepairOrderSerialNumberToRead>();

            return serialNumbers.Select(serialNumber =>
                new RepairOrderSerialNumberToRead()
                {
                    Id = serialNumber.Id,
                    RepairOrderItemId = serialNumber.RepairOrderItemId,
                    SerialNumber = serialNumber.SerialNumber
                }).ToList();
        }

        private static IList<RepairOrderTaxToRead> CreateTaxes(IList<RepairOrderTax> taxes)
        {
            if (taxes == null)
                return new List<RepairOrderTaxToRead>();

            return taxes.Select(tax =>
                        new RepairOrderTaxToRead()
                        {
                            Id = tax.Id,
                            RepairOrderId = tax.RepairOrderId,
                            TaxId = tax.TaxId,
                            PartTaxRate = tax.PartTaxRate,
                            LaborTaxRate = tax.LaborTaxRate,
                            PartTax = tax.PartTax,
                            LaborTax = tax.LaborTax
                        }).ToList();
        }

        private static IList<RepairOrderServiceTaxToRead> CreateServiceTaxes(IList<RepairOrderServiceTax> taxes)
        {
            if (taxes == null)
                return new List<RepairOrderServiceTaxToRead>();

            return taxes.Select(tax =>
                new RepairOrderServiceTaxToRead()
                {
                    Id = tax.Id,
                    RepairOrderServiceId = tax.RepairOrderServiceId,
                    TaxId = tax.TaxId,
                    PartTaxRate = tax.PartTaxRate,
                    LaborTaxRate = tax.LaborTaxRate,
                    PartTax = tax.PartTax,
                    LaborTax = tax.LaborTax
                }).ToList();
        }

        private static IList<RepairOrderPaymentToRead> CreatePayments(IList<RepairOrderPayment> payments)
        {
            if (payments == null)
                return new List<RepairOrderPaymentToRead>();

            return payments.Select(payment =>
                new RepairOrderPaymentToRead()
                {
                    Id = payment.Id,
                    RepairOrderId = payment.RepairOrderId,
                    PaymentMethod = payment.PaymentMethod,
                    Amount = payment.Amount
                }).ToList();
        }

        public static RepairOrderToReadInList CreateRepairOrderToReadInList(RepairOrder repairOrder)
        {
            if (repairOrder != null)
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