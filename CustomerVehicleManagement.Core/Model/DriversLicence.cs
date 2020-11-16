using CustomerVehicleManagement.Core.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerVehicleManagement.Core.Model
{
    public class DriversLicence : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Number { get; set; }
        [Required]
        public DateTimeRange ValidFromThru { get; set; }
        [Required]
        public string State { get; set; }
    }
}
