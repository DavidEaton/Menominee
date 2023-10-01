using CSharpFunctionalExtensions;
using Menominee.Common.Http;
using Menominee.Shared.Models.Businesses;

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
