using Menominee.Common.Enums;
using Menominee.Shared.Models.Businesses;
using Menominee.Shared.Models.Persons;
using Menominee.Shared.Models.Vehicles;
using System.Collections.Generic;

namespace Menominee.Shared.Models.Customers;

public class CustomerToRead
{
    public long Id { get; set; }
    public string Name;
    public EntityType EntityType { get; set; }
    public CustomerType CustomerType { get; set; }
    public PersonToRead Person { get; set; }
    public BusinessToRead Business { get; set; }
    public string Code { get; set; } = default;
    public string Notes { get; set; } = default;
    public IList<VehicleToRead> Vehicles { get; set; } = new List<VehicleToRead>();
}