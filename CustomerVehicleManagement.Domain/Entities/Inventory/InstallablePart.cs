using CSharpFunctionalExtensions;
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
        public static readonly int MaximumLineCodeLength = 10;
        public static readonly int MinimumMoneyAmount = 0;
        public static readonly int MaximumMoneyAmount = 99999;
        public static readonly string InvalidMoneyAmountMessage = $"Amount must be between {MinimumMoneyAmount} and {MaximumMoneyAmount}.";
        public static readonly string InvalidLineCodeLengthMessage = $"Line Code must not be more than {MaximumLineCodeLength} characters";
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
            if (list < MinimumMoneyAmount || list > MaximumMoneyAmount)
                return Result.Failure<double>(InvalidMoneyAmountMessage);

            return Result.Success(List = list);
        }

        public Result<double> SetCost(double cost)
        {
            if (cost < MinimumMoneyAmount || cost > MaximumMoneyAmount)
                return Result.Failure<double>(InvalidMoneyAmountMessage);

            return Result.Success(Cost = cost);
        }

        public Result<double> SetCore(double core)
        {
            if (core < MinimumMoneyAmount || core > MaximumMoneyAmount)
                return Result.Failure<double>(InvalidMoneyAmountMessage);

            return Result.Success(Core = core);
        }

        public Result<double> SetRetail(double retail)
        {
            if (retail < MinimumMoneyAmount || retail > MaximumMoneyAmount)
                return Result.Failure<double>(InvalidMoneyAmountMessage);

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
            if (lineCode.Length > MaximumLineCodeLength)
                return Result.Failure<string>(InvalidLineCodeLengthMessage);

            return Result.Success(LineCode = lineCode);
        }

        public Result<string> SetSubLineCode(string subLineCode)
        {
            if (subLineCode.Length > MaximumLineCodeLength)
                return Result.Failure<string>(InvalidLineCodeLengthMessage);

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
