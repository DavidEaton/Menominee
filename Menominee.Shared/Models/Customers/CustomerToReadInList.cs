using Menominee.Common.Enums;

namespace Menominee.Shared.Models.Customers;

public class CustomerToReadInList
{
    public long Id { get; set; } = default;
    public EntityType EntityType { get; set; }
    public string Name { get; set; } = default;
    public string AddressFull { get; set; } = default;
    public string PrimaryPhone { get; set; } = default;
    public string PrimaryEmail { get; set; } = default;
    public CustomerType CustomerType { get; set; } = default;
    public string Code { get; set; } = null;
}
