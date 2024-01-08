using CSharpFunctionalExtensions;
using Menominee.Domain.Enums;
using Menominee.Shared.Models.Http;
using Menominee.Shared.Models.Vehicles;

namespace Menominee.Client.Services.Vehicles;

public interface IVehicleDataService
{
    Task<Result<PostResponse>> AddAsync(VehicleToWrite vehicle);
    Task<Result> DeleteAsync(long id);
    Task<Result<VehicleToRead>> GetAsync(long id);
    Task<Result<IReadOnlyList<VehicleToRead>>> GetByParametersAsync(long customerId, SortOrder sortOrder, VehicleSortColumn sortColumn, bool includeInactive, string searchTerm);
    Task<Result> UpdateAsync(VehicleToWrite vehicle);
}