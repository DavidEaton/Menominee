using System.ComponentModel.DataAnnotations;

namespace CustomerVehicleManagement.Shared.Models.Payables.Vendors
{
    public class VendorToWrite
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Vendor Name is required.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vendor Code is required.")]
        public string VendorCode { get; set; } = string.Empty;

        public bool? IsActive { get; set; } = true;
    }
}
