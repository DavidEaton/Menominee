using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenomineePlayWASM.Shared.Entities.Scheduler
{
    public class SchedulerAppointmentDetail
    {
        public long Id { get; set; }
        public long AppointmentId { get; set; }
        public string JobCode { get; set; }
        public string Description { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public bool IsAllDay { get; set; } = false;

    }
}
