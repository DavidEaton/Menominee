using SharedKernel.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerVehicleManagement.Core.Model
{
    public class Organization : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public Person Contact { get; set; }
        public ICollection<Address> Addresses { get; set; }

        //public ICollection<OrganizationAddress> OrganizationAddresses { get; set; }

    }
}
