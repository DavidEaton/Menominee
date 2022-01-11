using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MenomineePlayWASM.Shared.Entities.Settings.Enums;

namespace MenomineePlayWASM.Shared.Entities.Settings
{
    public class CompanyInformation
    {
        [Required(ErrorMessage = "Company name is required")]
        public string Name { get; set; }
        public string Street1 { get; set; }
        public string Street2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Franchise { get; set; }
    }
}
