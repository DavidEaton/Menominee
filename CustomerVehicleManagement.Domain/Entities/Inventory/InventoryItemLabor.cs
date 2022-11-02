using CSharpFunctionalExtensions;
using Entity = Menominee.Common.Entity;

namespace CustomerVehicleManagement.Domain.Entities.Inventory
{
    public class InventoryItemLabor : Entity
    {
        public static readonly string RequiredMessage = "Please include all required items.";

        public LaborAmount LaborAmount { get; private set; }
        public TechAmount TechAmount { get; private set; }

        private InventoryItemLabor(LaborAmount laborAmount, TechAmount techAmount)
        {
            LaborAmount = laborAmount;
            TechAmount = techAmount;
        }

        public static Result<InventoryItemLabor> Create(LaborAmount laborAmount, TechAmount techAmount)
        {
            if (laborAmount is null || techAmount is null)
                return Result.Failure<InventoryItemLabor>(RequiredMessage);

            return Result.Success(new InventoryItemLabor(laborAmount, techAmount));
        }

        public Result<LaborAmount> SetLaborAmount(LaborAmount laborAmount)
        {
            if (laborAmount is null)
                return Result.Failure<LaborAmount>(RequiredMessage);

            return Result.Success(LaborAmount = laborAmount);
        }

        public Result<TechAmount> SetTechAmount(TechAmount techAmount)
        {
            if (techAmount is null)
                return Result.Failure<TechAmount>(RequiredMessage);

            return Result.Success(TechAmount = techAmount);
        }


        #region ORM

        // EF requires a parameterless constructor
        protected InventoryItemLabor() { }

        #endregion
    }
}
