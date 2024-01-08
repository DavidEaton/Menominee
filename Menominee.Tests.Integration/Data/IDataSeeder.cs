using Menominee.Domain.BaseClasses;
using System.Collections.Generic;

namespace Menominee.Tests.Integration.Data
{
    public interface IDataSeeder
    {
        void Save<T>(List<T> entities) where T : Entity;
    }
}
