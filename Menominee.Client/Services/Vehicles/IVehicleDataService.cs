using CSharpFunctionalExtensions;
using Menominee.Common.Enums;
using Menominee.Common.Http;
using Menominee.Shared.Models.Vehicles;

namespace Menominee.Client.Services.Vehicles;

public interface IVehicleDataService
{
    Task<Result<PostResponse>> AddVehicle(VehicleToWrite vehicle);
    Task<Result> DeleteVehicle(long id);
    Task<Result<VehicleToRead>> GetVehicle(long id);
    Task<Result<IReadOnlyList<VehicleToRead>>> GetVehicles(long customerId, SortOrder sortOrder, VehicleSortColumn sortColumn, bool includeInactive, string searchTerm);
    Task<Result> UpdateVehicle(VehicleToWrite vehicle);
}
