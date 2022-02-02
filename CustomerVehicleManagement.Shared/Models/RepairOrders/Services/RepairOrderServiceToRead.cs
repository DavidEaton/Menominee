using CustomerVehicleManagement.Domain.Entities.RepairOrders;
using CustomerVehicleManagement.Shared.Models.RepairOrders.Items;
using CustomerVehicleManagement.Shared.Models.RepairOrders.Taxes;
using CustomerVehicleManagement.Shared.Models.RepairOrders.Techs;
using System.Collections.Generic;
using System.Linq;

namespace CustomerVehicleManagement.Shared.Models.RepairOrders.Services
{
    public class RepairOrderServiceToRead
    {
        public long Id { get; set; }
        public long RepairOrderId { get; set; }
        public int SequenceNumber { get; set; }
        public string ServiceName { get; set; }
        public string SaleCode { get; set; }
        public bool IsCounterSale { get; set; }
        public bool IsDeclined { get; set; }
        public double PartsTotal { get; set; }
        public double LaborTotal { get; set; }
        public double TaxTotal { get; set; }
        public double ShopSuppliesTotal { get; set; }
        public double Total { get; set; }

        public IReadOnlyList<RepairOrderItemToRead> Items { get; set; } = new List<RepairOrderItemToRead>();
        public IReadOnlyList<RepairOrderTechToRead> Techs { get; set; } = new List<RepairOrderTechToRead>();
        public IReadOnlyList<RepairOrderServiceTaxToRead> Taxes { get; set; } = new List<RepairOrderServiceTaxToRead>();

        public static IReadOnlyList<RepairOrderServiceToRead> ConvertToDto(IList<RepairOrderService> services)
        {
            return services
                .Select(service =>
                        ConvertToDto(service))
                .ToList();
        }

        private static RepairOrderServiceToRead ConvertToDto(RepairOrderService service)
        {
            if (service != null)
            {
                return new RepairOrderServiceToRead()
                {
                    RepairOrderId = service.RepairOrderId,
                    SequenceNumber = service.SequenceNumber,
                    ServiceName = service.ServiceName,
                    SaleCode = service.SaleCode,
                    IsCounterSale = service.IsCounterSale,
                    IsDeclined = service.IsDeclined,
                    PartsTotal = service.PartsTotal,
                    LaborTotal = service.LaborTotal,
                    TaxTotal = service.TaxTotal,
                    ShopSuppliesTotal = service.ShopSuppliesTotal,
                    Total = service.Total,
                    Items = RepairOrderItemToRead.ConvertToDto(service.Items),
                    Techs = RepairOrderTechToRead.ConvertToDto(service.Techs),
                    Taxes = RepairOrderServiceTaxToRead.ConvertToDto(service.Taxes)
                };
            }

            return null;
        }
    }
}
