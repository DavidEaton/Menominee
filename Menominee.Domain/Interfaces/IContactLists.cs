using Menominee.Domain.Entities;
using System.Collections.Generic;

namespace Menominee.Domain.Interfaces
{
    public interface IContactLists
    {
        IReadOnlyList<Phone> Phones { get; }
        IReadOnlyList<Email> Emails { get; }
    }
}
