using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenomineePlayWASM.Shared.Entities.Inventory
{
    public class ManufacturerOld : Entity
    {
        //public long Id { get; set; }
        public string MfrId { get; set; }
        public string Name { get; set; }
        public string Prefix { get; set; }
    }
}
