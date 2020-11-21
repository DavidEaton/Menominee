using Migrations.Core.Enums;
using SharedKernel.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Migrations.Core.Entities
{
    public class ServiceRequest : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Ticket Ticket { get; set; }
        public SaleCode SaleCode { get; set; }
        public string Description { get; set; }
        public VehicleArea VehicleArea { get; set; }
        public string Note { get; set; }
    }
}
