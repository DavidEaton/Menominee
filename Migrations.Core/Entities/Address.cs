using SharedKernel.Enums;
using SharedKernel.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Migrations.Core.Entities
{
    public class Address : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string AddressLine { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        public string PostalCode { get; set; }

        [StringLength(2)]
        public string CountryCode { get; set; }

        [NotMapped]
        public TrackingState TrackingState { get; private set; }
        public void UpdateState(TrackingState state)
        {
            TrackingState = state;
        }
    }
}
