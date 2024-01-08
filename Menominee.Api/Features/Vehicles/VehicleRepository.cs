using Menominee.Api.Data;
using Menominee.Domain.Entities;
using Menominee.Domain.Enums;
using Menominee.Shared.Models.Vehicles;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Menominee.Api.Features.Vehicles;

public class VehicleRepository : IVehicleRepository
{
    private readonly ApplicationDbContext context;

    public VehicleRepository(ApplicationDbContext context)
    {
        this.context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public void Add(Vehicle vehicle)
    {
        if (vehicle is not null)
            context.Attach(vehicle);
    }

    public void Delete(Vehicle vehicle)
    {
        if (vehicle is not null)
            context.Remove(vehicle);
    }

    public async Task<Vehicle> GetEntityAsync(long id)
    {
        return await context.Vehicles.FirstOrDefaultAsync(vehicle => vehicle.Id == id);
    }

    public async Task<Vehicle> GetEntityAsync(string vin)
    {
        return await context.Vehicles.FirstOrDefaultAsync(vehicle => vehicle.VIN == vin);
    }

    public async Task<IReadOnlyList<Vehicle>> GetEntitiesAsync(long customerId, SortOrder sortOrder, VehicleSortColumn sortColumn, bool includeInactive, string searchTerm)
    {
        var query = context.Vehicles
            .AsNoTracking()
            .AsSplitQuery();

        if (customerId > 0)
        {
            var customer = await context.Customers
                .Include(customer => customer.Vehicles)
                .AsNoTracking()
                .AsSplitQuery()
                .FirstOrDefaultAsync(customer => customer.Id == customerId);

            query = customer.Vehicles.AsQueryable();
        }

        if (!includeInactive)
        {
            query = query.Where(vehicle => vehicle.Active);
        }

        query = query.Where(GetFilterProperty(sortColumn, searchTerm));

        query = sortOrder == SortOrder.Asc
            ? query.OrderBy(GetSortProperty(sortColumn))
            : query.OrderByDescending(GetSortProperty(sortColumn));

        return await query.ToListAsync();
    }

    private static Expression<Func<Vehicle, bool>> GetFilterProperty(VehicleSortColumn sortColumn, string searchTerm)
    {
        return sortColumn switch
        {
            VehicleSortColumn.Plate => vehicle => vehicle.Plate.Contains(searchTerm),
            VehicleSortColumn.UnitNumber => vehicle => vehicle.UnitNumber.Contains(searchTerm),
            VehicleSortColumn.VIN => vehicle => vehicle.VIN.Contains(searchTerm),
            _ => vehicle => true
        };
    }

    private static Expression<Func<Vehicle, object>> GetSortProperty(VehicleSortColumn sortColumn)
    {
        return sortColumn switch
        {
            VehicleSortColumn.Plate => vehicle => vehicle.Plate,
            VehicleSortColumn.UnitNumber => vehicle => vehicle.UnitNumber,
            VehicleSortColumn.VIN => vehicle => vehicle.VIN,
            _ => vehicle => vehicle.Id
        };
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }

    public async Task<VehicleToRead> GetAsync(long id)
    {
        var vehicleFromContext = await context.Vehicles
            .FirstOrDefaultAsync(vehicle => vehicle.Id == id);

        return vehicleFromContext is not null
            ? VehicleHelper.ConvertToReadDto(vehicleFromContext)
            : null;
    }
}
