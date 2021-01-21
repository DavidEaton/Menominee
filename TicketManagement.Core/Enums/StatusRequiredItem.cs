using System.ComponentModel.DataAnnotations;

namespace TicketManagement.Core.Enums
{
    public enum StatusRequiredItem
    {
        Customer,
        Referrel,
        Email,
        Vehicle,
        [Display(Name = "License Plate")]
        LicensePlate,
        VIN,
        [Display(Name = "Odometer In")]
        OdometerIn,
        [Display(Name = "Odometer Out")]
        OdometerOut,
        [Display(Name = "Sales Person")]
        SalesPerson,
        [Display(Name = "Counter Person")]
        CounterPerson,
        Inspector,
        Technicians,
        [Display(Name = "Replacement Reasons")]
        ReplacementReasons,
        [Display(Name = "Buyout Info")]
        BuyoutInfo,
        [Display(Name = "Commit Parts")]
        CommitParts,
        Payment,
        [Display(Name = "Warranty Info")]
        WarrantyInfo,
        [Display(Name = "New/Used Parts")]
        NewUsedParts,
        [Display(Name = "Courtesy Check Performed (Yes/No)")]
        CourtesyCheckPerformed,
        [Display(Name = "Part Serial Numbers")]
        PartSerialNumbers,
        [Display(Name = "Customer Authorization")]
        CustomerAuthorization
    }
}
