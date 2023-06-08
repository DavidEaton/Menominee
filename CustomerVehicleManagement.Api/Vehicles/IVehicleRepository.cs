using CustomerVehicleManagement.Domain.Entities;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.Vehicles
{
    public interface IVehicleRepository
    {
        Task<Vehicle> GetEntity(long id);

    }
}
