using Menominee.Api.Data;
using Menominee.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Menominee.Api.Vehicles
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly ApplicationDbContext context;

        public VehicleRepository(ApplicationDbContext context)
        {
            this.context = context ??
                throw new ArgumentNullException(nameof(context));
        }

        public async Task<Vehicle> GetEntity(long id)
        {
            return await context.Vehicles
                .FirstOrDefaultAsync(vehicle => vehicle.Id == id);
        }
    }
}
