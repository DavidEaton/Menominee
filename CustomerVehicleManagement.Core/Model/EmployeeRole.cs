using CustomerVehicleManagement.Core.Enums;
using SharedKernel.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerVehicleManagement.Core.Model
{
    public class EmployeeRole : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Employee Employee { get; set; }
        public EmploymentRole EmploymentRole { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime? ValidThru { get; set; }

    }
}
