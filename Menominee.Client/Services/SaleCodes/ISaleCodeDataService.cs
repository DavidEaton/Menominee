using CSharpFunctionalExtensions;
using Menominee.Shared.Models.SaleCodes;

namespace Menominee.Client.Services.SaleCodes
{
    public interface ISaleCodeDataService
    {
        Task<IReadOnlyList<SaleCodeToReadInList>> GetAllSaleCodesAsync();
        Task<IReadOnlyList<SaleCodeShopSuppliesToReadInList>> GetAllSaleCodeShopSuppliesAsync();
        Task<SaleCodeToRead> GetSaleCodeAsync(long id);
        Task<Result<SaleCodeToRead>> AddSaleCodeAsync(SaleCodeToWrite saleCode);
        Task<Result> UpdateSaleCodeAsync(SaleCodeToWrite saleCode, long id);
    }
}
