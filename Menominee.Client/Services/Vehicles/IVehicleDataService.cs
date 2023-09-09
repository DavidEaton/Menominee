using CSharpFunctionalExtensions;
using Menominee.Common.Enums;
using Menominee.Shared.Models;
using Menominee.Shared.Models.Vehicles;

namespace Menominee.Client.Services.Vehicles;

public interface IVehicleDataService
{
    Task<Result<PostResult>> AddVehicle(VehicleToWrite vehicle);
    Task<Result> DeleteVehicle(long id);
    Task<Result<VehicleToRead>> GetVehicle(long id);
    Task<Result<IReadOnlyList<VehicleToRead>>> GetVehicles(long customerId, SortOrder sortOrder, VehicleSortColumn sortColumn, bool includeInactive, string searchTerm);
    Task<Result> UpdateVehicle(VehicleToWrite vehicle);
}
