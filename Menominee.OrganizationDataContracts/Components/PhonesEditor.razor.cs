using CustomerVehicleManagement.Shared.Models;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace Menominee.OrganizationDataContracts.Components
{
    public partial class PhonesEditor : ComponentBase
    {
        public PhoneToWrite PhoneToWrite { get; set; }

        [Parameter]
        public IList<PhoneToWrite> PhonesToWrite { get; set; }
        private bool DialogVisible => PhoneToWrite != null && (Adding || Editing);
        private bool Adding { get; set; } = false;
        private bool Editing { get; set; } = false;

        private void Edit(PhoneToWrite item)
        {
            PhoneToWrite = item;
            Editing = true;
        }

        private void Add()
        {
            PhoneToWrite = new();
            Adding = true;
        }

        private void Save()
        {
            if (Adding)
                PhonesToWrite.Add(PhoneToWrite);

            Adding = false;
            Editing = false;
        }

        private void Cancel()
        {
            if (PhoneToWrite != null && Adding)
                PhoneToWrite = null;

            Adding = false;
            Editing = false;
        }
    }
}
