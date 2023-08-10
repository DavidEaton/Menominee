using Menominee.Shared.Models.Businesses;

namespace Menominee.Shared.Models.Company
{
    public class CompanyToRead
    {
        public long Id { get; set; }
        public BusinessToRead Business { get; set; }
        public long NextInvoiceNumberOrSeed { get; set; }
    }
}
