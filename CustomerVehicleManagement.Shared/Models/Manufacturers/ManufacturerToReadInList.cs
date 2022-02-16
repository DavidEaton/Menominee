using CustomerVehicleManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Shared.Models.Manufacturers
{
    public class ManufacturerToReadInList
    {
        public string Code { get; set; }
        public string Prefix { get; set; }
        public string Name { get; set; }
        //public xxx Country { get; set; }
        //public xxx Franchise { get; set; }

        public static ManufacturerToReadInList ConvertToDto(Manufacturer mfr)
        {
            if (mfr != null)
            {
                return new ManufacturerToReadInList
                {
                    Code = mfr.Code,
                    Prefix = mfr.Prefix,
                    Name = mfr.Name
                };
            }

            return null;
        }
    }
}
