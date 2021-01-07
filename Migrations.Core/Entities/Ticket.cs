using SharedKernel;
using SharedKernel.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Migrations.Core.Entities
{
    public class Ticket : Entity
    {
        // EF requires an empty constructor
        protected Ticket() { }
        public Vehicle Vehicle { get; set; }
        public Customer Customer { get; set; }
        /// <summary>
        /// The Customer Name now lives within the Ticket and can provide 
        /// accurate information regardless of whether the Customer changed their name.
        /// We will always know who our Customer was when their Ticket was created.
        /// </summary>
        public string CustomerName { get; set; }
        public TicketStatus Status { get; set; }
        public Employee SalesPerson { get; set; }
        public ICollection<ServiceRequest> ServiceRequests { get; set; }

        //public ICollection<Item> Items { get; set; }
        //public ICollection<Job> Jobs { get; set; }

    }
}
