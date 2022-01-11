using MenomineePlayWASM.Shared.Dtos.RepairOrders;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MenomineePlayWASM.Shared.Services.RepairOrders
{
    public interface IRepairOrderDataService
    {
        Task<IReadOnlyList<RepairOrderToReadInList>> GetAllRepairOrders();
        Task<RepairOrderToRead> GetRepairOrder(long id);
        Task<RepairOrderToRead> AddRepairOrder(RepairOrderToWrite repairOrder);
        Task UpdateRepairOrder(RepairOrderToWrite repairOrder, long id);
    }
}
