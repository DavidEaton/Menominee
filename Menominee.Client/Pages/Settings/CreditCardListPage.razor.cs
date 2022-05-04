using CustomerVehicleManagement.Shared.Models.CreditCards;
using Menominee.Client.Services.CreditCards;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telerik.Blazor.Components;
using Telerik.DataSource;

namespace Menominee.Client.Pages.Settings
{
    public partial class CreditCardListPage : ComponentBase
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public ICreditCardDataService CreditCardDataService { get; set; }

        [Parameter]
        public long ItemToSelect { get; set; } = 0;

        public IReadOnlyList<CreditCardToReadInList> CreditCardList;
        public IEnumerable<CreditCardToReadInList> SelectedCreditCards { get; set; } = Enumerable.Empty<CreditCardToReadInList>();
        public CreditCardToReadInList SelectedCreditCard { get; set; }
        public CreditCardToWrite CreditCardToModify { get; set; } = null;

        public long SelectedId
        {
            get => selectedId;
            set
            {
                selectedId = value;
                CanEdit = selectedId > 0;
                CanDelete = selectedId > 0;
            }
        }

        public TelerikGrid<CreditCardToReadInList> Grid { get; set; }

        private long selectedId;

        private bool CanEdit { get; set; } = false;
        private bool CanDelete { get; set; } = false;
        private bool EditCardDialogVisible { get; set; } = false;  

        protected override async Task OnInitializedAsync()
        {
            CreditCardList = (await CreditCardDataService.GetAllCreditCardsAsync()).ToList();

            if (CreditCardList.Count > 0)
            {
                if (ItemToSelect == 0)
                {
                    SelectedCreditCard = CreditCardList.FirstOrDefault();
                }
                else
                {
                    SelectedCreditCard = CreditCardList.Where(x => x.Id == ItemToSelect).FirstOrDefault();
                }
                SelectedId = SelectedCreditCard.Id;
                SelectedCreditCards = new List<CreditCardToReadInList> { SelectedCreditCard };
            }
        }

        protected override async Task OnParametersSetAsync()
        {
            GridState<CreditCardToReadInList> desiredState = new GridState<CreditCardToReadInList>()
            {
                SortDescriptors = new List<SortDescriptor>()
                {
                    new SortDescriptor { Member = "Name", SortDirection = ListSortDirection.Descending }
                }
            };

            if (Grid != null)
                await Grid?.SetState(desiredState);
        }

        private void OnAdd()
        {
            SelectedId = 0;
            CreditCardToModify = new();
            CreditCardToModify.FeeType = CreditCardFeeType.None;
            EditCardDialogVisible = true;
        }

        private async Task OnEditAsync()
        {
            if (SelectedId != 0)
            {
                CreditCardToRead cc = await CreditCardDataService.GetCreditCardAsync(SelectedId);
                if (cc != null)
                {
                    CreditCardToModify = CreditCardHelper.Transform(cc);
                    EditCardDialogVisible = true;
                }
            }
        }

        private void OnDelete()
        {
            SelectedId = 0;
        }

        protected void OnSelect(IEnumerable<CreditCardToReadInList> creditCards)
        {
            SelectedCreditCard = creditCards.FirstOrDefault();
            SelectedCreditCards = new List<CreditCardToReadInList> { SelectedCreditCard };
        }

        private void OnRowSelected(GridRowClickEventArgs args)
        {
            SelectedId = (args.Item as CreditCardToReadInList).Id;
        }

        private void OnDone()
        {
            NavigationManager.NavigateTo("/settings/");
        }

        private async Task OnSaveEditAsync()
        {
            if (CreditCardToModify?.FeeType == CreditCardFeeType.None)
            {
                CreditCardToModify.Fee = 0.0;
            }

            if (SelectedId == 0)
            {
                CreditCardToRead cc = await CreditCardDataService.AddCreditCardAsync(CreditCardToModify);
                if (cc != null)
                {
                    SelectedId = cc.Id;
                }
            }
            else
            {
                await CreditCardDataService.UpdateCreditCardAsync(CreditCardToModify, SelectedId);
            }

            if (SelectedId > 0)
            {
                CreditCardList = (await CreditCardDataService.GetAllCreditCardsAsync()).ToList();
                SelectedCreditCard = CreditCardList.Where(x => x.Id == SelectedId).FirstOrDefault();
                SelectedCreditCards = new List<CreditCardToReadInList> { SelectedCreditCard };
                Grid.Rebind();
            }

            EditCardDialogVisible = false;
        }

        private void OnCancelEdit()
        {
            EditCardDialogVisible = false;
        }
    }
}
