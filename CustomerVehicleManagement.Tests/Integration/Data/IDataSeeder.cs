using Menominee.Common;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Tests.Integration.Data
{
    public interface IDataSeeder
    {
        void SeedData<T>(List<T> entities) where T : Entity;
    }
}
