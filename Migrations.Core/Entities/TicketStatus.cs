using SharedKernel.Enums;
using SharedKernel.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Migrations.Core.Entities
{
    public class TicketStatus : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int Ordinal { get; set; }
        public string Name { get; set; }
        public bool CarryOver { get; set; }
        public bool SendSms { get; set; }
        public int Color { get; set; }
        //public SmsTemplate DefaultSmsTemplate { get; set; }
        public ICollection<StatusRequirement> StatusRequirements { get; set; }

        // EF State management for disconnected data
        public void UpdateState(TrackingState state)
        {
            TrackingState = state;
        }

        [NotMapped]
        public TrackingState TrackingState { get; private set; }

    }
}
