using CustomerVehicleManagement.Domain.Entities.Inventory;

namespace CustomerVehicleManagement.Shared.Models.Manufacturers
{
    public class ManufacturerToReadInList
    {
        public long Id { get; set; }
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
                    Id = mfr.Id,
                    Code = mfr.Code,
                    Prefix = mfr.Prefix,
                    Name = mfr.Name
                };
            }

            return null;
        }
    }
}
