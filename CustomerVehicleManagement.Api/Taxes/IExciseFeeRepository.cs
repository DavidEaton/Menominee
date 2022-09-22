using CustomerVehicleManagement.Domain.Entities.Taxes;
using CustomerVehicleManagement.Shared.Models.Taxes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.Taxes
{
    public interface IExciseFeeRepository
    {
        Task AddExciseFeeAsync(ExciseFee entity);
        Task<ExciseFee> GetExciseFeeEntityAsync(long id);
        Task<ExciseFeeToRead> GetExciseFeeAsync(long id);
        Task<IReadOnlyList<ExciseFeeToReadInList>> GetExciseFeeListAsync();
        void DeleteExciseFee(ExciseFee entity);
        Task<bool> ExciseFeeExistsAsync(long id);
        Task SaveChangesAsync();
    }
}
