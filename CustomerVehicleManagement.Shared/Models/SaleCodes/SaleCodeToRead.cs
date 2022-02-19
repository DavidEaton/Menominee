using CustomerVehicleManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Shared.Models.SaleCodes
{
    public class SaleCodeToRead
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public static SaleCodeToRead ConvertToDto(SaleCode sc)
        {
            if (sc != null)
            {
                return new SaleCodeToRead
                {
                    Id = sc.Id,
                    Code = sc.Code,
                    Name = sc.Name
                };
            }

            return null;
        }
    }
}
