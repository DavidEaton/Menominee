using Menominee.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Domain.Entities.Inventory
{
    public class InventoryTire : InventoryPart
    {
        public string Type { get; set; }
        public double Width { get; set; }
        public double AspectRatio { get; set; }
        public double Diameter { get; set; }
        public int LoadIndex { get; set; }
        public string SpeedRating { get; set; }
    }
}
