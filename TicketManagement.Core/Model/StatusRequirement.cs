using SharedKernel.Enums;
using SharedKernel.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TicketManagement.Core.Enums;

namespace TicketManagement.Core.Model
{
    public class StatusRequirement : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public StatusRequirementType StatusRequirementType { get; set; }
        public StatusRequiredItem StatusRequiredItem { get; set; }

        // EF State management for disconnected data
        public void UpdateTrackingState(TrackingState state)
        {
            TrackingState = state;
        }

        [NotMapped]
        public TrackingState TrackingState { get; private set; }

    }
}
