using CSharpFunctionalExtensions;
using Menominee.Client.Services.Manufacturers;
using Menominee.Client.Services.ProductCodes;
using Menominee.Client.Services.SaleCodes;
using Menominee.Client.Shared;
using Menominee.Common.Enums;
using Menominee.Shared.Models.Manufacturers;
using Menominee.Shared.Models.ProductCodes;
using Menominee.Shared.Models.RepairOrders.LineItems.Item;
using Menominee.Shared.Models.SaleCodes;
using Microsoft.AspNetCore.Components;

namespace Menominee.Client.Components.RepairOrders
{
    public partial class RepairOrderItemEditor : ComponentBase
    {
        [Inject]
        public IManufacturerDataService ManufacturerDataService { get; set; }

        [Inject]
        public ISaleCodeDataService SaleCodeDataService { get; set; }

        [Inject]
        public IProductCodeDataService ProductCodeDataService { get; set; }

        [Inject]
        public ILogger<RepairOrderItemEditor> Logger { get; set; }

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
                Title = FormTitle.BuildTitle(formMode, "Item");
            }
        }

        [Parameter]
        public EventCallback OnSave { get; set; }

        [Parameter]
        public EventCallback OnCancel { get; set; }

        private IReadOnlyList<ManufacturerToReadInList> Manufacturers = new List<ManufacturerToReadInList>();
        private IReadOnlyList<SaleCodeToReadInList> SaleCodes = null;
        private IReadOnlyList<ProductCodeToReadInList> ProductCodes = null;

        private bool parametersSet = false;

        //protected override void OnInitialized()
        //{
        //    foreach (SaleType item in Enum.GetValues(typeof(SaleType)))
        //    {
        //        SaleTypeEnumData.Add(new SaleTypeEnumModel { DisplayText = EnumExtensions.GetDisplayName(item), Value = item });
        //    }
        //    foreach (ItemLaborType item in Enum.GetValues(typeof(ItemLaborType)))
        //    {
        //        LaborTypeEnumData.Add(new LaborTypeEnumModel { DisplayText = item.ToString(), Value = item });
        //    }
        //    foreach (ItemDiscountType item in Enum.GetValues(typeof(ItemDiscountType)))
        //    {
        //        DiscountTypeEnumData.Add(new DiscountTypeEnumModel { DisplayText = item.ToString(), Value = item });
        //    }

        //    base.OnInitialized();
        //}

        protected override async Task OnParametersSetAsync()
        {
            if (parametersSet)
                return;
            parametersSet = true;
            SaleCodes = (await SaleCodeDataService.GetAllAsync()).ToList();

            await ManufacturerDataService.GetAllAsync()
                .Match(
                    success =>
                    {
                        Manufacturers = success;
                    },
                    failure => Logger.LogError(failure)
                );

            ManufacturerList = new()
            {
                new()
                {
                    Id = 0,
                    Code = "",
                    Prefix = "",
                    Name = "<< All >>"
                }
            };

            foreach (var mfr in Manufacturers)
            {
                if (mfr.Id != 0 && mfr.Prefix?.Length > 0)       // FIX ME - need server to only return list of configured Mfrs
                {
                    ManufacturerList.Add(new ManufacturerX
                    {
                        Id = mfr.Id,
                        Prefix = mfr.Prefix,
                        Name = mfr.Name
                    });
                }
            }

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
        }

        private FormMode formMode;
        //private List<SaleTypeEnumModel> SaleTypeEnumData { get; set; } = new();
        //private List<LaborTypeEnumModel> LaborTypeEnumData { get; set; } = new();
        //private List<DiscountTypeEnumModel> DiscountTypeEnumData { get; set; } = new();
        private string Title { get; set; }

        private bool CanChangePart { get; set; } = true;    // will eventually stop them from changing part #, salecode, etc. as needed
        private bool CanChangeProductCode { get; set; } = false;
        private long LastOnChangeManufacturerId { get; set; } = 0;
        private long LastOnChangeSaleCodeId { get; set; } = 0;


        //private int ManufacturerId { get; set; } = 0;
        //private int SaleCodeId { get; set; } = 0;
        //private int ProductCodeId { get; set; } = 0;
        private List<ManufacturerX> ManufacturerList = new();
        private List<SaleCodeX> SaleCodeList = new();
        private List<ProductCodeX> ProductCodeList = new();

        // placeholder list of manufacturers
        //public List<ManufacturerX> Manufacturers = new List<ManufacturerX>()
        //{
        //    new ManufacturerX { Id="0", Code="I", Name="Custom"},
        //    new ManufacturerX { Id="1", Code="X", Name="Miscellaneous"},
        //    new ManufacturerX { Id="2349", Code="ACD", Name="AC Delco"},
        //    new ManufacturerX { Id="2389", Code="CAS", Name="Castrol"},
        //    new ManufacturerX { Id="3102", Code="BB", Name="Bendix Brakes"},
        //    new ManufacturerX { Id="5700", Code="WE", Name="Walker Exhaust"}
        //};

        //// placeholder list of sale codes
        //public List<SaleCodeX> SaleCodes = new List<SaleCodeX>()
        //{
        //    new SaleCodeX { Code="A", Description="Alignments"},
        //    new SaleCodeX { Code="B", Description="Brakes"},
        //    new SaleCodeX { Code="E", Description="Exhaust"},
        //    new SaleCodeX { Code="S", Description="Suspension"},
        //    new SaleCodeX { Code="BA", Description="Batteries"},
        //    new SaleCodeX { Code="L", Description="Lube/Oil/Filter"},
        //    new SaleCodeX { Code="T", Description="Tires"}
        //};

        //// placeholder list of product codes
        //public List<ProductCodeX> ProductCodes = new List<ProductCodeX>()
        //{
        //    new ProductCodeX { ManufacturerId="0", SaleCode="A", Code="000A", Description="Alignments" },
        //    new ProductCodeX { ManufacturerId="0", SaleCode="B", Code="000B", Description="Brakes" },
        //    new ProductCodeX { ManufacturerId="1", SaleCode="E", Code="100E", Description="Exhaust" },
        //    new ProductCodeX { ManufacturerId="1", SaleCode="S", Code="100S", Description="Suspension" },
        //    new ProductCodeX { ManufacturerId="2389", SaleCode="L", Code="1111", Description="Full Synthetic Oil" },
        //    new ProductCodeX { ManufacturerId="2389", SaleCode="L", Code="1112", Description="Synthetic Blend Oil" },
        //    new ProductCodeX { ManufacturerId="3102", SaleCode="B", Code="2221", Description="Semi-Metalic Brake Pads" },
        //    new ProductCodeX { ManufacturerId="3102", SaleCode="B", Code="2222", Description="Rotors" },
        //    new ProductCodeX { ManufacturerId="5700", SaleCode="E", Code="3331", Description="Muffler" },
        //    new ProductCodeX { ManufacturerId="5700", SaleCode="E", Code="3332", Description="Exhaust Pipe" }
        //};


        public class ManufacturerX
        {
            public long Id { get; set; }
            public string Code { get; set; }
            public string Prefix { get; set; }
            public string Name { get; set; }
            public string DisplayText
            {
                get
                {
                    return Prefix + " - " + Name;
                }
            }
        }

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

        public async Task HandleManufacturerChange(object userInput)
        {
            var currentManufacturerId = (long)userInput;

            if (currentManufacturerId.Equals(LastOnChangeManufacturerId) || Item.SaleCode is null)
                return;

            LastOnChangeManufacturerId = currentManufacturerId;

            await LoadProductCodes();
        }

        public async Task HandleSaleCodeChange(object userInput)
        {
            var currentSaleCodeId = (long)userInput;

            if (currentSaleCodeId.Equals(LastOnChangeSaleCodeId) || Item.Manufacturer is null)
                return;

            LastOnChangeSaleCodeId = currentSaleCodeId;

            await LoadProductCodes();
        }

        private async Task LoadProductCodes()
        {
            await ProductCodeDataService.GetByManufacturerAndSaleCodeAsync(Item.Manufacturer.Id, Item.SaleCode.Id)
                .Match(

                success =>
                {
                    ProductCodes = success;
                },
                failure => Logger.LogError(failure));

            SetProductCodeList();

            CanChangeProductCode = ProductCodeList.Count > 0;

            SetItemProductCode();
        }

        private void SetItemProductCode()
        {
            if (ProductCodeList.Count.Equals(1))
            {
                Item.ProductCode = new()
                {
                    Id = ProductCodes[0].Id,
                    Code = ProductCodes[0].Code,
                    Name = ProductCodes[0].Name,
                };
            }
        }

        public void SetProductCodeList()
        {
            ProductCodeList = ProductCodes.Select(prodCode => new ProductCodeX
            {
                Id = prodCode.Id,
                Code = prodCode.Code,
                Name = prodCode.Name
            }).ToList();
        }
    }
}
