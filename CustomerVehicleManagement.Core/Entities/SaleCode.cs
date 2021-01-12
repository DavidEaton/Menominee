using SharedKernel.Enums;
using SharedKernel.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerVehicleManagement.Domain.Entities
{
    public class SaleCode : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }

        // EF State management for disconnected data
        public void UpdateState(TrackingState state)
        {
            TrackingState = state;
        }

        [NotMapped]
        public TrackingState TrackingState { get; private set; }

    }
}
