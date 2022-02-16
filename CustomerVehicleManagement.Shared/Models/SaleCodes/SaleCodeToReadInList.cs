using CustomerVehicleManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Shared.Models.SaleCodes
{
    public class SaleCodeToReadInList
    {
        public string Code { get; set; }
        public string Name { get; set; }

        public static SaleCodeToReadInList ConvertToDto(SaleCode sc)
        {
            if (sc != null)
            {
                return new SaleCodeToReadInList
                {
                    Code = sc.Code,
                    Name = sc.Name
                };
            }

            return null;
        }
    }
}
