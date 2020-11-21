using SharedKernel.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Migrations.Core.Entities
{
    public class Employee : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Person Person { get; set; }

        // Enitity Framework convenience property
        public int PersonId { get; set; }
        public DateTime Hired { get; set; }
        public DateTime Terminated { get; set; }
    }
}
