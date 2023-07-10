using Menominee.Domain.Entities;

namespace Menominee.Shared.Models.SaleCodes
{
    public class SaleCodeShopSuppliesHelper
    {
        public static SaleCodeShopSupplies ConvertWriteDtoToEntity(SaleCodeShopSuppliesToWrite shopSupplies)
        {
            return shopSupplies is null
                ? null
                : SaleCodeShopSupplies.Create(
                    shopSupplies.Percentage,
                    shopSupplies.MinimumJobAmount,
                    shopSupplies.MinimumCharge,
                    shopSupplies.MaximumCharge,
                    shopSupplies.IncludeParts,
                    shopSupplies.IncludeLabor)
                .Value;
        }
    }
}
