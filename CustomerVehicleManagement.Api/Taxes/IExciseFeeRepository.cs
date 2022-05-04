using CustomerVehicleManagement.Domain.Entities.Taxes;
using CustomerVehicleManagement.Shared.Models.Taxes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.Taxes
{
    public interface IExciseFeeRepository
    {
        Task AddExciseFeeAsync(ExciseFee exciseFee);
        Task<ExciseFee> GetExciseFeeEntityAsync(long id);
        Task<ExciseFeeToRead> GetExciseFeeAsync(long id);
        Task<IReadOnlyList<ExciseFeeToReadInList>> GetExciseFeeListAsync();
        Task<ExciseFee> UpdateExciseFeeAsync(ExciseFee exciseFee);
        Task DeleteExciseFeeAsync(long id);
        Task<bool> ExciseFeeExistsAsync(long id);
        Task<bool> SaveChangesAsync();
        void FixTrackingState();
    }
}
