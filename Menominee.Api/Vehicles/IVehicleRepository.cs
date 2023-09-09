using Menominee.Common.Enums;
using Menominee.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menominee.Api.Vehicles;

public interface IVehicleRepository
{
    Task<IReadOnlyList<Vehicle>> GetVehiclesAsync(long customerId, SortOrder sortOrder, VehicleSortColumn sortColumn, bool includeInactive, string searchTerm);
    Task<Vehicle> GetEntityAsync(long id);
    Task<Vehicle> GetEntityAsync(string vin);
    void AddVehicle(Vehicle entity);
    void DeleteVehicle(Vehicle entity);
    void DeleteVehicles(IReadOnlyList<Vehicle> entities);
    Task SaveChanges();
}
