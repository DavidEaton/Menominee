using CSharpFunctionalExtensions;
using Menominee.Shared.Models.Businesses;
using Menominee.Shared.Models.Http;

namespace Menominee.Client.Services.Businesses
{
    public interface IBusinessDataService
    {
        Task<Result<IReadOnlyList<BusinessToReadInList>>> GetAllAsync();
        Task<Result<BusinessToRead>> GetAsync(long id);
        Task<Result<PostResponse>> AddAsync(BusinessToWrite business);
        Task<Result> UpdateAsync(BusinessToWrite business);
    }
}
