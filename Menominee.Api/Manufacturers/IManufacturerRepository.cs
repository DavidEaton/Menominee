﻿using Menominee.Domain.Entities.Inventory;
using Menominee.Shared.Models.Manufacturers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menominee.Api.Manufacturers
{
    public interface IManufacturerRepository
    {
        Task AddManufacturerAsync(Manufacturer manufacturer);
        Task<Manufacturer> GetManufacturerEntityAsync(string code);
        Task<Manufacturer> GetManufacturerEntityAsync(long id);
        Task<ManufacturerToRead> GetManufacturerAsync(string code);
        Task<ManufacturerToRead> GetManufacturerAsync(long id);
        Task<IReadOnlyList<ManufacturerToReadInList>> GetManufacturerListAsync();
        Task DeleteManufacturerAsync(long id);
        Task<bool> ManufacturerExistsAsync(string code);
        Task SaveChangesAsync();
        Task<IReadOnlyList<Manufacturer>> GetManufacturerEntitiesAsync(List<long> manufacturerIds);
    }
}