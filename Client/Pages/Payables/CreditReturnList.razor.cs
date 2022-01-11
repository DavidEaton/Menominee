using Blazored.Modal;
using Blazored.Modal.Services;
using MenomineePlayWASM.Client.Components.Shared;
using MenomineePlayWASM.Shared.Entities.Payables.CreditReturns;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Grids;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MenomineePlayWASM.Client.Pages.Payables
{
    public partial class CreditReturnList
    {
        [Inject]
        public HttpClient HttpClient { get; set; }

        [Inject]
        private NavigationManager navigationManager { get; set; }

        //[Inject]
        //private ICreditReturnRepository creditReturnRepository { get; set; }

        [CascadingParameter]
        IModalService ModalService { get; set; }

        private long selectedId = 0;

        public List<CreditReturn> CreditReturns;

        private void OnEditReturn()
        {
            if (selectedId == 0)
            {
                var parameters = new ModalParameters();
                parameters.Add(nameof(ModalMessage.Message), "Please select a credit return to edit.");
                ModalService.Show<ModalMessage>("Edit Credit Return", parameters);

                //var options = new ModalOptions { HideCloseButton = true };
                //ModalService.Show<Confirm>("Hiding Close Button", options);
            }
            else
            {
                navigationManager.NavigateTo($"/payables/returns/edit/{selectedId}");
            }
        }

        private void OnDone()
        {
            navigationManager.NavigateTo("/payables/");
        }

        private void OnNewReturn()
        {
            navigationManager.NavigateTo("/payables/returns/create/");
        }

        private void OnDeleteReturn()
        {
            //navigationManager.NavigateTo("/payables/.../");
        }

        public void OnRowSelected(RowSelectEventArgs<CreditReturn> args)
        {
            selectedId = args.Data.Id;
        }

        //protected override async Task OnInitializedAsync()
        //{
        //    try
        //    {
        //        CreditReturns = await creditReturnRepository.GetReturnsAsync();
        //        CreditReturns = FormatReturnData(CreditReturns);
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

        private static List<CreditReturn> FormatReturnData(List<CreditReturn> returns)
        {
            foreach (var creditReturn in returns)
            {
                creditReturn.DateAsString = creditReturn.Date?.ToShortDateString();
            }

            return returns;
        }
    }
}

