using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TicketManagement.Core.Enums;

namespace TicketManagement.Core.Model
{
    public class StatusRequirement
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public StatusRequirementType StatusRequirementType { get; set; }
        public StatusRequiredItem StatusRequiredItem { get; set; }
    }
}
