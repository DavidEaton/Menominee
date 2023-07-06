using CustomerVehicleManagement.Domain.Entities.Inventory;
using System.Collections.Generic;
using System.Linq;

namespace CustomerVehicleManagement.Shared.Models.Inventory.InventoryItems.Package
{
    public class InventoryItemPackagePlaceholderHelper
    {
        public static InventoryItemPackagePlaceholderToRead ConvertToReadDto(InventoryItemPackagePlaceholder placeholder)
        {
            return placeholder is null
                ? new()
                : new()
                {
                    Id = placeholder.Id,
                    DisplayOrder = placeholder.DisplayOrder,
                    ItemType = placeholder.ItemType,
                    Details = placeholder.Details is null
                        ? new()
                        : new()
                        {
                            Quantity = placeholder.Details.Quantity,
                            ExciseFeeIsAdditional = placeholder.Details.ExciseFeeIsAdditional,
                            LaborAmountIsAdditional = placeholder.Details.LaborAmountIsAdditional,
                            PartAmountIsAdditional = placeholder.Details.PartAmountIsAdditional
                        }
                };
        }

        internal static List<InventoryItemPackagePlaceholderToRead> ConvertToReadDtos(List<InventoryItemPackagePlaceholder> placeholders)
        {
            return placeholders is null
                ? new()
                : placeholders.Select(ConvertToReadDto).ToList();
        }
    }
}
