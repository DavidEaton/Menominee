using CustomerVehicleManagement.Domain.Entities.Inventory;
using FluentValidation;

namespace CustomerVehicleManagement.Shared.Models.Inventory.InventoryItems.Labor
{
    public class InventoryItemLaborValidator : AbstractValidator<InventoryItemLaborToWrite>
    {
        public InventoryItemLaborValidator()
        {
            RuleFor(laborDto => laborDto.LaborAmount)
                .MustBeValueObject(laborAmount =>
                    LaborAmount.Create(
                        laborAmount.PayType,
                        laborAmount.Amount));

            RuleFor(laborDto => laborDto.TechAmount)
                .MustBeValueObject(techAmount =>
                    TechAmount.Create(
                        techAmount.PayType,
                        techAmount.Amount,
                        techAmount.SkillLevel));
        }
    }
}
