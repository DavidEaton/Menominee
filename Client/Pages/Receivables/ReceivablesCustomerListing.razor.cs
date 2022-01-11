using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Grids;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MenomineePlayWASM.Client.Pages.Receivables
{
    public partial class ReceivablesCustomerListing
    {
        private string url = "/receivables/customers";

        [Inject]
        public HttpClient HttpClient { get; set; }

        [Inject]
        private NavigationManager navigationManager { get; set; }

        [CascadingParameter]
        IModalService ModalService { get; set; }

        private int selectedId = 0;

        private void OnDone()
        {
            navigationManager.NavigateTo("/receivables");
        }

        private void OnEditCustomer()
        {
            navigationManager.NavigateTo($"{url}/edit/{selectedId}");
        }

        private void OnNewCustomer()
        {
            navigationManager.NavigateTo($"{url}/create");
        }

        //public void OnRowSelected(RowSelectEventArgs<CreditReturn> args)
        //{
        //    selectedId = args.Data.Id;
        //}

        //protected override async Task OnInitializedAsync()
        //{
        //    try
        //    {
        //        CreditReturns = await creditReturnRepository.GetReturns();
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}
    }
}
