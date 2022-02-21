using CustomerVehicleManagement.Shared.Models.Manufacturers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Menominee.Client.Services.Manufacturers
{
    public interface IManufacturerDataService
    {
        Task<IReadOnlyList<ManufacturerToReadInList>> GetAllManufacturers();
        Task<ManufacturerToRead> GetManufacturer(long id);
        Task<ManufacturerToRead> AddManufacturer(ManufacturerToWrite manufacturer);
        Task UpdateManufacturer(ManufacturerToWrite manufacturer, long id);
    }
}
