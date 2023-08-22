using Menominee.Domain.Entities.Inventory;
using Menominee.Shared.Models.Manufacturers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menominee.Api.Manufacturers
{
    public interface IManufacturerRepository
    {
        Task AddManufacturerAsync(Manufacturer manufacturer);
        Task<Manufacturer> GetManufacturerEntityAsync(long id);
        Task<ManufacturerToRead> GetManufacturerAsync(long id);
        Task<IReadOnlyList<ManufacturerToReadInList>> GetManufacturerListAsync();
        Task DeleteManufacturerAsync(long id);
        Task<bool> ManufacturerExistsAsync(long id);
        Task<IReadOnlyList<Manufacturer>> GetManufacturerEntitiesAsync(List<long> manufacturerIds);
        Task<List<string>> GetExistingPrefixList();
        Task<List<long>> GetExistingIdList();
        Task ExecuteInTransactionAsync(Func<Task> operations);
        Task ToggleIdentityInsert(bool enable);
        long DetermineManufacturerId(List<long> existingIds);
        Task SaveChangesAsync();
    }
}
