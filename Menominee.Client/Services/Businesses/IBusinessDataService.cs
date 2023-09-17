using CSharpFunctionalExtensions;
using Menominee.Shared.Models.Businesses;

namespace Menominee.Client.Services.Businesses
{
    public interface IBusinessDataService
    {
        Task<Result<IReadOnlyList<BusinessToReadInList>>> GetAllBusinesses();
        Task<Result<BusinessToRead>> GetBusiness(long id);
        Task<Result<BusinessToRead>> AddBusiness(BusinessToWrite business);
        Task<Result> UpdateBusiness(BusinessToWrite business);
    }
}
