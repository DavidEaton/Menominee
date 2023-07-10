using Menominee.Shared.Models.Organizations;

namespace Menominee.Shared.Models.Company
{
    public class CompanyToRead
    {
        public long Id { get; set; }
        public OrganizationToRead Organization { get; set; }
        public long NextInvoiceNumberOrSeed { get; set; }
    }
}
