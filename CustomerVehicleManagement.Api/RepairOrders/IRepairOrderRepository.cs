using CustomerVehicleManagement.Domain.Entities.RepairOrders;
using CustomerVehicleManagement.Shared.Models.RepairOrders;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.RepairOrders
{
    public interface IRepairOrderRepository
    {
        Task AddRepairOrderAsync(RepairOrder repairOrder);
        Task<RepairOrder> GetRepairOrderEntityAsync(long id);
        Task<RepairOrderToRead> GetRepairOrderAsync(long id);
        //Task<IReadOnlyList<RepairOrderToRead>> GetRepairOrdersAsync();
        Task<IReadOnlyList<RepairOrderToReadInList>> GetRepairOrderListAsync();
        void UpdateRepairOrderAsync(RepairOrder repairOrder);
        Task DeleteRepairOrderAsync(long id);
        Task<bool> RepairOrderExistsAsync(long id);
        Task SaveChangesAsync();
    }
}
