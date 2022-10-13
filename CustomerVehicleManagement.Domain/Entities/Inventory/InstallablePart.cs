using CSharpFunctionalExtensions;
using System;
using Entity = Menominee.Common.Entity;

namespace CustomerVehicleManagement.Domain.Entities.Inventory
{
    public abstract class InstallablePart : Entity
    {
        // Always test only concrete classes; don’t test abstract classes directly.
        // Abstract classes are implementation details. Targeting tests at the
        // abstract base class binds them to the code’s implementation details, 
        // which is an anit-pattern.
        public static readonly string RequiredMessage = $"Please include all required items.";
        public static readonly int MinimumWidth = 10;
        public static readonly int MinimumValue = 0;
        public static readonly int MaximumValue = 99999;
        public static readonly string InvalidValueMessage = $"Value must be between {MinimumValue} and {MaximumValue}.";
        public static readonly int MinimumLength = 1;
        public static readonly int MaximumLength = 10;
        public static readonly string InvalidLengthMessage = $"Each item must be between {MinimumLength} and {MaximumLength} characters";
        public double List { get; private set; }
        public double Cost { get; private set; }
        public double Core { get; private set; }
        public double Retail { get; private set; }
        public TechAmount TechAmount { get; private set; }
        public string LineCode { get; private set; }
        public string SubLineCode { get; private set; }
        public bool Fractional { get; private set; }

        protected InstallablePart(double list, double cost, double core, double retail, TechAmount techAmount, bool fractional, string lineCode = null, string subLineCode = null)
        {
            if (list < MinimumValue ||
                cost < MinimumValue ||
                core < MinimumValue ||
                retail < MinimumValue ||
                list > MaximumValue ||
                cost > MaximumValue ||
                core > MaximumValue ||
                retail > MaximumValue)
                throw new ArgumentOutOfRangeException(InvalidValueMessage);

            // TechAmount is validated before we ever get here

            lineCode = (lineCode ?? string.Empty).Trim();
            subLineCode = (subLineCode ?? string.Empty).Trim();

            if (lineCode.Length > MaximumLength || subLineCode.Length > MaximumLength)
                throw new ArgumentOutOfRangeException(InvalidLengthMessage);

            List = list;
            Cost = cost;
            Core = core;
            Retail = retail;
            TechAmount = techAmount;
            LineCode = lineCode;
            SubLineCode = subLineCode;
            Fractional = fractional;
        }

        public Result<double> SetList(double list)
        {
            if (list < MinimumValue || list > MaximumValue)
                return Result.Failure<double>(InvalidValueMessage);

            return Result.Success(List = list);
        }

        public Result<double> SetCost(double cost)
        {
            if (cost < MinimumValue || cost > MaximumValue)
                return Result.Failure<double>(InvalidValueMessage);

            return Result.Success(Cost = cost);
        }

        public Result<double> SetCore(double core)
        {
            if (core < MinimumValue || core > MaximumValue)
                return Result.Failure<double>(InvalidValueMessage);

            return Result.Success(Core = core);
        }

        public Result<double> SetRetail(double retail)
        {
            if (retail < MinimumValue || retail > MaximumValue)
                return Result.Failure<double>(InvalidValueMessage);

            return Result.Success(Retail = retail);
        }

        public Result<TechAmount> SetTechAmount(TechAmount techAmount)
        {
            if (techAmount is null)
                return Result.Failure<TechAmount>(RequiredMessage);

            return Result.Success(TechAmount = techAmount);
        }

        public Result<string> SetLineCode(string lineCode)
        {
            if (lineCode.Length < MinimumLength || lineCode.Length > MaximumLength)
                return Result.Failure<string>(InvalidLengthMessage);

            return Result.Success(LineCode = lineCode);
        }

        public Result<string> SetSubLineCode(string subLineCode)
        {
            if (subLineCode.Length < MinimumLength || subLineCode.Length > MaximumLength)
                return Result.Failure<string>(InvalidLengthMessage);

            return Result.Success(SubLineCode = subLineCode);
        }

        public Result<bool> SetFractional(bool fractional)
        {
            return Result.Success(Fractional = fractional);
        }

        #region ORM

        // EF requires a parameterless constructor
        protected InstallablePart() { }

        #endregion
    }
}
