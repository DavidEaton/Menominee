using Menominee.Common.Enums;

namespace Menominee.Domain.Interfaces
{
    public interface ICustomerEntity : IContactable
    {
        string DisplayName { get; }
        EntityType EntityType { get; }
    }
}