namespace MenomineePlayWASM.Shared.Entities.Payables.Vendors
{
    public class Vendor : Entity
    {
        //public long Id { get; set; }
        public string VendorCode { get; set; }
        public string Name { get; set; }
        public bool? IsActive { get; set; }
    }
}
