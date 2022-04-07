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
                // TODO: check if this part's product code requires warranty
                // if (ProductCodeRequireWwarranty(item))
                return true;
            }
            return false;
        }

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
                            Type = (WarrantyType)warranty.Type,
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
        
        private static bool IsFractional(double quantitySold)
        {
            return !(quantitySold % 1 == 0);
        }

        public static RepairOrderToWrite Project(RepairOrderToRead repairOrder)
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

        private static List<RepairOrderWarrantyToRead> MissingRequiredWarranties(RepairOrderItemToRead item)
        {
            var list = new List<RepairOrderWarrantyToRead>();
            // If QuantitySold is fractional, and part requires warranty, that's an invalid
            // state we must prevent.
            // TODO: This is a business rule. Business rules shouuld live in the domain layer.
            if (IsFractional(item.QuantitySold))
                return new List<RepairOrderWarrantyToRead>();

            int missingRequiredWarrantiesCount = (int)item.QuantitySold - item.Warranties.Count;

            for (var i = 0; i < missingRequiredWarrantiesCount; i++)
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

        // TODO: Move this logic down into the domain aggregate class: Domain.Entities.RepairOrders.RepairOrderItem.cs
        private static bool SerialNumberRequired(RepairOrderItemToWrite item)
        {
            if ((item.PartType == PartType.Part || item.PartType == PartType.Tire) && item.QuantitySold > 0)
            {
                // check if this part's product code requires serial numbers
                // if (ProductCodeRequiresSerialNumber(item))
                return true;
            }
            return false;
        }

        public static RepairOrder Project(RepairOrderToWrite repairOrder)
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
                result.Payments = ProjectPayments(repairOrder.Payments);

            if (repairOrder?.Services?.Count > 0)
                result.Services = ProjectServices(repairOrder.Services);

            if (repairOrder?.Taxes?.Count > 0)
                result.Taxes = ProjectTaxes(repairOrder.Taxes);

            return result;
        }

        private static List<RepairOrderService> ProjectServices(List<RepairOrderServiceToWrite> services)
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
                    Items = ProjectServiceItems(service.Items),
                    Taxes = ProjectServiceTaxes(service.Taxes),
                    Techs = ProjectServiceTechnicians(service.Techs)
                }).ToList();
        }

        public static List<RepairOrderTax> ProjectTaxes(List<RepairOrderTaxToWrite> taxes)
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

        public static List<RepairOrderPayment> ProjectPayments(List<RepairOrderPaymentToWrite> payments)
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

        public static IList<RepairOrderItem> ProjectServiceItems(IList<RepairOrderItemToWrite> items)
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
                    ProductCode = ProductCodeHelper.CreateProductCode(item.ProductCode),
                    ProductCodeId = item.ProductCodeId,
                    QuantitySold = item.QuantitySold,
                    RepairOrderServiceId = item.RepairOrderServiceId,
                    SaleCode = ProjectSaleCode(item.SaleCode),
                    SaleCodeId = item.SaleCodeId,
                    SaleType = item.SaleType,
                    SellingPrice = item.SellingPrice,
                    Total = item.Total,
                    SerialNumbers = ProjectServiceItemSerialNumbers(item.SerialNumbers),
                    Taxes = ProjectServiceItemTaxes(item.Taxes),
                    Warranties = ProjectServiceItemWarranties(item.Warranties)
                }).ToList();
        }

        public static List<RepairOrderTech> ProjectServiceTechnicians(IList<RepairOrderTechToWrite> technicians)
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

        public static List<RepairOrderServiceTax> ProjectServiceTaxes(IList<RepairOrderServiceTaxToWrite> taxes)
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

        public static List<RepairOrderWarranty> ProjectServiceItemWarranties(IList<RepairOrderWarrantyToWrite> warranties)
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

        public static List<RepairOrderItemTax> ProjectServiceItemTaxes(IList<RepairOrderItemTaxToWrite> taxes)
        {
            if (taxes == null)
                return new List<RepairOrderItemTax>();

            return taxes.Select(tax =>
                new RepairOrderItemTax()
                {
                    LaborTax = tax.LaborTax,
                }).ToList();
        }
        
        private static List<RepairOrderSerialNumber> ProjectServiceItemSerialNumbers(IList<RepairOrderSerialNumberToWrite> serialNumbers)
        {
            if (serialNumbers is null)
                return new List<RepairOrderSerialNumber>();

            return serialNumbers.Select(serialNumber =>
                new RepairOrderSerialNumber()
                {
                    RepairOrderItemId = serialNumber.RepairOrderItemId,
                    SerialNumber = serialNumber.SerialNumber
                }).ToList();
        }

        public static SaleCode ProjectSaleCode(SaleCodeToWrite saleCode)
        {
            if (saleCode == null)
                return null;

            return new SaleCode()
            {
                Code = saleCode.Code,
                Name = saleCode.Name
            };
        }

        private static SaleCodeToWrite ProjectSaleCode(SaleCodeToRead saleCode)
        {
            if (saleCode is null)
                return null;

            return new SaleCodeToWrite
            {
                Code = saleCode.Code,
                Name = saleCode.Name
            };
        }

        public static SaleCodeToRead ProjectSaleCode(SaleCode saleCode)
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

        private static List<RepairOrderTaxToWrite> ProjectTaxes(List<RepairOrderTaxToRead> taxes)
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

        private static List<RepairOrderPaymentToWrite> ProjectPayments(List<RepairOrderPaymentToRead> payments)
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

        private static List<RepairOrderWarrantyToWrite> ProjectServiceItemWarranties(IList<RepairOrderWarrantyToRead> warranties)
        {
            if (warranties is null)
                return new List<RepairOrderWarrantyToWrite>();

            return warranties.Select(warranty =>
                new RepairOrderWarrantyToWrite()
                {
                    Id =    warranty.Id,
                    NewWarranty = warranty.NewWarranty,
                    OriginalInvoiceId = warranty.OriginalInvoiceId,
                    OriginalWarranty = warranty.OriginalWarranty,
                    Quantity = warranty.Quantity,
                    RepairOrderItemId = warranty.RepairOrderItemId,
                    Type = warranty.Type
                }).ToList();
        }
        
        private static List<RepairOrderItemTaxToWrite> ProjectServiceItemTaxes(IList<RepairOrderItemTaxToRead> taxes)
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

        private static List<RepairOrderSerialNumberToWrite> ProjectServiceItemSerialNumbers(IList<RepairOrderSerialNumberToRead> serialNumbers)
        {
            if (serialNumbers is null)
                return new List<RepairOrderSerialNumberToWrite>();

            return serialNumbers.Select(serialNumber =>
                new RepairOrderSerialNumberToWrite()
                {
                    Id = serialNumber.Id,
                    RepairOrderItemId = serialNumber.RepairOrderItemId,
                    SerialNumber = serialNumber.SerialNumber
                }).ToList();
        }

        private static List<RepairOrderServiceToWrite> ProjectServices(List<RepairOrderServiceToRead> services)
        {
            if (services is null)
                return new List<RepairOrderServiceToWrite>();

            return services.Select(service =>
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
                }).ToList();
        }

        private static List<RepairOrderItemToWrite> ProjectServiceItems(IList<RepairOrderItemToRead> items)
        {
            if (items is null)
                return new List<RepairOrderItemToWrite>();

            foreach (var item in items)
            {
                item.SerialNumbers.AddRange(MissingRequiredSerialNumbers(item));
                item.Warranties.AddRange(MissingRequiredWarranties(item));
            }

            return items.Select(item =>
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
                    Manufacturer = ProjectManufacturer(item.Manufacturer),
                    ManufacturerId = item.ManufacturerId,
                    PartNumber = item.PartNumber,
                    PartType = item.PartType,
                    ProductCode = ProductCodeHelper.CreateProductCode(item.ProductCode),
                    ProductCodeId = item.ProductCodeId,
                    QuantitySold = item.QuantitySold,
                    RepairOrderServiceId = item.RepairOrderServiceId,
                    SaleCode = ProjectSaleCode(item.SaleCode),
                    SaleCodeId = item.SaleCodeId,
                    SaleType = item.SaleType,
                    SellingPrice = item.SellingPrice,
                    Total = item.Total,
                    SerialNumbers = ProjectServiceItemSerialNumbers(item.SerialNumbers),
                    Warranties = ProjectServiceItemWarranties(item.Warranties),
                    Taxes = ProjectServiceItemTaxes(item.Taxes)
                }).ToList();
        }

        private static List<RepairOrderSerialNumberToRead> MissingRequiredSerialNumbers(RepairOrderItemToRead item)
        {
            var list = new List<RepairOrderSerialNumberToRead>();
            // If QuantitySold is fractional, and part requires serial number,
            // that's an invalid state we must prevent.
            // TODO: This is a business rule. Business rules shouuld live in the domain layer.
            if (IsFractional(item.QuantitySold))
                return new List<RepairOrderSerialNumberToRead>();

            int missingRequiredSerialNumbersCount = (int)item.QuantitySold - item.SerialNumbers.Count;

            for (var i = 0; i < missingRequiredSerialNumbersCount; i++)
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

        private static ManufacturerToWrite ProjectManufacturer(ManufacturerToRead manufacturer)
        {
            if (manufacturer is null)
                return new ManufacturerToWrite();

            return new ManufacturerToWrite
            {
                Code = manufacturer.Code,
                Name = manufacturer.Name
            };
        }

        private static IList<RepairOrderTechToRead> ProjectServiceTechnicians(IList<RepairOrderTech> technicians)
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
        
        private static List<RepairOrderItem> ProjectServiceItems(List<RepairOrderItemToWrite> items)
        {
            if (items == null)
                return new List<RepairOrderItem>();

            return items.Select(item =>
                new RepairOrderItem()
                {
                    RepairOrderServiceId = item.RepairOrderServiceId,
                    //Manufacturer = ManufacturerToRead.ConvertToDto(item.Manufacturer),
                    ManufacturerId = item.ManufacturerId,
                    PartNumber = item.PartNumber,
                    Description = item.Description,
                    SaleCode = ProjectSaleCode(item.SaleCode),
                    SaleCodeId = item.SaleCodeId,
                    ProductCode = ProductCodeHelper.CreateProductCode(item.ProductCode),
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
                    Taxes = ProjectServiceItemTaxes(item.Taxes)
                }).ToList();
        }
        
        private static List<RepairOrderItemToRead> ProjectServiceItems(IList<RepairOrderItem> items)
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
                    SaleCode = ProjectSaleCode(item.SaleCode),
                    SaleCodeId = item.SaleCodeId,
                    ProductCode = ProductCodeHelper.CreateProductCode(item.ProductCode),
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
                    Taxes = ProjectServiceItemTaxes(item.Taxes)
                }).ToList();
        }

        private static List<RepairOrderServiceToRead> ProjectServices(List<RepairOrderService> services)
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
                    Items = ProjectServiceItems(service.Items),
                    Techs = ProjectServiceTechnicians(service.Techs),
                    Taxes = ProjectServiceTaxes(service.Taxes)
                }).ToList();
        }

        public static RepairOrderToRead Project(RepairOrder repairOrder)
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
                    Services = ProjectServices(repairOrder.Services),
                    Taxes = ProjectTaxes(repairOrder.Taxes),
                    Payments = ProjectPayments(repairOrder.Payments)
                };
            }

            return null;
        }

        private static List<RepairOrderItemTaxToRead> ProjectServiceItemTaxes(IList<RepairOrderItemTax> taxes)
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

        private static List<RepairOrderWarrantyToRead> ProjectServiceItemWarranties(IList<RepairOrderWarranty> warranties)
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

        private static List<RepairOrderSerialNumberToRead> ProjectServiceItemSerialNumbers(IList<RepairOrderSerialNumber> serialNumbers)
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

        private static List<RepairOrderTaxToRead> ProjectTaxes(IList<RepairOrderTax> taxes)
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

        private static IList<RepairOrderServiceTaxToRead> ProjectServiceTaxes(IList<RepairOrderServiceTax> taxes)
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

        private static List<RepairOrderPaymentToRead> ProjectPayments(IList<RepairOrderPayment> payments)
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

        public static RepairOrderToReadInList ProjectInList(RepairOrder repairOrder)
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