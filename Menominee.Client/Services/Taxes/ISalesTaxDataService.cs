using CSharpFunctionalExtensions;
using Menominee.Common.Http;
using Menominee.Shared.Models.Taxes;

namespace Menominee.Client.Services.Taxes
{
    public interface ISalesTaxDataService
    {
        Task<Result<IReadOnlyList<SalesTaxToReadInList>>> GetAllAsync();
        Task<Result<SalesTaxToRead>> GetAsync(long id);
        Task<Result<PostResponse>> AddAsync(SalesTaxToWrite salesTax);
        Task<Result> UpdateAsync(SalesTaxToWrite salesTax);
    }
}