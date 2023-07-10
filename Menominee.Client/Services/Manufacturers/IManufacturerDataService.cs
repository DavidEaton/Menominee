using Menominee.Shared.Models.Manufacturers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menominee.Client.Services.Manufacturers
{
    public interface IManufacturerDataService
    {
        Task<IReadOnlyList<ManufacturerToReadInList>> GetAllManufacturersAsync();
        Task<ManufacturerToRead> GetManufacturerAsync(long id);
        Task<ManufacturerToRead> GetManufacturerAsync(string code);
        Task<ManufacturerToRead> AddManufacturerAsync(ManufacturerToWrite manufacturer);
        Task UpdateManufacturerAsync(ManufacturerToWrite manufacturer, long id);
    }
}
