using System.Linq;
using System.Threading.Tasks;
using CustomerVehicleManagement.Api.Inventory;
using CustomerVehicleManagement.Shared.Models.Inventory.InventoryItems;
using CustomerVehicleManagement.Shared.Models.Manufacturers;
using CustomerVehicleManagement.Shared.Models.ProductCodes;
using TestingHelperLibrary;

namespace CustomerVehicleManagement.Tests.Integration
{
    public class InventoryTestHelper : InventoryItemTestHelper
    {

        public static async Task<InventoryItemToRead> GetItem(long id)
        {
            using (var context = IntegrationTestBase.CreateTestContext())
            {
                var repo = new InventoryItemRepository(context);
                return await repo.GetItem(id);
            }
        }

        public static ProductCodeToRead GetProductCodeToRead()
        {
            using var context = IntegrationTestBase.CreateTestContext();

            return ProductCodeHelper.ConvertEntityToReadDto(
                context.ProductCodes.First());
        }

        public static ManufacturerToRead GetManufacturerToRead()
        {
            using var context = IntegrationTestBase.CreateTestContext();

            return ManufacturerHelper.ConvertEntityToReadDto(
                context.Manufacturers.First());
        }
    }
}
