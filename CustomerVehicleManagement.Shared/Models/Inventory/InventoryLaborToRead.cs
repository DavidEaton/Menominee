using CustomerVehicleManagement.Domain.Entities.Inventory;
using Menominee.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Shared.Models.Inventory
{
    public class InventoryLaborToRead
    {
        public long Id { get; set; }
        public ItemLaborType LaborType { get; set; }
        public double LaborAmount { get; set; }
        public ItemLaborType TechPayType { get; set; }
        public double TechPayAmount { get; set; }
        public SkillLevel SkillLevel { get; set; }

        public static InventoryLaborToRead ConvertToDto(InventoryLabor labor)
        {
            if (labor != null)
            {
                return new InventoryLaborToRead
                {
                    Id = labor.Id,
                    LaborType = labor.LaborType,
                    LaborAmount = labor.LaborAmount,
                    TechPayType = labor.TechPayType,
                    TechPayAmount = labor.TechPayAmount,
                    SkillLevel = labor.SkillLevel
                };
            }

            return null;
        }
    }
}
