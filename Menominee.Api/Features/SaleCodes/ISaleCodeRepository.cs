using CSharpFunctionalExtensions;
using Menominee.Domain.Entities;
using Menominee.Shared.Models.SaleCodes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menominee.Api.Features.SaleCodes
{
    public interface ISaleCodeRepository
    {
        void Add(SaleCode entity);
        void Delete(SaleCode entity);
        Task<SaleCode> GetEntityAsync(string code);
        Task<SaleCode> GetEntityAsync(long id);
        Task<SaleCodeToRead> GetAsync(long id);
        Task<IReadOnlyList<SaleCodeToReadInList>> GetListAsync();
        Task<IReadOnlyList<SaleCodeShopSuppliesToReadInList>> GetShopSuppliesListAsync();
        Task<IReadOnlyList<SaleCodeShopSuppliesToRead>> GetShopSuppliesAsync(long Id);
        Task SaveChangesAsync();
        Task<Result<IReadOnlyList<SaleCode>>> GetSaleCodeEntitiesAsync(List<long> ids, bool all = false);
    }
}
