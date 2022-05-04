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
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public double LaborRate { get; set; }
        public double DesiredMargin { get; set; }

        public static SaleCodeToReadInList ConvertToDto(SaleCode sc)
        {
            if (sc != null)
            {
                return new SaleCodeToReadInList
                {
                    Id = sc.Id,
                    Code = sc.Code,
                    Name = sc.Name,
                    LaborRate = sc.LaborRate,
                    DesiredMargin = sc.DesiredMargin
                };
            }

            return null;
        }
    }
}
