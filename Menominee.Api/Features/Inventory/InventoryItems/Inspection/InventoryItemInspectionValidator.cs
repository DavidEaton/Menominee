﻿using FluentValidation;
using Menominee.Api.Features.Inventory.InventoryItems.Labor;
using Menominee.Shared.Models.Inventory.InventoryItems.Inspection;

namespace Menominee.Api.Features.Inventory.InventoryItems.Inspection
{
    public class InventoryItemInspectionValidator : AbstractValidator<InventoryItemInspectionToWrite>
    {
        public InventoryItemInspectionValidator()
        {
            RuleFor(inspectionDto => inspectionDto.Type)
                .IsInEnum();

            RuleFor(inspectionDto => inspectionDto.LaborAmount)
                .SetValidator(new LaborAmountValidator())
                .When(inspectionDto => inspectionDto.LaborAmount is not null);

            RuleFor(inspectionDto => inspectionDto.TechAmount)
                .SetValidator(new TechAmountValidator())
                .When(inspectionDto => inspectionDto.TechAmount is not null);
        }
    }
}
