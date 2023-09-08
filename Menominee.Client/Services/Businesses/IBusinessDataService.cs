using Menominee.Shared.Models.Businesses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menominee.Client.Services.Businesses
{
    public interface IBusinessDataService
    {
        Task<IReadOnlyList<BusinessToReadInList>> GetAllBusinesses();
        Task<BusinessToRead> GetBusiness(long id);
        Task<BusinessToRead> AddBusiness(BusinessToWrite business);
        Task UpdateBusiness(BusinessToWrite business, long id);
    }
}
