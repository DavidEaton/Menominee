using CSharpFunctionalExtensions;
using Entity = Menominee.Common.Entity;

namespace CustomerVehicleManagement.Domain.Entities.RepairOrders
{
    // TODO: DDD: Rename this class to TicketTax
    public class RepairOrderTax : Entity
    {
        public static readonly string RequiredMessage = $"Please include all required items.";
        public PartTax PartTax { get; private set; }
        public LaborTax LaborTax { get; private set; }
        private RepairOrderTax(PartTax partTax, LaborTax laborTax)
        {
            PartTax = partTax;
            LaborTax = laborTax;
        }

        public static Result<RepairOrderTax> Create(PartTax partTax, LaborTax laborTax)
        {
            // FluentValidation has already parsed caller's input
            // (value objects); no need to duplicate validation here.
            return Result.Success(new RepairOrderTax(partTax, laborTax));
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
        protected RepairOrderTax() { }

        #endregion
    }
}
