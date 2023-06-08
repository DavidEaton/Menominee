using CSharpFunctionalExtensions;
using CustomerVehicleManagement.Domain.Entities.Inventory;

namespace CustomerVehicleManagement.Domain.Entities.RepairOrders
{
    public class RepairOrderItemPart : InstallablePart
    {
        public static readonly int MinimumLength = 1;
        public static readonly int MaximumLength = 10;
        public static readonly string InvalidLengthMessage = $"Each item must be between {MinimumLength} and {MaximumLength} characters";
        private RepairOrderItemPart(double list, double cost, double core, double retail, TechAmount techAmount, bool fractional, string lineCode = null, string subLineCode = null)
            : base(list, cost, core, retail, techAmount, fractional, lineCode, subLineCode)
        { }

        public static Result<RepairOrderItemPart> Create(double list, double cost, double core, double retail, TechAmount techAmount, bool fractional, string lineCode = null, string subLineCode = null)
        {
            if (list < MinimumMoneyAmount ||
                cost < MinimumMoneyAmount ||
                core < MinimumMoneyAmount ||
                retail < MinimumMoneyAmount ||
                list > MaximumMoneyAmount ||
                cost > MaximumMoneyAmount ||
                core > MaximumMoneyAmount ||
                retail > MaximumMoneyAmount)
                return Result.Failure<RepairOrderItemPart>(InvalidMoneyAmountMessage);

            lineCode = (lineCode ?? string.Empty).Trim();
            subLineCode = (subLineCode ?? string.Empty).Trim();

            if (lineCode.Length > MaximumLength || subLineCode.Length > MaximumLength)
                return Result.Failure<RepairOrderItemPart>(InvalidLengthMessage);

            return Result.Success(new RepairOrderItemPart(list, cost, core, retail, techAmount, fractional, lineCode, subLineCode));
        }

        #region ORM

        // EF requires a parameterless constructor
        protected RepairOrderItemPart() { }

        #endregion
    }
}
