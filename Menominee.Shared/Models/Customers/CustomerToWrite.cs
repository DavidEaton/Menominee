using Menominee.Common.Enums;
using Menominee.Shared.Models.Businesses;
using Menominee.Shared.Models.Persons;
using Menominee.Shared.Models.Vehicles;
using System.Collections.Generic;

namespace Menominee.Shared.Models.Customers;

public class CustomerToWrite
{
    public long Id { get; set; }
    public string Name => GetFullName();
    public EntityType EntityType { get; set; }
    public CustomerType CustomerType { get; set; }
    public PersonToWrite Person { get; set; }
    public BusinessToWrite Business { get; set; }
    public string Code { get; set; } = null;
    public IList<VehicleToWrite> Vehicles { get; set; } = new List<VehicleToWrite>();
    private string GetFullName()
    {
        return EntityType switch
        {
            EntityType.Person => string.IsNullOrWhiteSpace(Person.Name.MiddleName) ? $"{Person.Name.FirstName} {Person.Name.LastName}" : $"{Person.Name.FirstName} {Person.Name.MiddleName} {Person.Name.LastName}",
            EntityType.Business => Business.Name,
            _ => string.Empty
        };
    }
}
