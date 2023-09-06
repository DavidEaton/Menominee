using Menominee.Domain.Entities.Inventory;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menominee.Api.SellingPriceNames;

public interface ISellingPriceNameRepository
{
    Task<IReadOnlyList<SellingPriceName>> GetAll();
    Task<SellingPriceName> GetEntity(long id);
    void Add(SellingPriceName entity);
    void Delete(SellingPriceName entity);
    Task SaveChanges();
}
