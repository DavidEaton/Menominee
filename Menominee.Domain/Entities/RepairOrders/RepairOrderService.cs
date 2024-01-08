using CSharpFunctionalExtensions;
using System.Collections.Generic;
using System.Linq;
using Entity = Menominee.Domain.BaseClasses.Entity;

namespace Menominee.Domain.Entities.RepairOrders
{
    // TODO: DDD: Rename this class to TicketService
    public class RepairOrderService : Entity
    {
        public static readonly string RequiredMessage = $"Please include all required items.";
        public static readonly int MinimumLength = 2;
        public static readonly int MaximumLength = 255;
        public static readonly string InvalidLengthMessage = $"Name must be between {MinimumLength} character(s) {MaximumLength} and in length";

        public string ServiceName { get; private set; }
        public SaleCode SaleCode { get; private set; }
        public bool IsCounterSale => LineItems.All(lineItem => lineItem.IsCounterSale);
        public bool IsDeclined => LineItems.All(lineItem => lineItem.IsDeclined);
        public double PartsTotal => LineItems.Select(
            lineItem => lineItem.AmountTotal)
            .Sum();
        public double LaborTotal => LineItems.Select(
            lineItem => lineItem.LaborAmount.Amount * lineItem.QuantitySold)
            .Sum();
        public double DiscountTotal => LineItems.Select(
            lineItem => lineItem.DiscountAmount.Amount * lineItem.QuantitySold)
            .Sum(); // Usually negative -AL
        public double ServiceTaxTotal => Taxes.Select(
            tax => tax.LaborTax.Amount + tax.PartTax.Amount)
            .Sum();
        public double LineItemTaxesTotal =>
            LineItems.Sum(lineItem =>
                lineItem.Taxes.Sum(tax =>
                    (tax.LaborTax.Amount * tax.LaborTax.Rate) + (tax.PartTax.Amount * tax.PartTax.Rate)));
        public double ExciseFeesTotal => LineItems.Select(
            lineItem => lineItem.ExciseFeesTotal)
            .Sum();
        public double ShopSuppliesTotal { get; private set; }
        //  Looks like we previously left the discount amount positive, then subtracted it from the total.  I'm not sure if it really matters one way or the other as long as we pick one and stick with it. -AL
        public double Total =>
            PartsTotal + LaborTotal + DiscountTotal + ExciseFeesTotal + ShopSuppliesTotal;
        public double TotalWithTax =>
            PartsTotal + LaborTotal + DiscountTotal + ExciseFeesTotal + ShopSuppliesTotal + ServiceTaxTotal;

        private readonly List<RepairOrderLineItem> lineItems = new();
        public IReadOnlyList<RepairOrderLineItem> LineItems => lineItems.ToList();

        private readonly List<RepairOrderServiceTechnician> technicians = new();
        public IReadOnlyList<RepairOrderServiceTechnician> Technicians => technicians.ToList();

        private readonly List<RepairOrderServiceTax> taxes = new();
        public IReadOnlyList<RepairOrderServiceTax> Taxes => taxes.ToList();

        private RepairOrderService(string serviceName, SaleCode saleCode, double shopSuppliesTotal, List<RepairOrderLineItem> lineItems, List<RepairOrderServiceTechnician> technicians, List<RepairOrderServiceTax> taxes)
        {
            ServiceName = serviceName;
            SaleCode = saleCode;
            ShopSuppliesTotal = shopSuppliesTotal;
            this.lineItems = lineItems ?? new List<RepairOrderLineItem>();
            this.technicians = technicians ?? new List<RepairOrderServiceTechnician>();
            this.taxes = taxes ?? new List<RepairOrderServiceTax>();
        }

        public static Result<RepairOrderService> Create(
            string serviceName,
            SaleCode saleCode,
            double shopSuppliesTotal,
            List<RepairOrderLineItem> lineItems = null,
            List<RepairOrderServiceTechnician> technicians = null,
            List<RepairOrderServiceTax> taxes = null)
        {
            serviceName = (serviceName ?? string.Empty).Trim();

            if (serviceName.Length > MaximumLength || serviceName.Length < MinimumLength)
                return Result.Failure<RepairOrderService>(InvalidLengthMessage);

            if (saleCode is null)
                return Result.Failure<RepairOrderService>(RequiredMessage);

            return Result.Success(new RepairOrderService(serviceName, saleCode, shopSuppliesTotal, lineItems, technicians, taxes));
        }

        public Result<string> SetServiceName(string serviceName)
        {
            serviceName = (serviceName ?? string.Empty).Trim();

            if (serviceName.Length < MinimumLength ||
                serviceName.Length > MaximumLength)
                return Result.Failure<string>(InvalidLengthMessage);

            return Result.Success(ServiceName = serviceName);
        }

        public Result<SaleCode> SetSaleCode(SaleCode saleCode)
        {
            return
                saleCode is null
                ? Result.Failure<SaleCode>(RequiredMessage)
                : Result.Success(SaleCode = saleCode);
        }

        public Result<RepairOrderLineItem> AddLineItem(RepairOrderLineItem lineItem)
        {
            if (lineItem is null)
                return Result.Failure<RepairOrderLineItem>(RequiredMessage);

            lineItems.Add(lineItem);

            return Result.Success(lineItem);
        }

        public Result<RepairOrderLineItem> RemoveLineItem(RepairOrderLineItem lineItem)
        {
            if (lineItem is null)
                return Result.Failure<RepairOrderLineItem>(RequiredMessage);

            lineItems.Remove(lineItem);

            return Result.Success(lineItem);
        }

        public Result<RepairOrderServiceTechnician> AddTechnician(RepairOrderServiceTechnician technician)
        {
            if (technician is null)
                return Result.Failure<RepairOrderServiceTechnician>(RequiredMessage);

            technicians.Add(technician);

            return Result.Success(technician);
        }

        public Result<RepairOrderServiceTechnician> RemoveTechnician(RepairOrderServiceTechnician technician)
        {
            if (technician is null)
                return Result.Failure<RepairOrderServiceTechnician>(RequiredMessage);

            technicians.Remove(technician);

            return Result.Success(technician);
        }


        public Result<RepairOrderServiceTax> AddTax(RepairOrderServiceTax tax)
        {
            if (tax is null)
                return Result.Failure<RepairOrderServiceTax>(RequiredMessage);

            taxes.Add(tax);

            return Result.Success(tax);
        }

        public Result<RepairOrderServiceTax> RemoveTax(RepairOrderServiceTax tax)
        {
            if (tax is null)
                return Result.Failure<RepairOrderServiceTax>(RequiredMessage);

            taxes.Remove(tax);

            return Result.Success(tax);
        }

        #region ORM

        // EF requires a parameterless constructor
        protected RepairOrderService()
        {
            lineItems = new List<RepairOrderLineItem>();
            technicians = new List<RepairOrderServiceTechnician>();
            taxes = new List<RepairOrderServiceTax>();
        }

        #endregion
    }
}
