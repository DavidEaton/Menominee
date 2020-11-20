using CustomerVehicleManagement.Core.Enums;
using CustomerVehicleManagement.Core.Model;
using SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketManagement.Core.Model
{
    public class Customer : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; }
        public Person Person { get; }
        public Organization Organization { get; }
        public EntityType EntityType
        {
            get => (Organization != null) ? EntityType.Organization : EntityType.Person;

        }
        public CustomerType CustomerType { get; }
        public DateTime ValidFrom { get; }
        public IList<Vehicle> Vehicles { get; }
    }
}
