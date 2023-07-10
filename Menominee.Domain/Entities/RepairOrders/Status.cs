using System.ComponentModel.DataAnnotations;

namespace Menominee.Domain.Entities.RepairOrders
{
    public enum Status
    {
        New,

        [Display(Name = "Estimate/Quote")]
        EstimateQuote,

        Approved,

        [Display(Name = "In Progress")]
        InProgress,

        [Display(Name = "On Hold")]
        OnHold,

        Invoiced,

        [Display(Name = "Awaiting Payment")]
        AwaitingPayment,

        Completed,

        [Display(Name = "Picked Up")]
        PickedUp
    }
}
