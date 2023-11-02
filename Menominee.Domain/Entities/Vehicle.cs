using CSharpFunctionalExtensions;
using Menominee.Common.Enums;
using System;
using Entity = Menominee.Common.Entity;

namespace Menominee.Domain.Entities;

public class Vehicle : Entity
{
    public static readonly int MaximumMakeModelLength = 50;
    public static readonly int MinimumMakeModelLength = 2;
    public static readonly int VinLength = 17;
    public static readonly int MaximumPlateLength = 20;
    public static readonly int MaximumUnitNumberLength = 20;
    public static readonly int MaximumColorLength = 12;

    public static readonly string InvalidVinMessage = $"VIN was invalid";
    public const int YearMinimum = 1896; // First year of production commercial vehicles
    public static readonly string InvalidYearMessage = $"Year must be between {YearMinimum} and {DateTime.Today.Year + 1}";
    public static readonly string InvalidLengthMessage = $"Make, Model must be between {MinimumMakeModelLength} and {MaximumMakeModelLength} characters in length";
    public static readonly string NonTraditionalVehicleInvalidMakeModelMessage = $"Please enter Make or Model";
    public static string InvalidMaximumLengthMessage(int max) => $"Value must be less than {max} characters in length";
    public static readonly string InvalidPlateStateProvinceMessage = $"Plate State/Province is invalid";

    public string VIN { get; private set; }
    public int? Year { get; private set; }
    public string Make { get; private set; }
    public string Model { get; private set; }
    public bool NonTraditionalVehicle { get; private set; } = false; // We need to allow for non-traditional vehicles. For example, they may be servicing a trailer and just type in TRAILER for the make and nothing else.
    public string Plate { get; private set; }
    public State? PlateStateProvince { get; private set; }
    public string UnitNumber { get; private set; }
    public string Color { get; private set; }
    public bool Active { get; private set; } = true;

    public override string ToString()
    {
        return $"{Year ?? 0} {Make} {Model}";
    }

    private Vehicle(string vin, int? year, string make, string model, bool nonTraditionalVehicle, string plate, State? plateStateProvince, string unitNumber, string color, bool active)
    {
        VIN = vin;
        Year = year;
        Make = make;
        Model = model;
        NonTraditionalVehicle = nonTraditionalVehicle;
        Plate = plate;
        PlateStateProvince = plateStateProvince;
        UnitNumber = unitNumber;
        Color = color;
        Active = active;
    }

    public static Result<Vehicle> Create(string vin, int? year, string make, string model, string plate, State? plateStateProvince, string unitNumber, string color, bool active = true, bool nonTraditionalVehicle = false)
    {
        make = (make ?? string.Empty).Trim();
        model = (model ?? string.Empty).Trim();
        plate = (plate ?? string.Empty).Trim();
        unitNumber = (unitNumber ?? string.Empty).Trim();
        color = (color ?? string.Empty).Trim();

        var vinResult = ValidateVin(vin);
        if (vinResult.IsFailure)
            return Result.Failure<Vehicle>(vinResult.Error);

        var makeModelResult = ValidateMakeModel(make, model, nonTraditionalVehicle);
        if (makeModelResult.IsFailure)
            return Result.Failure<Vehicle>(makeModelResult.Error);

        var yearResult = ValidateYear(year);
        if (yearResult.IsFailure)
            return Result.Failure<Vehicle>(yearResult.Error);

        var plateResult = ValidatePlate(plate);
        if (plateResult.IsFailure)
            return Result.Failure<Vehicle>(plateResult.Error);

        var plateStateProvinceResult = ValidatePlateStateProvince(plateStateProvince);
        if (plateStateProvinceResult.IsFailure)
            return Result.Failure<Vehicle>(plateStateProvinceResult.Error);

        var unitNumberResult = ValidateUnitNumber(unitNumber);
        if (unitNumberResult.IsFailure)
            return Result.Failure<Vehicle>(unitNumberResult.Error);

        var colorResult = ValidateColor(color);
        if (colorResult.IsFailure)
            return Result.Failure<Vehicle>(colorResult.Error);

        return Result.Success(new Vehicle(vin, year, make, model, nonTraditionalVehicle, plate, plateStateProvince, unitNumber, color, active));
    }

    private static Result ValidateMakeModel(string make, string model, bool nonTraditionalVehicle)
    {
        if (!nonTraditionalVehicle)
        {
            if (string.IsNullOrWhiteSpace(make) || string.IsNullOrWhiteSpace(model))
                return Result.Failure(InvalidLengthMessage);

            if (make.Length < MinimumMakeModelLength || make.Length > MaximumMakeModelLength ||
                model.Length < MinimumMakeModelLength || model.Length > MaximumMakeModelLength)
                return Result.Failure(InvalidLengthMessage);
        }

        if (nonTraditionalVehicle)
            if (string.IsNullOrWhiteSpace(make) && string.IsNullOrWhiteSpace(model))
                return Result.Failure(NonTraditionalVehicleInvalidMakeModelMessage);

        return Result.Success();
    }

