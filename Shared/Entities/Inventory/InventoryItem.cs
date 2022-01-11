using MenomineePlayWASM.Shared.Entities.Inventory.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MenomineePlayWASM.Shared.Entities.Inventory
{
    public class InventoryItem : Entity
    {
        public virtual Manufacturer Manufacturer {get; set;}
        public string PartNumber { get; set; }
        public string Description { get; set; }
        public InventoryItemType PartType { get; set; }
        public double Retail { get; set; }
        public double Cost { get; set; }
        public double Core { get; set; }
        public double Labor { get; set; }
        public double OnHand { get; set; }
    }
}
