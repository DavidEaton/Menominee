using SharedKernel.Enums;
using SharedKernel.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketManagement.Core.Model
{
    public class Ticket : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Vehicle Vehicle { get; set; }
        public Customer Customer { get; set; }
        public TicketStatus Status { get; set; }
        public Employee SalesPerson { get; set; }
        public ICollection<ServiceRequest> ServiceRequests { get; set; }

        //public ICollection<Item> Items { get; set; }
        //public ICollection<Job> Jobs { get; set; }

        // EF State management for disconnected data
        public void UpdateState(TrackingState state)
        {
            TrackingState = state;
        }

        [NotMapped]
        public TrackingState TrackingState { get; private set; }

    }
}
