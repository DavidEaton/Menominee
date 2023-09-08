using Menominee.Api.Data;
using Menominee.Domain.Entities;
using Menominee.Shared.Models.Vehicles;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Menominee.Api.Vehicles;

public class VehicleRepository : IVehicleRepository
{
    private readonly ApplicationDbContext context;

    public VehicleRepository(ApplicationDbContext context)
    {
        this.context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public void AddVehicle(Vehicle entity)
    {
        var existingEntity = context.Vehicles.Local
            .FirstOrDefault(vehicle => vehicle.Id.Equals(entity.Id));

        if (existingEntity is not null)
        {
            context.Entry(existingEntity).State = EntityState.Detached;
        }

        context.Vehicles.Attach(entity);
    }

    public void DeleteVehicle(Vehicle entity)
    {
        context.Vehicles.Remove(entity);
    }

    public void DeleteVehicles(IReadOnlyList<Vehicle> entities)
    {
        context.Vehicles.RemoveRange(entities);
    }

    public async Task<Vehicle> GetEntityAsync(long id)
    {
        return await context.Vehicles.FirstOrDefaultAsync(vehicle => vehicle.Id == id);
    }

    public async Task<Vehicle> GetEntityAsync(string vin)
    {
        return await context.Vehicles.FirstOrDefaultAsync(vehicle => vehicle.VIN == vin);
    }

    public async Task<IReadOnlyList<VehicleToRead>> GetVehiclesAsync()
    {
        return await context.Vehicles
            .AsNoTracking()
            .AsSplitQuery()
            .Select(vehicle => VehicleHelper.ConvertToReadDto(vehicle))
            .ToListAsync();
    }

    public async Task SaveChanges()
    {
        await context.SaveChangesAsync();
    }
}
