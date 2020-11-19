using CustomerVehicleManagement.Core.Enums;
using CustomerVehicleManagement.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerVehicleManagement.Core.Model
{
    public class Person : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public DateTime Birthday { get; set; }
        public Gender Gender { get; set; }
        public DriversLicence DriversLicence { get; set; }
        public int? DriversLicenceId { get; set; }
        public ICollection<Address> Addresses { get; set; }
        //public ICollection<PersonAddress> PersonAddresses { get; set; }
    }
}
