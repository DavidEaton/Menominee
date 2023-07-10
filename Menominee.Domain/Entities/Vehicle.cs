using CSharpFunctionalExtensions;
using System;
using Entity = Menominee.Common.Entity;

namespace Menominee.Domain.Entities
{
    public class Vehicle : Entity
    {
        public static readonly int MaximumLength = 50;
        public static readonly int MinimumLength = 2;
        public static readonly int VinLength = 17;
        public static readonly string InvalidVinMessage = $"VIN was invalid";
        public const int YearMinimum = 1896; // First year of production commercial vehicles
        public static readonly string InvalidYearMessage = $"Year must be between {YearMinimum} and {DateTime.Today.Year + 1}";
        public static readonly string InvalidLengthMessage = $"Make, Model must be between {MinimumLength} and {MaximumLength} characters in length";
        public static readonly string NonTraditionalVehicleInvalidMakeModelMessage = $"Please enter Make or Model";
        public string VIN { get; private set; }
        public int? Year { get; private set; }
        public string Make { get; private set; }
        public string Model { get; private set; }
        public bool NonTraditionalVehicle { get; private set; } = false; // We need to allow for non-traditional vehicles. For example, they may be servicing a trailer and just type in TRAILER for the make and nothing else.

        public override string ToString()
        {
            return $"{Year ?? 0} {Make} {Model}";
        }
        private Vehicle(string vin, int? year, string make, string model, bool nonTraditionalVehicle)
        {
            VIN = vin;
            Year = year;
            Make = make;
            Model = model;
            NonTraditionalVehicle = nonTraditionalVehicle;
        }

        public static Result<Vehicle> Create(string vin, int? year, string make, string model, bool nonTraditionalVehicle = false)
        {
            vin = (vin ?? string.Empty).Trim();
            make = (make ?? string.Empty).Trim();
            model = (model ?? string.Empty).Trim();

            var vinResult = ValidateVin(vin, nonTraditionalVehicle);
            if (vinResult.IsFailure)
                return Result.Failure<Vehicle>(vinResult.Error);

            var makeModelResult = ValidateMakeModel(make, model, nonTraditionalVehicle);
            if (makeModelResult.IsFailure)
                return Result.Failure<Vehicle>(makeModelResult.Error);

            var yearResult = ValidateYear(year);
            if (yearResult.IsFailure)
                return Result.Failure<Vehicle>(yearResult.Error);

            return Result.Success(new Vehicle(vin, year, make, model, nonTraditionalVehicle));
        }

        private static Result ValidateMakeModel(string make, string model, bool nonTraditionalVehicle)
        {
            if (!nonTraditionalVehicle)
            {
                if (string.IsNullOrWhiteSpace(make) || string.IsNullOrWhiteSpace(model))
                    return Result.Failure(InvalidLengthMessage);

                if (make.Length < MinimumLength || make.Length > MaximumLength ||
                    model.Length < MinimumLength || model.Length > MaximumLength)
                    return Result.Failure(InvalidLengthMessage);
            }

            if (nonTraditionalVehicle)
                if (string.IsNullOrWhiteSpace(make) && string.IsNullOrWhiteSpace(model))
                    return Result.Failure(NonTraditionalVehicleInvalidMakeModelMessage);

            return Result.Success();
        }

        private static Result ValidateVin(string vin, bool nonTraditionalVehicle)
        {

            if (!nonTraditionalVehicle && vin.Length != VinLength)
                return Result.Failure<Vehicle>(InvalidVinMessage);

            if (nonTraditionalVehicle)
                return Result.Success();

            return Result.Success();
        }

        private static Result ValidateYear(int? year)
        {
            if (year > DateTime.Today.Year + 1 || year < YearMinimum)
                return Result.Failure(InvalidYearMessage);

            return Result.Success();
        }

        // NO SetVin method. Like changing Entity.Id is not allowed,
        // so no SetId method in any domain class that derives from Entity

        public Result<int?> SetYear(int? year)
        {
            if (year > DateTime.Today.Year + 1 || year < YearMinimum)
                return Result.Failure<int?>(InvalidYearMessage);

            return Result.Success(Year = year);
        }

        public Result<string> SetMake(string make)
        {
            make = (make ?? string.Empty).Trim();

            if (make.Length < MinimumLength || make.Length > MaximumLength)
                return Result.Failure<string>(InvalidLengthMessage);

            return Result.Success(Make = make);
        }

        public Result<string> SetModel(string model)
        {
            model = (model ?? string.Empty).Trim();

            if (model.Length < MinimumLength || model.Length > MaximumLength)
                return Result.Failure<string>(InvalidLengthMessage);

            return Result.Success(Model = model);
        }

        #region ORM

        // EF requires a parameterless constructor
        protected Vehicle() { }

        #endregion
    }
}
