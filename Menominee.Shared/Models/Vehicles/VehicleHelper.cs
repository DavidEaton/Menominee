using Menominee.Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Menominee.Shared.Models.Vehicles
{
    public class VehicleHelper
    {
        public static VehicleToRead ConvertToReadDto(Vehicle vehicle)
        {
            return vehicle is null
                ? new()
                : new()
                {
                    Id = vehicle.Id,
                    Make = vehicle.Make,
                    Model = vehicle.Model,
                    Year = vehicle.Year,
                    VIN = vehicle.VIN,
                    Plate = vehicle.Plate,
                    PlateStateProvince = vehicle.PlateStateProvince,
                    UnitNumber = vehicle.UnitNumber,
                    Color = vehicle.Color,
                    Active = vehicle.Active,
                    NonTraditionalVehicle = vehicle.NonTraditionalVehicle
                };
        }

        public static List<VehicleToRead> ConvertToReadDtos(IReadOnlyList<Vehicle> vehicles)
        {
            return vehicles?
                .Select(vehicle =>
                    new VehicleToRead()
                    {
                        Id = vehicle.Id,
                        Make = vehicle.Make,
                        Model = vehicle.Model,
                        Year = vehicle.Year,
                        VIN = vehicle.VIN,
                        Plate = vehicle.Plate,
                        PlateStateProvince = vehicle.PlateStateProvince,
                        UnitNumber = vehicle.UnitNumber,
                        Color = vehicle.Color,
                        Active = vehicle.Active
                    })
                .ToList()
                ?? new List<VehicleToRead>();
        }
    }
}
