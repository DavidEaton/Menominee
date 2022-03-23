using CustomerVehicleManagement.Domain.Entities;
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
using System.Collections.Generic;
using System.Linq;

namespace CustomerVehicleManagement.Shared.Models.RepairOrders
{
    public class RepairOrderHelper
    {
        public static RepairOrderToWrite ConvertReadToWriteDto(RepairOrderToRead repairOrder)
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
                Services = ServicesReadToWriteDto(repairOrder),
                Payments = PaymentsReadToWriteDto(repairOrder),
                Taxes = TaxesReadToWriteDto(repairOrder)
            };

            foreach (var service in repairOrderToWrite.Services)
            {
                foreach (var item in service.Items)
                    AddMissingSerialNumbers(item);
            }

            foreach (var service in repairOrderToWrite.Services)
            {
                foreach (var item in service.Items)
                    AddMissingWarranties(item);
            }

            return repairOrderToWrite;
        }

        private static void AddMissingWarranties(RepairOrderItemToWrite item)
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

        //public static IList<RepairOrderItem> CreateItemsFromWriteDto(IList<RepairOrderItemToWrite> items)
        //{
        //    var result = new List<RepairOrderItem>();

        //    foreach (var item in items)
        //    {
        //        result.Add(new RepairOrderItem()
        //        {
        //            Core = item.Core,
        //            Cost = item.Cost,
        //            Description = item.Description,
        //            DiscountEach = item.DiscountEach,
        //            DiscountType = item.DiscountType,
        //            IsCounterSale = item.IsCounterSale,
        //            IsDeclined = item.IsDeclined,
        //            LaborEach = item.LaborEach,
        //            LaborType = item.LaborType,
        //            //Manufacturer = item.Manufacturer,
        //            ManufacturerId = item.ManufacturerId,
        //            PartNumber = item.PartNumber,
        //            PartType = item.PartType,
        //            ProductCode = CreateProductCodeFromWriteDto(item.ProductCode),
        //            ProductCodeId = item.ProductCodeId,
        //            QuantitySold = item.QuantitySold,
        //            RepairOrderServiceId = item.RepairOrderServiceId,
        //            SaleCode = CreateSaleCodeFromWriteDto(item.SaleCode),
        //            SaleCodeId = item.SaleCodeId,
        //            SaleType = item.SaleType,
        //            SellingPrice = item.SellingPrice,
        //            Total = item.Total,
        //            SerialNumbers = CreateSerialNumbersFromWriteDto(item.SerialNumbers),
        //            Taxes = CreateTaxesFromWriteDto(item.Taxes),
        //            Warranties = CreateWarrantiesFromWriteDto(item.Warranties)
        //        });
        //    }
        //    return result;
        //}

        public static RepairOrder CreateRepairOrderFromWriteDto(RepairOrderToWrite repairOrder)
        {
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
                result.Payments = RepairOrderHelper.CreatePaymentsFromWriteDto(repairOrder.Payments);

            if (repairOrder?.Services?.Count > 0)
                result.Services = RepairOrderHelper.CreateServicesFromWriteDto(repairOrder.Services);

            if (repairOrder?.Taxes?.Count > 0)
                result.Taxes = RepairOrderHelper.CreateTaxesFromWriteDto(repairOrder.Taxes);

            return result;
        }

        private static IList<RepairOrderTax> CreateTaxesFromWriteDto(IList<RepairOrderTaxToWrite> taxes)
        {
            return (IList<RepairOrderTax>)taxes
                .Select(tax => new RepairOrderTax()
                {
                    LaborTax = tax.LaborTax,
                    LaborTaxRate = tax.LaborTaxRate,
                    PartTax = tax.PartTax,
                    PartTaxRate = tax.PartTaxRate,
                    RepairOrderId = tax.RepairOrderId,
                    TaxId = tax.TaxId
                });
        }

        private static IList<RepairOrderPayment> CreatePaymentsFromWriteDto(IList<RepairOrderPaymentToWrite> payments)
        {
            return (IList<RepairOrderPayment>)payments
                .Select(payment => new RepairOrderPayment()
                {
                    Amount = payment.Amount,
                    PaymentMethod = payment.PaymentMethod,
                    RepairOrderId = payment.RepairOrderId
                });
        }

        private static IList<RepairOrderService> CreateServicesFromWriteDto(IList<RepairOrderServiceToWrite> services)
        {
            return (IList<RepairOrderService>)services
                .Select(service => new RepairOrderService()
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
                    Items = CreateItemsFromWriteDto(service.Items),
                    Taxes = CreateServiceTaxesFromWriteDto(service.Taxes),
                    Techs = CreateTechniciansFromWriteDto(service.Techs)
                });
        }

        public static IList<RepairOrderItem> CreateItemsFromWriteDto(IList<RepairOrderItemToWrite> items)
        {
            return (IList<RepairOrderItem>)items.Select(item => new RepairOrderItem()
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
                ProductCode = CreateProductCodeFromWriteDto(item.ProductCode),
                ProductCodeId = item.ProductCodeId,
                QuantitySold = item.QuantitySold,
                RepairOrderServiceId = item.RepairOrderServiceId,
                SaleCode = CreateSaleCodeFromWriteDto(item.SaleCode),
                SaleCodeId = item.SaleCodeId,
                SaleType = item.SaleType,
                SellingPrice = item.SellingPrice,
                Total = item.Total,
                SerialNumbers = CreateSerialNumbersFromWriteDto(item.SerialNumbers),
                Taxes = CreateTaxesFromWriteDto(item.Taxes),
                Warranties = CreateWarrantiesFromWriteDto(item.Warranties)
            });
        }

        public static IList<RepairOrderTech> CreateTechniciansFromWriteDto(IList<RepairOrderTechToWrite> technicians)
        {
            return (IList<RepairOrderTech>)technicians.Select(technician => new RepairOrderTech()
            {
                RepairOrderServiceId = technician.RepairOrderServiceId,
                TechnicianId = technician.TechnicianId
            });
        }

        public static IList<RepairOrderServiceTax> CreateServiceTaxesFromWriteDto(IList<RepairOrderServiceTaxToWrite> taxes)
        {
            var result = new List<RepairOrderServiceTax>();

            foreach (var serviceTax in taxes)
            {
                result.Add(new RepairOrderServiceTax()
                {
                    LaborTax = serviceTax.LaborTax,
                    LaborTaxRate = serviceTax.LaborTaxRate,
                    PartTax = serviceTax.PartTax,
                    PartTaxRate = serviceTax.PartTaxRate,
                    RepairOrderServiceId = serviceTax.RepairOrderServiceId,
                    TaxId = serviceTax.TaxId
                });
            }

            return result;
        }

        private static IList<RepairOrderWarranty> CreateWarrantiesFromWriteDto(IList<RepairOrderWarrantyToWrite> warranties)
        {
            var result = new List<RepairOrderWarranty>();

            foreach (var item in warranties)
            {
                result.Add(new RepairOrderWarranty()
                {
                    NewWarranty = item.NewWarranty,
                    OriginalInvoiceId = item.OriginalInvoiceId,
                    OriginalWarranty = item.OriginalWarranty,
                    Quantity = item.Quantity,
                    RepairOrderItemId = item.RepairOrderItemId,
                    Type = item.Type
                });
            }

            return result;
        }

        private static IList<RepairOrderItemTax> CreateTaxesFromWriteDto(IList<RepairOrderItemTaxToWrite> taxes)
        {
            var result = new List<RepairOrderItemTax>();

            foreach (var tax in taxes)
            {
                result.Add(new RepairOrderItemTax()
                {
                    LaborTax = tax.LaborTax,

                });
            }

            return result;
        }

        private static IList<RepairOrderSerialNumber> CreateSerialNumbersFromWriteDto(IList<RepairOrderSerialNumberToWrite> serialNumbers)
        {
            var result = new List<RepairOrderSerialNumber>();

            foreach (var serialNumber in serialNumbers)
            {
                result.Add(new RepairOrderSerialNumber()
                {
                    RepairOrderItemId = serialNumber.RepairOrderItemId,
                    SerialNumber = serialNumber.SerialNumber
                });
            }

            return result;
        }

        private static SaleCode CreateSaleCodeFromWriteDto(SaleCodeToWrite saleCode)
        {
            var result = new SaleCode()
            {
                Code = saleCode.Code,
                Name = saleCode.Name
            };

            return result;
        }

        private static ProductCode CreateProductCodeFromWriteDto(ProductCodeToWrite productCode)
        {
            var result = new ProductCode()
            {
                Code = productCode.Code,
                Manufacturer = productCode.Manufacturer,
                Name = productCode.Name,
                SaleCode = productCode.SaleCode
            };

            return result;
        }

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

        public static int WarrantyMissingCount(IList<RepairOrderServiceToWrite> services)
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

        public static int SerialNumbersMissingCount(IList<RepairOrderServiceToWrite> services)
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

        private static IList<RepairOrderTaxToWrite> TaxesReadToWriteDto(RepairOrderToRead repairOrder)
        {
            var result = new List<RepairOrderTaxToWrite>();
            foreach (var tax in repairOrder?.Taxes)
                result.Add(TaxReadToWriteDto(tax));

            return result;
        }

        private static IList<RepairOrderTaxToWrite> TaxesReadToWriteDto(IList<RepairOrderTaxToRead> taxes)
        {
            var result = new List<RepairOrderTaxToWrite>();
            foreach (var tax in taxes)
                result.Add(TaxReadToWriteDto(tax));

            return result;
        }

        private static RepairOrderTaxToWrite TaxReadToWriteDto(RepairOrderTaxToRead tax)
        {
            return new RepairOrderTaxToWrite()
            {
                LaborTax = tax.LaborTax,
                LaborTaxRate = tax.LaborTaxRate,
                PartTax = tax.PartTax,
                PartTaxRate = tax.PartTaxRate,
                RepairOrderId = tax.RepairOrderId,
                TaxId = tax.TaxId
            };
        }

        private static IList<RepairOrderPaymentToWrite> PaymentsReadToWriteDto(RepairOrderToRead repairOrder)
        {
            var result = new List<RepairOrderPaymentToWrite>();
            foreach (var payment in repairOrder?.Payments)
                result.Add(PaymentReadToWriteDto(payment));

            return result;
        }

        private static RepairOrderPaymentToWrite PaymentReadToWriteDto(RepairOrderPaymentToRead payment)
        {
            return new RepairOrderPaymentToWrite()
            {
                Id = payment.Id,
                RepairOrderId = payment.RepairOrderId,
                PaymentMethod = payment.PaymentMethod,
                Amount = payment.Amount
            };
        }

        private static IList<RepairOrderServiceToWrite> ServicesReadToWriteDto(RepairOrderToRead repairOrder)
        {
            var result = new List<RepairOrderServiceToWrite>();
            foreach (var service in repairOrder?.Services)
                result.Add(ServiceReadToWriteDto(service));

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
            return !(quantitySold % 1 == 0);
        }

        private static IList<RepairOrderWarrantyToWrite> WarrantiesReadToWriteDto(IList<RepairOrderWarrantyToRead> warranties)
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
                    Type = warranty.Type
                });
            }

            return result;
        }

        private static IList<RepairOrderItemTaxToWrite> TaxItemsReadToWriteDto(IList<RepairOrderItemTaxToRead> taxes)
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
                    TaxId = tax.TaxId
                });
            }

            return result;
        }

        private static IList<RepairOrderSerialNumberToWrite> SerialNumbersReadToWriteDto(IList<RepairOrderSerialNumberToRead> serialNumbers)
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

        private static RepairOrderServiceToWrite ServiceReadToWriteDto(RepairOrderServiceToRead service)
        {
            return new RepairOrderServiceToWrite
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
                Items = ItemsReadToWriteDto(service.Items)
            };
        }

        private static IList<RepairOrderItemToWrite> ItemsReadToWriteDto(IList<RepairOrderItemToRead> items)
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
                    Total = item.Total,
                    SerialNumbers = SerialNumbersReadToWriteDto(item.SerialNumbers),
                    Taxes = TaxItemsReadToWriteDto(item.Taxes),
                    Warranties = WarrantiesReadToWriteDto(item.Warranties)
                });
            }

            return result;
        }

        public static IList<RepairOrderTechToRead> ConvertTechniciansToReadDto(IList<RepairOrderTech> techs)
        {
            return techs
                .Select(tech =>
                        ConvertTechnicianToReadDto(tech))
                .ToList();
        }

        private static RepairOrderTechToRead ConvertTechnicianToReadDto(RepairOrderTech tech)
        {
            if (tech != null)
            {
                return new RepairOrderTechToRead()
                {
                    Id = tech.Id,
                    RepairOrderServiceId = tech.RepairOrderServiceId,
                    TechnicianId = tech.TechnicianId
                };
            }

            return null;
        }

        public static IList<RepairOrderItemToRead> ConvertItemsToReadDto(IList<RepairOrderItem> items)
        {
            return items
                .Select(item =>
                        ConvertItemToReadDto(item))
                .ToList();
        }

        private static RepairOrderItemToRead ConvertItemToReadDto(RepairOrderItem item)
        {
            if (item != null)
            {
                return new RepairOrderItemToRead()
                {
                    Id = item.Id,
                    RepairOrderServiceId = item.RepairOrderServiceId,
                    Manufacturer = ManufacturerToRead.ConvertToDto(item.Manufacturer),
                    ManufacturerId = item.ManufacturerId,
                    PartNumber = item.PartNumber,
                    Description = item.Description,
                    SaleCode = ConvertSaleCodeToReadDto(item.SaleCode),
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
                    SerialNumbers = ConvertSerialNumbersToDto(item.SerialNumbers),
                    Warranties = ConvertToReadDto(item.Warranties),
                    Taxes = ConvertToReadDto(item.Taxes)
                };
            }

            return null;
        }

        public static IList<RepairOrderServiceToRead> ConvertServicesToReadDto(IList<RepairOrderService> services)
        {
            return services
                .Select(service =>
                        ConvertServiceToReadDto(service))
                .ToList();
        }

        private static RepairOrderServiceToRead ConvertServiceToReadDto(RepairOrderService service)
        {
            if (service != null)
            {
                return new RepairOrderServiceToRead()
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
                    Items = ConvertItemsToReadDto(service.Items),
                    Techs = ConvertTechniciansToReadDto(service.Techs),
                    Taxes = ConvertServiceTaxesToReadDto(service.Taxes)
                };
            }

            return null;
        }

        public static RepairOrderToRead ConvertToReadDto(RepairOrder repairOrder)
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
                    Services = ConvertServicesToReadDto(repairOrder.Services),
                    Taxes = ConvertTaxesToReadDto(repairOrder.Taxes),
                    Payments = ConvertPaymentsToReadDto(repairOrder.Payments)
                };
            }

            return null;
        }

        public static IList<RepairOrderItemTaxToRead> ConvertToReadDto(IList<RepairOrderItemTax> taxes)
        {
            return taxes
                .Select(tax =>
                        ConvertToReadDto(tax))
                .ToList();
        }

        private static RepairOrderItemTaxToRead ConvertToReadDto(RepairOrderItemTax tax)
        {
            if (tax != null)
            {
                return new RepairOrderItemTaxToRead()
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

            return null;
        }

        public static IList<RepairOrderWarrantyToRead> ConvertToReadDto(IList<RepairOrderWarranty> warranties)
        {
            return warranties
                .Select(warranty =>
                        ConvertToReadDto(warranty))
                .ToList();
        }

        private static RepairOrderWarrantyToRead ConvertToReadDto(RepairOrderWarranty warranty)
        {
            if (warranty != null)
            {
                return new RepairOrderWarrantyToRead()
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

            return null;
        }

        public static IList<RepairOrderSerialNumberToRead> ConvertSerialNumbersToDto(IList<RepairOrderSerialNumber> serialNumbers)
        {
            return serialNumbers
                .Select(serialNumber =>
                        ConvertSerialNumberToDto(serialNumber))
                .ToList();
        }

        private static RepairOrderSerialNumberToRead ConvertSerialNumberToDto(RepairOrderSerialNumber serialNumber)
        {
            if (serialNumber != null)
            {
                return new RepairOrderSerialNumberToRead()
                {
                    Id = serialNumber.Id,
                    RepairOrderItemId = serialNumber.RepairOrderItemId,
                    SerialNumber = serialNumber.SerialNumber
                };
            }

            return null;
        }

        public static IList<RepairOrderTaxToRead> ConvertTaxesToReadDto(IList<RepairOrderTax> taxes)
        {
            return taxes
                .Select(tax =>
                        ConvertTaxToReadDto(tax))
                .ToList();
        }

        private static RepairOrderTaxToRead ConvertTaxToReadDto(RepairOrderTax tax)
        {
            if (tax != null)
            {
                return new RepairOrderTaxToRead()
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

            return null;
        }

        public static IList<RepairOrderServiceTaxToRead> ConvertServiceTaxesToReadDto(IList<RepairOrderServiceTax> taxes)
        {
            return taxes
                .Select(tax =>
                        ConvertServiceTaxToReadDto(tax))
                .ToList();
        }

        private static RepairOrderServiceTaxToRead ConvertServiceTaxToReadDto(RepairOrderServiceTax tax)
        {
            if (tax != null)
            {
                return new RepairOrderServiceTaxToRead()
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

            return null;
        }

        public static IList<RepairOrderPaymentToRead> ConvertPaymentsToReadDto(IList<RepairOrderPayment> payments)
        {
            return payments
                .Select(payment =>
                        ConvertPaymentToReadDto(payment))
                .ToList();
        }

        private static RepairOrderPaymentToRead ConvertPaymentToReadDto(RepairOrderPayment payment)
        {
            if (payment != null)
            {
                return new RepairOrderPaymentToRead()
                {
                    Id = payment.Id,
                    RepairOrderId = payment.RepairOrderId,
                    PaymentMethod = payment.PaymentMethod,
                    Amount = payment.Amount
                };
            }

            return null;
        }

        public static RepairOrderToReadInList ConvertToReadInList(RepairOrder repairOrder)
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

        public static IReadOnlyList<RepairOrderPurchaseToRead> ConvertPurchasesToReadDto(IList<RepairOrderPurchase> purchases)
        {
            return purchases
                .Select(purchase =>
                        ConvertPurchaseToReadDto(purchase))
                .ToList();
        }

        private static RepairOrderPurchaseToRead ConvertPurchaseToReadDto(RepairOrderPurchase purchase)
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

        public static SaleCodeToRead ConvertSaleCodeToReadDto(SaleCode saleCode)
        {
            if (saleCode != null)
            {
                return new SaleCodeToRead
                {
                    Id = saleCode.Id,
                    Code = saleCode.Code,
                    Name = saleCode.Name
                };
            }

            return null;
        }
    }
}