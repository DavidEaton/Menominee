using Menominee.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Domain.Entities
{
    public class ProductCode : Entity
    {
        public virtual Manufacturer Manufacturer { get; set; }
        public string Code { get; set; }
        public virtual SaleCode SaleCode { get; set; }
        public string Name { get; set; }
    }
}
