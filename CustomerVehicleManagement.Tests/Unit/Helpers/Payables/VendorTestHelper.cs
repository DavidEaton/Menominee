using CustomerVehicleManagement.Domain.Entities.Payables;
using Menominee.Common.Enums;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Tests.Unit.Helpers.Payables
{
    public class VendorTestHelper
    {
        public static List<Vendor> CreateVendors(int count, DefaultPaymentMethod defaultPaymentMethod = null)
        {
            var list = new List<Vendor>();

            for (int i = 0; i < count; i++)
            {
                if (i % 2 == 0)
                    list.Add(CreateVendor(defaultPaymentMethod));

                list.Add(CreateVendor(defaultPaymentMethod: null));
            }

            return list;
        }

        public static Vendor CreateVendor(DefaultPaymentMethod defaultPaymentMethod = null)
        {
            return Vendor.Create(
                name: Utilities.RandomCharacters(Vendor.MinimumLength),
                vendorCode: Utilities.RandomCharacters(Vendor.MinimumLength),
                vendorRole: VendorRole.PartsSupplier,
                defaultPaymentMethod: defaultPaymentMethod).Value;
        }

    }
}
