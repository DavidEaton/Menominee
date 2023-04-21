using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

/*
 * VK: I'll be outlining what I did using "VK" comments. I'll also put importance/priority to my edits, so you know how important that edit is:
 * Importance (Im.) 1: minor issue, borderline matter of taste. You can reverse the edit freely if you prefer the old way
 * Im.2: medium priority, something I'd generally recommend you do
 * Im.3: something very important
 */

namespace CustomerVehicleManagement.Tests.Integration.Tests
{
    /*
     * VK, Im.1: I don't really like this "Should" notation. A test must convey a story about the application in plain English,
     * and when we read a "should" test, it makes it seem as though the application should do what the test's name says,
     * e.g it should always add/read/delete inventory items. A better way would be this:
     * InventoryItemsControllerShould -> InventoryItemsController
     * AddInventoryItem -> CanAddInventoryItem
     * So the whole thing would read as "Inventory items controller can add inventory item". This way, you are emphasizing optionality.
     * I normally don't do even that, and just go for "[Controller]Tests" notation, which is the simplest option.
     *
     * Im.2: Don't use "Async" in test names, those are implementation details that don't have anything to do with the story the test tells
     * Im.1: Put underscores between words in test name
     */

    public class InventoryItemsControllerShould
    {
        private const int NoContent = 204;
        private const int Created = 201;

        // VK Im.2: Arrange and Act sections tend to have lots of duplicated code;
        // it's best to extract the repeating parts

        // Only test the controller’s observable behavior, as it’s perceived by its client
        //[Fact]
        //public async Task Add_an_inventory_item()
        //{
        //    // Each test creates its own set of data

        //    // Arrange

        //    // Act

        //    // VK Im.1: You can assert against what the API returns in GetInventoryItem, but it's easier to just check
        //    // the object in the database. Moreover, you don't always have appropriate APIs to peer into the DB, so 
        //    // asserting against domain objects is often the only choice. Article on this topic: https://enterprisecraftsmanship.com/posts/how-to-assert-database-state/
        //    // Assert
        //    true.Should().BeTrue();
        //}

        // VK Im.2: Read-only operations aren't that important for integration testing because bugs in them usually don't lead to
        // currupted data. There's no complex logic in read operations, you can skip testing them
        //[Fact]
        //public async Task Get_inventory_items()
        //{

        //}

        //[Fact]
        //public async Task Delete_an_InventoryItem()
        //{

        //}

        //[Fact]
        //public async Task Update_an_InventoryItem()
        //{

        //}

        //[Fact]
        //public async Task Get_an_InventoryItem_by_Id()
        //{

        //}


        //[Fact]
        //public async Task Get_an_InventoryItem_by_Manufacturer_Id_and_Part_Number()
        //{

        //}


        //[Fact]
        //public async Task Not_add_invalid_inventory_item_but_return_Status404NotFound()
        //{

        //}

        //[Fact]
        //public async Task Not_update_invalid_inventory_item_but_return_Status404NotFound()
        //{

        //}

        //[Fact]
        //public async Task Not_delete_invalid_inventory_item_but_return_Status404NotFound()
        //{

        //}

        //[Fact]
        //public async Task Not_get_invalid_inventory_item_but_return_NotFoundResult()
        //{

        //}

        //[Fact]
        //public async Task Not_get_inventory_item_by_invalid_manufacturer_Id_and_part_number_but_return_NotFoundResult()
        //{


        //}

    }
}
