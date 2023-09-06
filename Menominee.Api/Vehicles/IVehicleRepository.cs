using Menominee.Domain.Entities;
using Menominee.Shared.Models.Vehicles;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menominee.Api.Vehicles;

public interface IVehicleRepository
{
    Task<IReadOnlyList<VehicleToRead>> GetVehiclesAsync();
    Task<Vehicle> GetEntityAsync(long id);
    Task<Vehicle> GetEntityAsync(string vin);
    void AddVehicle(Vehicle entity);
    void DeleteVehicle(Vehicle entity);
    void DeleteVehicles(IReadOnlyList<Vehicle> entities);
    Task SaveChanges();
}
