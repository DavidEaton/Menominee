using Menominee.Shared.Models.ProductCodes;
using Menominee.Shared.Models.RepairOrders.LineItems;
using Menominee.Shared.Models.RepairOrders.LineItems.Item;
using Menominee.Shared.Models.SaleCodes;
using Menominee.Client.Services.ProductCodes;
using Menominee.Client.Services.SaleCodes;
using Menominee.Client.Shared;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Menominee.Client.Components.RepairOrders
{
    public partial class RepairOrderLaborEditor : ComponentBase
    {
        [Inject]
        public ISaleCodeDataService saleCodeDataService { get; set; }

        [Inject]
        public IProductCodeDataService productCodeDataService { get; set; }

        [Parameter]
        public RepairOrderItemToWrite Item { get; set; }

        [Parameter]
        public bool DialogVisible { get; set; }

        [Parameter]
        public FormMode Mode
        {
            get => formMode;
            set
            {
                formMode = value;
                Title = FormTitle.BuildTitle(formMode, "Labor");
            }
        }

        [Parameter]
        public EventCallback OnSave { get; set; }

        [Parameter]
        public EventCallback OnCancel { get; set; }

        private IReadOnlyList<SaleCodeToReadInList> SaleCodes = null;
        private IReadOnlyList<ProductCodeToReadInList> ProductCodes = null;

        protected override void OnInitialized()
        {
            foreach (ItemLaborType item in Enum.GetValues(typeof(ItemLaborType)))
            {
                LaborTypeEnumData.Add(new LaborTypeEnumModel { DisplayText = item.ToString(), Value = item });
            }

            base.OnInitialized();
        }

        protected override async Task OnParametersSetAsync()
        {
            SaleCodes = (await saleCodeDataService.GetAllSaleCodesAsync()).ToList();
            ProductCodes = (await productCodeDataService.GetAllProductCodesAsync()).ToList();

            SaleCodeList = new();
            foreach (var saleCode in SaleCodes)
            {
                SaleCodeList.Add(new SaleCodeX
                {
                    Id = saleCode.Id,
                    Code = saleCode.Code,
                    Name = saleCode.Name
                });
            }

            ProductCodeList = new();
            foreach (var prodCode in ProductCodes)
            {
                ProductCodeList.Add(new ProductCodeX
                {
                    Id = prodCode.Id,
                    Code = prodCode.Code,
                    Name = prodCode.Name
                });
            }
        }

        private FormMode formMode;
        private List<LaborTypeEnumModel> LaborTypeEnumData { get; set; } = new List<LaborTypeEnumModel>();
        private string Title { get; set; }

        private bool CanChangePart { get; set; } = true;    // will eventually stop them from changing part #, salecode, etc. as needed

        private List<SaleCodeX> SaleCodeList = new();
        private List<ProductCodeX> ProductCodeList = new();

        public class SaleCodeX
        {
            public long Id { get; set; }
            public string Code { get; set; }
            public string Name { get; set; }
            public string DisplayText
            {
                get
                {
                    return Code + " - " + Name;
                }
            }
        }

        public class ProductCodeX
        {
            public long Id { get; set; }
            public string Code { get; set; }
            //public string SaleCode { get; set; }
            public string Name { get; set; }
            public string DisplayText
            {
                get
                {
                    return Code + " - " + Name;
                }
            }
        }

    }
}
