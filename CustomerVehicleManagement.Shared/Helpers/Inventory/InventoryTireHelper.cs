using CustomerVehicleManagement.Domain.Entities.Inventory;
using CustomerVehicleManagement.Shared.Models.Inventory;

namespace CustomerVehicleManagement.Shared.Helpers.Inventory
{
    public class InventoryTireHelper
    {
        public static InventoryTire CreateEntityFromWriteDto(InventoryTireToWrite tire)
        {
            return new()
            {
                Type = tire.Type,
                Width = tire.Width,
                AspectRatio = tire.AspectRatio,
                Diameter = tire.Diameter,
                LoadIndex = tire.LoadIndex,
                SpeedRating = tire.SpeedRating
            };
        }

        public static InventoryTireToWrite CreateWriteDtoFromReadDto(InventoryTireToRead tire)
        {
            return new()
            {
                Type = tire.Type,
                Width = tire.Width,
                AspectRatio = tire.AspectRatio,
                Diameter = tire.Diameter,
                LoadIndex = tire.LoadIndex,
                SpeedRating = tire.SpeedRating
            };
        }
    }
}
