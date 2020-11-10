using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntityApp.Domain.Classes
{
    public class Organization : Party, IEntity
    {

        [Required]
        public string Name { get; set; }

        public Person Contact { get; set; }

    }
}
