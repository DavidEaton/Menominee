using Menominee.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Domain.Entities
{
    public class SaleCode : Entity
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
