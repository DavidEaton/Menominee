using CustomerVehicleManagement.Domain.Entities.RepairOrders;
using CustomerVehicleManagement.Shared.Models.RepairOrders.Payments;
using CustomerVehicleManagement.Shared.Models.RepairOrders.Services;
using CustomerVehicleManagement.Shared.Models.RepairOrders.Statuses;
using CustomerVehicleManagement.Shared.Models.RepairOrders.Taxes;
using TestingHelperLibrary.Fakers;

namespace TestingHelperLibrary.RepairOrders
{
    public class RepairOrderTestHelper
    {
        public static List<RepairOrderPayment> CreateRepairOrderPayments(List<RepairOrderPaymentToWrite> payments)
        {
            return (payments ?? new List<RepairOrderPaymentToWrite>())
                .Select(payment =>
                {
                    var editedPayment = new RepairOrderPaymentFaker(generateId: false, id: payment.Id).Generate();
                    editedPayment.SetAmount(payment.Amount);
                    editedPayment.SetPaymentMethod(payment.PaymentMethod);
                    return editedPayment;
                })
                .ToList();
        }

        public static List<RepairOrderService> CreateRepairOrderServices(List<RepairOrderServiceToWrite> services)
        {
            return (services ?? new List<RepairOrderServiceToWrite>())
                .Select(service =>
                {
                    var editedService = new RepairOrderServiceFaker(generateId: false, id: service.Id).Generate();
                    editedService.SetServiceName(service.ServiceName);
                    editedService.SetSaleCode(new SaleCodeFaker(id: service.SaleCode.Id).Generate());
                    editedService.SaleCode.SetDesiredMargin(service.SaleCode.DesiredMargin);
                    editedService.SaleCode.SetLaborRate(service.SaleCode.LaborRate);
                    editedService.SaleCode.SetCode(service.SaleCode.Code);
                    editedService.SaleCode.SetName(service.SaleCode.Name);
                    editedService.SaleCode.SetShopSupplies(editedService.SaleCode.ShopSupplies);
                    return editedService;
                })
                .ToList();
        }

        public static List<RepairOrderStatus> CreateRepairOrderStatuses(List<RepairOrderStatusToWrite> statuses)
        {
            return (statuses ?? new List<RepairOrderStatusToWrite>())
                .Select(status =>
                {
                    var editedStatus = new RepairOrderStatusFaker(generateId: false, id: status.Id).Generate();
                    editedStatus.SetStatus(status.Status);
                    editedStatus.SetDescription(status.Description);
                    editedStatus.SetDate(status.Date);
                    return editedStatus;
                })
                .ToList();
        }

        public static List<RepairOrderTax> CreateRepairOrderTaxes(List<RepairOrderTaxToWrite> taxes)
        {
            return (taxes ?? new List<RepairOrderTaxToWrite>())
                .Select(tax =>
                {
                    var editedTax = new RepairOrderTaxFaker(generateId: false, id: tax.Id).Generate();
                    editedTax.SetPartTax(PartTax.Create(tax.PartTax.Rate, tax.PartTax.Amount).Value);
                    editedTax.SetLaborTax(LaborTax.Create(tax.LaborTax.Rate, tax.LaborTax.Amount).Value);
                    return editedTax;
                })
                .ToList();
        }
    }
}
