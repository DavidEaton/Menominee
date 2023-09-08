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

        public static VehicleToWrite ConvertToWriteDto(Vehicle vehicle)
        {
            return vehicle is null
                ? null
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
                    Active = vehicle.Active
                };
        }

        public static Vehicle ConvertWriteDtoToEntity(VehicleToWrite vehicle)
        {
            return vehicle is null
                ? null
                : Vehicle.Create(
                    vehicle.VIN,
                    vehicle.Year,
                    vehicle.Make,
                    vehicle.Model,
                    vehicle.Plate,
                    vehicle.PlateStateProvince,
                    vehicle.UnitNumber,
                    vehicle.Color,
                    vehicle.Active).Value;
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
                        Active = vehicle.Active,
                        NonTraditionalVehicle = vehicle.NonTraditionalVehicle
                    })
                .ToList()
                ?? new List<VehicleToRead>();
        }

        public static List<VehicleToWrite> ConvertToWriteDtos(IReadOnlyList<Vehicle> vehicles)
        {
            return vehicles?
                .Select(vehicle => ConvertToWriteDto(vehicle))
                .ToList()
                ?? new List<VehicleToWrite>();
        }

        public static VehicleToWrite ConvertReadToWriteDto(VehicleToRead vehicle)
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

        public static List<VehicleToWrite> ConvertReadToWriteDtos(IReadOnlyList<VehicleToRead> vehicles)
        {
            return vehicles?
                .Select(vehicle => ConvertReadToWriteDto(vehicle))
                .ToList()
                ?? new List<VehicleToWrite>();
        }
    }
}
