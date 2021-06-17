using CustomerVehicleManagement.Shared.Models;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Grids;
using System.Collections.Generic;
using System.Net.Http;

namespace Menominee.UiExperiments.Pages
{
    public partial class Persons
    {
        [Inject]
        public HttpClient HttpClient { get; set; }
        public IEnumerable<PersonReadDto> PersonsList;
        public int SelectedId { get; set; }



        public void RowSelectedHandler(RowSelectEventArgs<PersonReadDto> args)
        {
            SelectedId = args.Data.Id;
        }

    }
}
