using CSharpFunctionalExtensions;
using Menominee.Common.Http;
using Menominee.Shared.Models.SaleCodes;

namespace Menominee.Client.Services.SaleCodes
{
    public interface ISaleCodeDataService
    {
        Task<IReadOnlyList<SaleCodeToReadInList>> GetAllAsync();
        Task<IReadOnlyList<SaleCodeShopSuppliesToReadInList>> GetAllShopSuppliesAsync();
        Task<SaleCodeToRead> GetAsync(long id);
        Task<Result<PostResponse>> AddAsync(SaleCodeToWrite saleCode);
        Task<Result> UpdateAsync(SaleCodeToWrite saleCode);
    }
}
