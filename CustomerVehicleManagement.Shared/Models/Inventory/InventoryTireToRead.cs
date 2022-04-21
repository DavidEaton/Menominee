using CustomerVehicleManagement.Domain.Entities.Inventory;
using Menominee.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Shared.Models.Inventory
{
    public class InventoryTireToRead
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
        public long Id { get; set; }
        public string Type { get; set; }
        public double Width { get; set; }
        public double AspectRatio { get; set; }
        public double Diameter { get; set; }
        public int LoadIndex { get; set; }
        public string SpeedRating { get; set; }

        public static InventoryTireToRead ConvertToDto(InventoryItemTire tire)
        {
            if (tire != null)
            {
                return new InventoryTireToRead
                {
                    Id = tire.Id,
                    List = tire.List,
                    Cost = tire.Cost,
                    Core = tire.Core,
                    Retail = tire.Retail,
                    TechPayType = tire.TechPayType,
                    TechPayAmount = tire.TechPayAmount,
                    LineCode = tire.LineCode,
                    SubLineCode = tire.SubLineCode,
                    Fractional = tire.Fractional,
                    SkillLevel = tire.SkillLevel,
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
