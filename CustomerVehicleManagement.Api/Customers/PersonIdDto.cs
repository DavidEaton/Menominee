using System.Text.Json.Serialization;

namespace CustomerVehicleManagement.Api.Customers
{
    public class PersonIdDto
    {
        [JsonConstructor]
        public PersonIdDto(int id)
        {
            Id = id;
        }

        public int Id { get; set; }
    }
}