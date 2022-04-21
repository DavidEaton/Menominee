using CustomerVehicleManagement.Shared.Models.Manufacturers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Menominee.Client.Services.Manufacturers
{
    public interface IManufacturerDataService
    {
        Task<IReadOnlyList<ManufacturerToReadInList>> GetAllManufacturersAsync();
        Task<ManufacturerToRead> GetManufacturerAsync(long id);
        Task<ManufacturerToRead> AddManufacturerAsync(ManufacturerToWrite manufacturer);
        Task UpdateManufacturerAsync(ManufacturerToWrite manufacturer, long id);
    }
}
