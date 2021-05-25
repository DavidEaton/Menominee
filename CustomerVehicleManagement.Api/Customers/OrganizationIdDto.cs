using System.Text.Json.Serialization;

namespace CustomerVehicleManagement.Api.Customers
{
    public class OrganizationIdDto
    {
        public OrganizationIdDto(int id)
        {
            Id = id;
        }

        public int Id { get; set; }
    }
}