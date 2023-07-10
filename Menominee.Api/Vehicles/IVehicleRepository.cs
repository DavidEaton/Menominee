using Menominee.Domain.Entities;
using System.Threading.Tasks;

namespace Menominee.Api.Vehicles
{
    public interface IVehicleRepository
    {
        Task<Vehicle> GetEntity(long id);

    }
}
