using CustomerVehicleManagement.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerVehicleManagement.Domain.Entities
{
    public class OrganizationAddress
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Organization Organization { get; set; }
        public int OrganizationId { get; set; }
        public Address Address { get; set; }
        public int AddressId { get; set; }

    }
}
