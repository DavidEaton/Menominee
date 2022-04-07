using CustomerVehicleManagement.Domain.Entities.Inventory;
using Menominee.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Shared.Models.Inventory
{
    public class InventoryPartToReadInList
    {
        public double List { get; set; }
        public double Cost { get; set; }
        public double Core { get; set; }
        public double Retail { get; set; }
        public ItemLaborType TechPayType { get; set; }
        public double TechPayAmount { get; set; }
        public string LineCode { get; set; }
        public string SubLineCode { get; set; }
        public bool Fractional { get; set; }
        public SkillLevel SkillLevel { get; set; }

        public static InventoryPartToReadInList ConvertToDto(InventoryPart part)
        {
            if (part != null)
            {
                return new InventoryPartToReadInList()
                {
                    List = part.List,
                    Cost = part.Cost,
                    Core = part.Core,
                    Retail = part.Retail,
                    TechPayType = part.TechPayType,
                    TechPayAmount = part.TechPayAmount,
                    LineCode = part.LineCode,
                    SubLineCode = part.SubLineCode,
                    Fractional = part.Fractional,
                    SkillLevel = part.SkillLevel
                };
            }

            return null;
        }
    }
}
