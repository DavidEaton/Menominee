using Menominee.Domain.Entities.Inventory;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menominee.Api.SellingPriceNames;

public interface ISellingPriceNameRepository
{
    void Add(SellingPriceName entity);
    void Delete(SellingPriceName entity);
    Task<IReadOnlyList<SellingPriceName>> GetAllAsync();
    Task<SellingPriceName> GetEntityAsync(long id);
    Task SaveChangesAsync();
}
