using CSharpFunctionalExtensions;
using Menominee.Shared.Models.Http;
using Menominee.Shared.Models.Inventory.MaintenanceItems;

namespace Menominee.Client.Services.Inventory
{
    public interface IMaintenanceItemDataService
    {
        Task<Result<IReadOnlyList<MaintenanceItemToReadInList>>> GetAllAsync();
        Task<Result<MaintenanceItemToRead>> GetAsync(long id);
        Task<Result<PostResponse>> AddAsync(MaintenanceItemToWrite item);
        Task<Result> UpdateAsync(MaintenanceItemToWrite item);
        Task<Result> DeleteAsync(long id);
    }
}
