using CSharpFunctionalExtensions;

namespace CustomerVehicleManagement.Domain.Entities.Inventory
{
    public class InventoryItemPart : InstallablePart
    {
        public static readonly int MinimumLength = 1;
        public static readonly int MaximumLength = 10;
        public static readonly int MinimumWidth = 10;
        public static readonly int MaximumWidth = 999;
        public static readonly string InvalidLengthMessage = $"Each item must be between {MinimumLength} and {MaximumLength} characters";
        private InventoryItemPart(double list, double cost, double core, double retail, TechAmount techAmount, bool fractional, string lineCode = null, string subLineCode = null)
            : base(list, cost, core, retail, techAmount, fractional, lineCode, subLineCode)
        { }

        public static Result<InventoryItemPart> Create(double list, double cost, double core, double retail, TechAmount techAmount, bool fractional, string lineCode = null, string subLineCode = null)
        {
            if (list < MinimumMoneyAmount ||
                cost < MinimumMoneyAmount ||
                core < MinimumMoneyAmount ||
                retail < MinimumMoneyAmount ||
                list > MaximumMoneyAmount ||
                cost > MaximumMoneyAmount ||
                core > MaximumMoneyAmount ||
                retail > MaximumMoneyAmount)
                return Result.Failure<InventoryItemPart>(InvalidMoneyAmountMessage);

            lineCode = (lineCode ?? string.Empty).Trim();
            subLineCode = (subLineCode ?? string.Empty).Trim();

            if (lineCode.Length > MaximumLength || subLineCode.Length > MaximumLength)
                return Result.Failure<InventoryItemPart>(InvalidLengthMessage);

            return Result.Success(new InventoryItemPart(list, cost, core, retail, techAmount, fractional, lineCode, subLineCode));
        }

        #region ORM

        // EF requires a parameterless constructor
        protected InventoryItemPart() { }

        #endregion
    }
}
