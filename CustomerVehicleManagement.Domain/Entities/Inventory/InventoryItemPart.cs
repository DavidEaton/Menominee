using CSharpFunctionalExtensions;

namespace CustomerVehicleManagement.Domain.Entities.Inventory
{
    public class InventoryItemPart : InstallablePart
    {

        private InventoryItemPart(InventoryItem item, double list, double cost, double core, double retail, TechAmount techAmount, string lineCode, string subLineCode, bool fractional)
            : base(item, list, cost, core, retail, techAmount, lineCode, subLineCode, fractional)
        { }

        public static Result<InventoryItemPart> Create(InventoryItem item, double list, double cost, double core, double retail, TechAmount techAmount, string lineCode, string subLineCode, bool fractional)
        {
            return Result.Success(new InventoryItemPart(item, list, cost, core, retail, techAmount, lineCode, subLineCode, fractional));
        }

        #region ORM

        // EF requires a parameterless constructor
        protected InventoryItemPart() { }

        #endregion
    }
}
