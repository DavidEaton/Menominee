using CSharpFunctionalExtensions;
using Entity = Menominee.Common.Entity;

namespace Menominee.Domain.Entities.RepairOrders
{
    // TODO: DDD: Rename this class to ServiceLineTax
    public class RepairOrderItemTax : Entity
    {
        public static readonly string RequiredMessage = $"Please include all required items.";
        public PartTax PartTax { get; private set; }
        public LaborTax LaborTax { get; private set; }
        private RepairOrderItemTax(PartTax partTax, LaborTax laborTax)
        {
            PartTax = partTax;
            LaborTax = laborTax;
        }

        public static Result<RepairOrderItemTax> Create(PartTax partTax, LaborTax laborTax)
        {
            return partTax is null || laborTax is null
                ? Result.Failure<RepairOrderItemTax>(RequiredMessage)
                : Result.Success(new RepairOrderItemTax(partTax, laborTax));
        }

        public Result<PartTax> SetPartTax(PartTax partTax)
        {
            return
                partTax is null
                ? Result.Failure<PartTax>(RequiredMessage)
                : Result.Success(PartTax = partTax);
        }

        public Result<LaborTax> SetLaborTax(LaborTax laborTax)
        {
            return
                laborTax is null
                ? Result.Failure<LaborTax>(RequiredMessage)
                : Result.Success(LaborTax = laborTax);
        }

        #region ORM

        // EF requires a parameterless constructor
        protected RepairOrderItemTax() { }

        #endregion
    }
}
