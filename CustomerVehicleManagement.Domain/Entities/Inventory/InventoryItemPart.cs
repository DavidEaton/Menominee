using CSharpFunctionalExtensions;

namespace CustomerVehicleManagement.Domain.Entities.Inventory
{
    public class InventoryItemPart : InstallablePart
    {
        private InventoryItemPart(double list, double cost, double core, double retail, TechAmount techAmount, string lineCode, string subLineCode, bool fractional)
            : base(list, cost, core, retail, techAmount, lineCode, subLineCode, fractional)
        { }

        public static Result<InventoryItemPart> Create(double list, double cost, double core, double retail, TechAmount techAmount, bool fractional, string lineCode = null, string subLineCode = null)
        {
            return Result.Success(new InventoryItemPart(list, cost, core, retail, techAmount, lineCode, subLineCode, fractional));
        }

        #region ORM

        // EF requires a parameterless constructor
        protected InventoryItemPart() { }

        #endregion
    }
}
