using Menominee.Domain.Interfaces;

namespace Menominee.Shared.Models.Contactable
{
    public class EmailToWrite : IHasPrimary
    {
        public long Id { get; set; }
        public string Address { get; set; } = string.Empty;
        public bool IsPrimary { get; set; } = false;
    }
}