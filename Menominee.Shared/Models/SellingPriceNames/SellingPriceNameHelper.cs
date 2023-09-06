using Menominee.Domain.Entities.Inventory;

namespace Menominee.Shared.Models.SellingPriceNames;

public class SellingPriceNameHelper
{
    public static SellingPriceNameToRead ConvertToReadDto(SellingPriceName sellingPriceName)
    {
        return sellingPriceName is null
            ? new()
            : new()
            {
                Id = sellingPriceName.Id,
                Name = sellingPriceName.Name
            };
    }

    public static SellingPriceNameToWrite ConvertToWriteDto(SellingPriceName sellingPriceName)
    {
        return sellingPriceName is null
            ? new()
            : new()
            {
                Id = sellingPriceName.Id,
                Name = sellingPriceName.Name
            };
    }

    public static SellingPriceNameToWrite ConvertReadToWriteDto(SellingPriceNameToRead sellingPriceName)
    {
        return sellingPriceName is null
            ? new()
            : new()
            {
                Id = sellingPriceName.Id,
                Name = sellingPriceName.Name
            };
    }
}
