using Menominee.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Domain.Entities
{
    public class Manufacturer : Entity
    {
        public string Code { get; set; }
        public string Prefix { get; set; }
        public string Name { get; set; }
        //public xxx Country { get; set; }
        //public xxx Franchise { get; set; }
    }
}
