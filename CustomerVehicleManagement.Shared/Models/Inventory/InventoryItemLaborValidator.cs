using CustomerVehicleManagement.Domain.Entities.Inventory;
using FluentValidation;

namespace CustomerVehicleManagement.Shared.Models.Inventory
{
    public class InventoryItemLaborValidator : AbstractValidator<InventoryItemLaborToWrite>
    {
        public InventoryItemLaborValidator()
        {
            RuleFor(laborDto => laborDto)
                .MustBeEntity(
                    laborDto => InventoryItemLabor.Create(
                        LaborAmount.Create(
                            laborDto.LaborAmount.PayType,
                            laborDto.LaborAmount.Amount)
                        .Value,
                        TechAmount.Create(
                            laborDto.TechAmount.PayType,
                            laborDto.TechAmount.Amount,
                            laborDto.TechAmount.SkillLevel)
                        .Value));
        }
    }
}
