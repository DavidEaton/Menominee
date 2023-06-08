using CustomerVehicleManagement.Domain.Entities.RepairOrders;
using CustomerVehicleManagement.Shared.Models.RepairOrders;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.RepairOrders
{
    public interface IRepairOrderRepository
    {
        Task Add(RepairOrder repairOrder);
        Task<RepairOrder> GetEntity(long id);
        Task<RepairOrderToRead> Get(long id);
        Task<IReadOnlyList<RepairOrderToReadInList>> Get();
        Task Delete(long id);
        Task<bool> Exists(long id);
        Task SaveChanges();
        Task<List<long>> GetTodaysRepairOrderNumbers();
        long GetLastInvoiceNumberOrSeed();
    }
}