    private static Result ValidateVin(string vin)
    {
        if (vin is null) return Result.Success();

        return vin.Length.Equals(VinLength)
            ? Result.Success()
            : Result.Failure(InvalidVinMessage);
    }

    private static Result ValidateYear(int? year)
    {
        if (year > DateTime.Today.Year + 1 || year < YearMinimum)
            return Result.Failure(InvalidYearMessage);

        return Result.Success();
    }

    private static Result ValidatePlate(string plate)
    {
        if (string.IsNullOrWhiteSpace(plate)) return Result.Success();

        return plate.Length <= MaximumPlateLength
            ? Result.Success()
            : Result.Failure(InvalidMaximumLengthMessage(MaximumPlateLength));
    }

    private static Result ValidatePlateStateProvince(State? plateStateProvince)
    {
        if (plateStateProvince is null) return Result.Success();

        return Enum.IsDefined(typeof(State), plateStateProvince)
            ? Result.Success()
            : Result.Failure(InvalidPlateStateProvinceMessage);
    }

    private static Result ValidateUnitNumber(string unitNumber)
    {
        if (string.IsNullOrWhiteSpace(unitNumber)) return Result.Success();

        return unitNumber.Length <= MaximumUnitNumberLength
            ? Result.Success()
            : Result.Failure(InvalidMaximumLengthMessage(MaximumUnitNumberLength));
    }

    private static Result ValidateColor(string color)
    {
        if (string.IsNullOrWhiteSpace(color)) return Result.Success();

        return color.Length <= MaximumColorLength
            ? Result.Success()
            : Result.Failure(InvalidMaximumLengthMessage(MaximumColorLength));
    }

    public Result<string> SetVin(string vin)
    {
        if (vin is null) return Result.Success(VIN = vin);

        return vin.Length.Equals(VinLength)
            ? Result.Success(VIN = vin)
            : Result.Failure<string>(InvalidVinMessage);
    }

    public Result<int?> SetYear(int? year)
    {
        if (year > DateTime.Today.Year + 1 || year < YearMinimum)
            return Result.Failure<int?>(InvalidYearMessage);

        return Result.Success(Year = year);
    }

    public Result<string> SetMake(string make)
    {
        make = (make ?? string.Empty).Trim();

        if (make.Length < MinimumMakeModelLength || make.Length > MaximumMakeModelLength)
            return Result.Failure<string>(InvalidLengthMessage);

        return Result.Success(Make = make);
    }

    public Result<string> SetModel(string model)
    {
        model = (model ?? string.Empty).Trim();

        if (model.Length < MinimumMakeModelLength || model.Length > MaximumMakeModelLength)
            return Result.Failure<string>(InvalidLengthMessage);

        return Result.Success(Model = model);
    }

    public Result<string> SetPlate(string plate)
    {
        if (string.IsNullOrWhiteSpace(plate)) return Result.Success(Plate = plate);

        return plate.Length <= MaximumPlateLength
            ? Result.Success(Plate = plate)
            : Result.Failure<string>(InvalidMaximumLengthMessage(MaximumPlateLength));
    }

    public Result<State?> SetPlateStateProvince(State? plateStateProvince)
    {
        if (plateStateProvince is null) return Result.Success(PlateStateProvince = plateStateProvince);

        return Enum.IsDefined(typeof(State), plateStateProvince)
            ? Result.Success(PlateStateProvince = plateStateProvince)
            : Result.Failure<State?>(InvalidPlateStateProvinceMessage);
    }

    public Result<string> SetUnitNumber(string unitNumber)
    {
        if (string.IsNullOrWhiteSpace(unitNumber)) return Result.Success(UnitNumber = unitNumber);

        return unitNumber.Length <= MaximumUnitNumberLength
            ? Result.Success(UnitNumber = unitNumber)
            : Result.Failure<string>(InvalidMaximumLengthMessage(MaximumUnitNumberLength));
    }

    public Result<string> SetColor(string color)
    {
        if (string.IsNullOrWhiteSpace(color)) return Result.Success(Color = color);

        return color.Length <= MaximumColorLength
            ? Result.Success(Color = color)
            : Result.Failure<string>(InvalidMaximumLengthMessage(MaximumColorLength));
    }

    public Result<bool> SetActive(bool active = true)
    {
        return Result.Success(Active = active);
    }

    public Result<bool> SetNonTraditionalVehicle(bool nonTraditionalVehicle)
    {
        return Result.Success(NonTraditionalVehicle = nonTraditionalVehicle);
    }

    #region ORM

    // EF requires a parameterless constructor
    protected Vehicle() { }

    #endregion
}
