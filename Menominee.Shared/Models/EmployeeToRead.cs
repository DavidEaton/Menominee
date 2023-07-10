using System;

namespace Menominee.Shared.Models
{
    public class EmployeeToRead
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public DateTime Hired { get; set; }
        public string ShopRole { get; set; }
    }
}
