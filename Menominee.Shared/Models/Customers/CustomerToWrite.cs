using Menominee.Shared.Models.Persons;
using Menominee.Common.Enums;

namespace Menominee.Shared.Models.Customers;

public class CustomerToWrite
{
    public long Id { get; set; }
    public EntityType EntityType { get; set; }
    public CustomerType CustomerType { get; set; }
    public PersonToWrite Person { get; set; }
    public BusinessToWrite Business { get; set; }
    public string Code { get; set; } = null;

    //public ContactPreferences ContactPreferences { get; set; }
}
