using CSharpFunctionalExtensions;
using Menominee.Shared.Models.Http;
using Menominee.Shared.Models.RepairOrders;

namespace Menominee.Client.Components.RepairOrders
{
    public interface IRepairOrderDataService
    {
        Task<Result<IReadOnlyList<RepairOrderToReadInList>>> GetAllAsync();
        Task<Result<RepairOrderToRead>> GetAsync(long id);
        Task<Result<PostResponse>> AddAsync(RepairOrderToWrite repairOrder);
        Task<Result> UpdateAsync(RepairOrderToWrite repairOrder);
    }
}
