using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Migrations.Core.Entities
{
    public class PersonAddress
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Person Person { get; set; }
        public int PersonId { get; set; }
        public Address Address { get; set; }
        public int AddressId { get; set; }
    }
}
