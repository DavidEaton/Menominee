using CSharpFunctionalExtensions;
using Menominee.Shared.Models.Http;
using Menominee.Shared.Models.Inventory.InventoryItems;

namespace Menominee.Client.Services.Inventory
{
    public interface IInventoryItemDataService
    {
        Task<Result<IReadOnlyList<InventoryItemToReadInList>>> GetAllAsync();
        Task<Result<IReadOnlyList<InventoryItemToReadInList>>> GetByManufacturerAsync(long manufacturerId);
        Task<Result<InventoryItemToRead>> GetAsync(long id);
        Task<Result<PostResponse>> AddAsync(InventoryItemToWrite item);
        Task<Result> UpdateAsync(InventoryItemToWrite item);
    }
}
