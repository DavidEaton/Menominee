using CustomerVehicleManagement.Shared.Models.CreditCards;
using Menominee.Client.Services.CreditCards;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telerik.Blazor.Components;

namespace Menominee.Client.Components.Settings.Pages
{
    public partial class CreditCardListPage : ComponentBase
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public ICreditCardDataService CreditCardDataService { get; set; }

        [Parameter]
        public long Id { get; set; } = 0;

        public IReadOnlyList<CreditCardToReadInList> CreditCards;
        public IEnumerable<CreditCardToReadInList> SelectedCreditCards { get; set; } = Enumerable.Empty<CreditCardToReadInList>();
        public CreditCardToReadInList SelectedCreditCard { get; set; }
        public CreditCardToWrite CreditCard { get; set; } = null;

        public TelerikGrid<CreditCardToReadInList> Grid { get; set; }

        private bool CanEdit { get => Id > 0; }
        private bool CanDelete { get => Id > 0; }

        private FormMode EditFormMode = FormMode.Unknown;

        protected override async Task OnInitializedAsync()
        {
            CreditCards = (await CreditCardDataService.GetAllCreditCardsAsync()).ToList();

            if (CreditCards?.Count > 0)
            {
                SelectedCreditCard = CreditCards.FirstOrDefault();
                Id = SelectedCreditCard.Id;
                SelectedCreditCards = new List<CreditCardToReadInList> { SelectedCreditCard };
            }
        }

        // TODO - Failed attempt to sort by name initially
        //protected override async Task OnParametersSetAsync()
        //{
        //    GridState<CreditCardToReadInList> desiredState = new GridState<CreditCardToReadInList>()
        //    {
        //        SortDescriptors = new List<SortDescriptor>()
        //        {
        //            new SortDescriptor { Member = "Name", SortDirection = ListSortDirection.Descending }
        //        }
        //    };

        //    if (Grid != null)
        //        await Grid?.SetState(desiredState);
        //}

        private void OnAdd()
        {
            //Id = 0;
            CreditCard = new();
            CreditCard.FeeType = CreditCardFeeType.None;
            EditFormMode = FormMode.Add;
        }

        private async Task RowDoubleClickAsync(GridRowClickEventArgs args)
        {
            Id = (args.Item as CreditCardToReadInList).Id;
            await OnEditAsync();
        }

        private async Task OnEditAsync()
        {
            if (Id != 0)
            {
                CreditCardToRead cc = await CreditCardDataService.GetCreditCardAsync(Id);
                if (cc != null)
                {
                    CreditCard = CreditCardHelper.CreateCreditCard(cc);
                }
                EditFormMode = FormMode.Edit;
            }
        }

        private void OnDelete()
        {
            //if (Id > 0)
            //    await CreditCardDataService.Delete(Id);
        }

        protected void OnSelect(IEnumerable<CreditCardToReadInList> creditCards)
        {
            SelectedCreditCard = creditCards.FirstOrDefault();
            SelectedCreditCards = new List<CreditCardToReadInList> { SelectedCreditCard };
        }

        private void OnRowSelected(GridRowClickEventArgs args)
        {
            Id = (args.Item as CreditCardToReadInList).Id;
        }

        private void OnDone()
        {
            NavigationManager.NavigateTo("/settings/");
        }

        protected async Task HandleAddSubmitAsync()
        {
            if (CreditCard.FeeType == CreditCardFeeType.None)
                CreditCard.Fee = 0.0;

            Id = (await CreditCardDataService.AddCreditCardAsync(CreditCard)).Id;
            await EndAddEditAsync();
            Grid.Rebind();
        }

        protected async Task HandleEditSubmitAsync()
        {
            if (CreditCard.FeeType == CreditCardFeeType.None)
                CreditCard.Fee = 0.0;

            await CreditCardDataService.UpdateCreditCardAsync(CreditCard, Id);
            await EndAddEditAsync();
        }

        protected async Task SubmitHandlerAsync()
        {
            if (EditFormMode == FormMode.Add)
                await HandleAddSubmitAsync();
            else if (EditFormMode == FormMode.Edit)
                await HandleEditSubmitAsync();
        }

        protected async Task EndAddEditAsync()
        {
            EditFormMode = FormMode.Unknown;
            CreditCards = (await CreditCardDataService.GetAllCreditCardsAsync()).ToList();
            SelectedCreditCard = CreditCards.Where(x => x.Id == Id).FirstOrDefault();
            SelectedCreditCards = new List<CreditCardToReadInList> { SelectedCreditCard };
        }
    }
}
