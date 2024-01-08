using Menominee.Domain.Enums;

namespace Menominee.Shared.Models.RepairOrders.LineItems
{
    public class DiscountAmountToRead
    {
        public ItemDiscountType DiscountType { get; set; }
        public double Amount { get; set; }
    }
}
