using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Menominee.Common.Enums
{
    public enum InventoryItemType
    {
        Part,
        Labor,
        Tire,
        [Display(Name = "Courtesy Check")]
        CourtesyCheck,
        Donation,
        Warranty,
        [Display(Name = "Gift Certificate")]
        GiftCertificate
    }
}
