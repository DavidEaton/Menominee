using System;

namespace CustomerVehicleManagement.Shared.Models
{
    public class EmployeeReadDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public DateTime Hired { get; set; }
        public string ShopRole { get; set; }
    }
}
