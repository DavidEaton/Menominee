using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Shared.Models.Manufacturers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.Manufacturers
{
    public interface IManufacturerRepository
    {
        Task AddManufacturerAsync(Manufacturer manufacturer);
        Task<Manufacturer> GetManufacturerEntityAsync(string code);
        Task<ManufacturerToRead> GetManufacturerAsync(string code);
        Task<IReadOnlyList<ManufacturerToReadInList>> GetManufacturerListAsync();
        void UpdateManufacturerAsync(Manufacturer manufacturer);
        Task DeleteManufacturerAsync(string code);
        Task<bool> ManufacturerExistsAsync(string code);
        Task<bool> SaveChangesAsync();
        void FixTrackingState();
    }
}
