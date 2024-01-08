using Menominee.Domain.Entities;
using Menominee.Domain.Enums;
using Menominee.Shared.Models.Vehicles;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menominee.Api.Features.Vehicles;

public interface IVehicleRepository
{
    void Add(Vehicle entity);
    void Delete(Vehicle entity);
    Task<IReadOnlyList<Vehicle>> GetEntitiesAsync(long customerId, SortOrder sortOrder, VehicleSortColumn sortColumn, bool includeInactive, string searchTerm);
    Task<VehicleToRead> GetAsync(long id);
    Task<Vehicle> GetEntityAsync(long id);
    Task<Vehicle> GetEntityAsync(string vin);
    Task SaveChangesAsync();
}