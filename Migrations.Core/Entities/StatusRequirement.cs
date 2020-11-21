using Migrations.Core.Enums;
using SharedKernel.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Migrations.Core.Entities
{
    public class StatusRequirement : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public StatusRequirementType StatusRequirementType { get; set; }
        public StatusRequiredItem StatusRequiredItem { get; set; }
    }
}
