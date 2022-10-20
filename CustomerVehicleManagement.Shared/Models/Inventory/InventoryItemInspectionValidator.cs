using CustomerVehicleManagement.Domain.Entities.Inventory;
using FluentValidation;

namespace CustomerVehicleManagement.Shared.Models.Inventory
{
    public class InventoryItemInspectionValidator : AbstractValidator<InventoryItemInspectionToWrite>
    {
        public InventoryItemInspectionValidator()
        {
            RuleFor(inspectionDto => inspectionDto)
                .MustBeEntity(
                    inspectionDto => InventoryItemInspection.Create(
                        LaborAmount.Create(
                            inspectionDto.LaborAmount.PayType,
                            inspectionDto.LaborAmount.Amount)
                        .Value,
                        TechAmount.Create(
                            inspectionDto.TechAmount.PayType,
                            inspectionDto.TechAmount.Amount,
                            inspectionDto.TechAmount.SkillLevel)
                        .Value,
                        inspectionDto.Type));
        }
    }
}
