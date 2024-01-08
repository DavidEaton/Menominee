using CSharpFunctionalExtensions;
using Menominee.Shared.Models.Http;
using Menominee.Shared.Models.Taxes;

namespace Menominee.Client.Services.Taxes
{
    public interface IExciseFeeDataService
    {
        Task<Result<IReadOnlyList<ExciseFeeToReadInList>>> GetAllAsync();
        Task<Result<ExciseFeeToRead>> GetAsync(long id);
        Task<Result<PostResponse>> AddAsync(ExciseFeeToWrite exciseFee);
        Task<Result> UpdateAsync(ExciseFeeToWrite exciseFee);
    }
}