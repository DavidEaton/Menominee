using System.Collections.Generic;

namespace CustomerVehicleManagement.Api.Data.Models
{
    public class ContactReadDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<PhoneReadDto> Phones { get; set; } = new List<PhoneReadDto>();

    }
}
