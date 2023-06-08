using Menominee.Common;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Tests.Integration.Data
{
    public interface IDataSeeder
    {
        void Save<T>(List<T> entities) where T : Entity;
    }
}
