﻿using CustomerVehicleManagement.Domain.Entities.Payables;

namespace CustomerVehicleManagement.Data
{
    public static class TestDataGeneratorResults
    {
        public static IReadOnlyList<Vendor> Vendors { get; set; } = new List<Vendor>();
        public static IReadOnlyList<VendorInvoicePaymentMethod> PaymentMethods { get; set; } = new List<VendorInvoicePaymentMethod>();
        public static IReadOnlyList<DefaultPaymentMethod> DefaultPaymentMethods { get; set; } = new List<DefaultPaymentMethod>();

    }
}
