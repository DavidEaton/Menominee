using CustomerVehicleManagement.Domain.Entities.Inventory;
using FluentValidation;

namespace CustomerVehicleManagement.Shared.Models.Inventory
{
    public class MaintenanceItemValidator : AbstractValidator<MaintenanceItemToWrite>
    {

        public MaintenanceItemValidator()
        {
            //RuleFor(maintenanceItem => maintenanceItem)
            //    .MustBeEntity(
            //        maintenanceItem => MaintenanceItem.Create(
            //            maintenanceItem.DisplayOrder,
            //            InventoryItem.Create(
            //                maintenanceItem.Item.Manufacturer,
            //                maintenanceItem.Item.ItemNumber,
            //                maintenanceItem.Item.Description,
            //                maintenanceItem.Item.ProductCode,
            //                maintenanceItem.Item.ItemType,
            //                maintenanceItem.Item.Part,
            //                maintenanceItem.Item.Labor,
            //                maintenanceItem.Item.Tire,
            //                maintenanceItem.Item.Package,
            //                maintenanceItem.Item.Inspection,
            //                maintenanceItem.Item.Warranty).Value));

        }
    }
}
