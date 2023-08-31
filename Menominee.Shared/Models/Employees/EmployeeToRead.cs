using Menominee.Common.Enums;
using System;

namespace Menominee.Shared.Models.Employees
{
    public class EmployeeToRead
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public Gender Gender { get; set; }
        public DateTime? Hired { get; set; }
        public string ShopRole { get; set; }
    }
}
