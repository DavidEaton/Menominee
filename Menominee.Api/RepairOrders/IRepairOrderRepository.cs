using Menominee.Domain.Entities.RepairOrders;
using Menominee.Shared.Models.RepairOrders;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menominee.Api.RepairOrders
{
    public interface IRepairOrderRepository
    {
        void Add(RepairOrder repairOrder);
        void Delete(RepairOrder repairOrder);
        Task<RepairOrder?> GetEntityAsync(long id);
        Task<RepairOrderToRead?> GetAsync(long id);
        Task<IReadOnlyList<RepairOrderToReadInList?>> GetListAsync();
        Task SaveChangesAsync();
        Task<List<long>> GetTodaysRepairOrderNumbersAsync();
        long GetLastInvoiceNumberOrSeed();
    }
}
