using CSharpFunctionalExtensions;
using Menominee.Domain.Entities.Inventory;
using Menominee.Shared.Models.Manufacturers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menominee.Api.Features.Manufacturers
{
    public interface IManufacturerRepository
    {
        void Add(Manufacturer entity);
        void Delete(Manufacturer entity);
        Task<Manufacturer> GetEntityAsync(long id);
        Task<ManufacturerToRead> GetAsync(long id);
        Task<IReadOnlyList<ManufacturerToReadInList>> GetListAsync();
        Task<Result<IReadOnlyList<Manufacturer>>> GetEntitiesAsync(List<long> manufacturerIds = null);
        Task<List<string>> GetExistingPrefixListAsync();
        Task<List<long>> GetExistingIdsAsync();
        Task ExecuteInTransactionAsync(Func<Task> operations);
        Task ToggleIdentityInsertAsync(bool enable);
        long GetNextManufacturerId(List<long> existingIds);
        Task SaveChangesAsync();
    }
}
