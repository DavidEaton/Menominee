using Menominee.Common.Enums;
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
    public EntityType EntityType { get; set; }
    public CustomerType CustomerType { get; set; }
    public PersonToWrite Person { get; set; }
    public BusinessToWrite Business { get; set; }
    public string Code { get; set; } = null;
    public List<VehicleToWrite> Vehicles { get; set; } = new List<VehicleToWrite>();
}
