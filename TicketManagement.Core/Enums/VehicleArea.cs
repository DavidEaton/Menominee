using System.ComponentModel.DataAnnotations;

namespace TicketManagement.Core.Enums
{
    public enum VehicleArea
    {
        Front,
        Rear,
        Left,
        Right,
        [Display(Name = "Left Front")]
        LeftFront,
        [Display(Name = "Right Front")]
        RightFront,
        [Display(Name = "Left Rear")]
        LeftRear,
        [Display(Name = "Right Rear")]
        RightRear,
        Center,
        [Display(Name = "Front Rear")]
        FrontRear,
        [Display(Name = "Left Right")]
        LeftRight
    }
}
