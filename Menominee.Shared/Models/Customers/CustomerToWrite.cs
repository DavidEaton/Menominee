using Menominee.Domain.Enums;
using Menominee.Shared.Models.Businesses;
using Menominee.Shared.Models.Persons;
using Menominee.Shared.Models.Vehicles;
using System.Collections.Generic;

namespace Menominee.Shared.Models.Customers;

public class CustomerToWrite
{
    // TODO: This dto is somewhat unusual in that most ToWrite dtos
    // contain refernces to other entiites as ToRead dto properties.
    // This one contains references to other ToWrite dtos.  This is
    // to accommodate saving a new customer with a new person or business
    // in one go, AND their vehicle(s).
    public long Id { get; set; }
    public bool IsBusiness => Business?.IsNotEmpty ?? false;
    public bool IsPerson => Person?.IsNotEmpty ?? false;
    public EntityType EntityType { get; set; } = EntityType.Person;
    public CustomerType CustomerType { get; set; } = CustomerType.Retail;
    public PersonToWrite Person { get; set; } = new();
    public BusinessToWrite Business { get; set; } = new();
    public string Code { get; set; } = null;
    public List<VehicleToWrite> Vehicles { get; set; } = new List<VehicleToWrite>();

    public string CustomerName =>
        Business?.IsNotEmpty is true ? Business.Name :
        Person?.IsNotEmpty is true ? Person.Name :
        string.Empty;
}
