using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Domain.Entities.RepairOrders;
using CustomerVehicleManagement.Shared.Models.RepairOrders.LineItems;
using CustomerVehicleManagement.Shared.Models.RepairOrders.Taxes;
using CustomerVehicleManagement.Shared.Models.RepairOrders.Techs;
using CustomerVehicleManagement.Shared.Models.SaleCodes;
using System.Collections.Generic;
using System.Linq;

namespace CustomerVehicleManagement.Shared.Models.RepairOrders.Services
{
    public class ServiceHelper
    {
        public static List<RepairOrderServiceToRead> ConvertToReadDtos(IReadOnlyList<RepairOrderService> services)
        {
            return services?.Select(
                service =>
                new RepairOrderServiceToRead()
                {
                    Id = service.Id,
                    ServiceName = service.ServiceName,
                    SaleCode = new SaleCodeToRead()
                    {
                        Id = service.SaleCode.Id,
                        Code = service.SaleCode.Code,
                        DesiredMargin = service.SaleCode.DesiredMargin,
                        LaborRate = service.SaleCode.LaborRate,
                        Name = service.SaleCode.Name,
                        ShopSupplies = new SaleCodeShopSuppliesToRead()
                        {
                            IncludeLabor = service.SaleCode.ShopSupplies.IncludeLabor,
                            IncludeParts = service.SaleCode.ShopSupplies.IncludeParts,
                            MaximumCharge = service.SaleCode.ShopSupplies.MaximumCharge,
                            MinimumCharge = service.SaleCode.ShopSupplies.MinimumCharge,
                            MinimumJobAmount = service.SaleCode.ShopSupplies.MinimumJobAmount,
                            Percentage = service.SaleCode.ShopSupplies.Percentage
                        }
                    },
                    IsCounterSale = service.IsCounterSale,
                    IsDeclined = service.IsDeclined,
                    PartsTotal = service.PartsTotal,
                    LaborTotal = service.LaborTotal,
                    DiscountTotal = service.DiscountTotal,
                    TaxTotal = service.TaxTotal,
                    ShopSuppliesTotal = service.ShopSuppliesTotal,
                    Total = service.Total,
                    Items = LineItemHelper.ConvertToReadDtos(service.LineItems),
                    Techs = TechnicianHelper.ConvertToReadDtos(service.Technicians),
                    Taxes = ServiceTaxHelper.ConvertToReadDtos(service.Taxes)
                }).ToList()
                ?? new List<RepairOrderServiceToRead>();
        }

        public static List<RepairOrderServiceToWrite> ConvertReadToWriteDtos(List<RepairOrderServiceToRead> services)
        {
            return services?.Select(
                service =>
                new RepairOrderServiceToWrite()
                {
                    DiscountTotal = service.DiscountTotal,
                    Id = service.Id,
                    IsCounterSale = service.IsCounterSale,
                    IsDeclined = service.IsDeclined,
                    LaborTotal = service.LaborTotal,
                    PartsTotal = service.PartsTotal,
                    SaleCode = service.SaleCode,
                    ServiceName = service.ServiceName,
                    ShopSuppliesTotal = service.ShopSuppliesTotal,
                    TaxTotal = service.TaxTotal,
                    Total = service.Total,
                    LineItems = LineItemHelper.CovertReadToWriteDtos(service.Items),
                    Techs = TechnicianHelper.CovertReadToWriteDtos(service.Techs),
                    Taxes = ServiceTaxHelper.CovertReadToWriteDtos(service.Taxes)
                }).ToList()
                ?? new List<RepairOrderServiceToWrite>();
        }

        public static List<RepairOrderService> ConvertWriteDtosToEntities(
            IReadOnlyList<RepairOrderServiceToWrite> services,
            IReadOnlyList<SaleCode> saleCodes)
        {
            return
                services is null
                ? new List<RepairOrderService>()
                : services.Select(
                    service => RepairOrderService.Create(
                    service.ServiceName,
                    saleCodes.FirstOrDefault(saleCode => service.SaleCode.Id == saleCode.Id),
                    service.ShopSuppliesTotal,
                    null,
                    null,
                    null
                    ).Value)
                .ToList();
        }
    }
}
