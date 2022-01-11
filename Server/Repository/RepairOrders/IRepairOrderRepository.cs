using MenomineePlayWASM.Shared.Dtos.RepairOrders;
using MenomineePlayWASM.Shared.Entities.RepairOrders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MenomineePlayWASM.Server.Repository.RepairOrders
{
    public interface IRepairOrderRepository
    {
        Task AddRepairOrderAsync(RepairOrder repairOrder);
        Task<RepairOrder> GetRepairOrderEntityAsync(long id);
        Task<RepairOrderToRead> GetRepairOrderAsync(long id);
        Task<IReadOnlyList<RepairOrderToRead>> GetRepairOrdersAsync();
        Task<IReadOnlyList<RepairOrderToReadInList>> GetRepairOrderListAsync();
        void UpdateRepairOrderAsync(RepairOrder repairOrder);
        Task DeleteRepairOrderAsync(long id);
        Task<bool> RepairOrderExistsAsync(long id);
        Task<bool> SaveChangesAsync();
        void FixTrackingState();
    }
}
