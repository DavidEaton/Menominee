using CSharpFunctionalExtensions;
using Menominee.Domain.Entities.Inventory;
using Entity = Menominee.Common.Entity;

namespace Menominee.Domain.Entities.RepairOrders
{
    public class RepairOrderItemLabor : Entity
    {
        public static readonly string RequiredMessage = "Please include all required items.";

        public LaborAmount LaborAmount { get; private set; }
        public TechAmount TechAmount { get; private set; }

        private RepairOrderItemLabor(LaborAmount laborAmount, TechAmount techAmount)
        {
            LaborAmount = laborAmount;
            TechAmount = techAmount;
        }

        public static Result<RepairOrderItemLabor> Create(LaborAmount laborAmount, TechAmount techAmount)
        {
            if (laborAmount is null || techAmount is null)
                return Result.Failure<RepairOrderItemLabor>(RequiredMessage);

            return Result.Success(new RepairOrderItemLabor(laborAmount, techAmount));
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
        protected RepairOrderItemLabor() { }

        #endregion
    }
}
