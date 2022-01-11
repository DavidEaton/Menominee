using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenomineePlayWASM.Shared.Dtos.Inventory
{
    public class InventoryItemToReadInList
    {
        public long Id { get; set; }
        public string MfrId { get; set; }
        public string PartNumber { get; set; }
        public string Description { get; set; }
        public string PartType { get; set; }
        public double Retail { get; set; }
        public double Cost { get; set; }
        public double Core { get; set; }
        public double Labor { get; set; }
        public double OnHand { get; set; }
    }
}
