using CustomerVehicleManagement.Shared.Models.RepairOrders;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menominee.Client.Components.RepairOrders
{
    public interface IRepairOrderDataService
    {
        Task<IReadOnlyList<RepairOrderToReadInList>> GetAllRepairOrders();
        Task<RepairOrderToRead> GetRepairOrder(long id);
        Task<RepairOrderToRead> AddRepairOrder(RepairOrderToWrite repairOrder);
        Task UpdateRepairOrder(RepairOrderToWrite repairOrder, long id);
    }
}
