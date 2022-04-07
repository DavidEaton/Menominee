using CustomerVehicleManagement.Domain.Entities.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Shared.Models.Inventory
{
    public class InventoryTireToRead
    {
        public long Id { get; set; }
        public string Type { get; set; }
        public double Width { get; set; }
        public double AspectRatio { get; set; }
        public double Diameter { get; set; }
        public int LoadIndex { get; set; }
        public string SpeedRating { get; set; }

        public static InventoryTireToRead ConvertToDto(InventoryTire tire)
        {
            if (tire != null)
            {
                return new InventoryTireToRead
                {
                    Id = tire.Id,
                    Type = tire.Type,
                    Width = tire.Width,
                    AspectRatio = tire.AspectRatio,
                    Diameter = tire.Diameter,
                    LoadIndex = tire.LoadIndex,
                    SpeedRating = tire.SpeedRating
                };
            }

            return null;
        }
    }
}
