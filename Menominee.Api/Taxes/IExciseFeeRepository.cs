using Menominee.Domain.Entities.Taxes;
using Menominee.Shared.Models.Taxes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menominee.Api.Taxes
{
    public interface IExciseFeeRepository
    {
        void Add(ExciseFee entity);
        void Delete(ExciseFee entity);
        Task<ExciseFee> GetEntityAsync(long id);
        Task<ExciseFeeToRead> GetAsync(long id);
        Task<IReadOnlyList<ExciseFeeToReadInList>> GetListAsync();
        Task SaveChangesAsync();
    }
}
