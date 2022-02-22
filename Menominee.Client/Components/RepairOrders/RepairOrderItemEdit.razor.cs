﻿using CustomerVehicleManagement.Domain.Enums;
using CustomerVehicleManagement.Shared.Models.Manufacturers;
using CustomerVehicleManagement.Shared.Models.ProductCodes;
using CustomerVehicleManagement.Shared.Models.RepairOrders.Items;
using CustomerVehicleManagement.Shared.Models.SaleCodes;
using Menominee.Client.Services.Manufacturers;
using Menominee.Client.Services.ProductCodes;
using Menominee.Client.Services.SaleCodes;
using Menominee.Common.Enums;
//using MenomineePlayWASM.Shared.Entities.RepairOrders.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Menominee.Client.Components.RepairOrders
{
    public partial class RepairOrderItemEdit : ComponentBase
    {
        [Inject]
        public IManufacturerDataService manufacturerDataService { get; set; }

        [Inject]
        public ISaleCodeDataService saleCodeDataService { get; set; }

        [Inject]
        public IProductCodeDataService productCodeDataService { get; set; }

        //[Inject]
        //IJSRuntime _js { get; set; }

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
                if (formMode == FormMode.Add)
                    Title = "Add";
                else if (formMode == FormMode.Edit)
                    Title = "Edit";
                else
                    Title = "View";
                Title += " Item";
            }
        }

        [Parameter]
        public EventCallback OnSave { get; set; }

        [Parameter]
        public EventCallback OnCancel { get; set; }

        private IReadOnlyList<ManufacturerToReadInList> Manufacturers = null;
        private IReadOnlyList<SaleCodeToReadInList> SaleCodes = null;
        private IReadOnlyList<ProductCodeToReadInList> ProductCodes = null;

        protected override void OnInitialized()
        {
            foreach (SaleType item in Enum.GetValues(typeof(SaleType)))
            {
                //SaleTypeEnumData.Add(new SaleTypeEnumModel { DisplayText = item.ToString(), Value = item });
                SaleTypeEnumData.Add(new SaleTypeEnumModel { DisplayText = EnumExtensions.GetDisplayName(item), Value = item });
            }
            foreach (ItemLaborType item in Enum.GetValues(typeof(ItemLaborType)))
            {
                LaborTypeEnumData.Add(new LaborTypeEnumModel { DisplayText = item.ToString(), Value = item });
            }
            foreach (ItemDiscountType item in Enum.GetValues(typeof(ItemDiscountType)))
            {
                DiscountTypeEnumData.Add(new DiscountTypeEnumModel { DisplayText = item.ToString(), Value = item });
            }

            base.OnInitialized();
        }

        protected override async Task OnParametersSetAsync()
        {
            Manufacturers = (await manufacturerDataService.GetAllManufacturers()).ToList();
            SaleCodes = (await saleCodeDataService.GetAllSaleCodes()).ToList();
            ProductCodes = (await productCodeDataService.GetAllProductCodes()).ToList();    // FIX ME - need to restrict list to mfr, salecode

            ManufacturerList = new();
            foreach (var mfr in Manufacturers)
            {
                if (mfr.Prefix.Length > 0)
                {
                    ManufacturerList.Add(new ManufacturerX
                    {
                        Id = mfr.Id,
                        Code = mfr.Code,
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
        private List<SaleTypeEnumModel> SaleTypeEnumData { get; set; } = new List<SaleTypeEnumModel>();
        private List<LaborTypeEnumModel> LaborTypeEnumData { get; set; } = new List<LaborTypeEnumModel>();
        private List<DiscountTypeEnumModel> DiscountTypeEnumData { get; set; } = new List<DiscountTypeEnumModel>();
        private string Title { get; set; }

        private bool CanChangePart { get; set; } = true;    // will eventually stop them from changing part #, salecode, etc. as needed

        // replace the following when able
        private string PlaceholderReasonForReplacement { get; set; } = string.Empty;
        private int PlaceholderQuantityOnHand { get; set; } = 0;

        private int ManufacturerId { get; set; } = 0;
        private int SaleCodeId { get; set; } = 0;
        private int ProductCodeId { get; set; } = 0;
        private List<ManufacturerX> ManufacturerList = new List<ManufacturerX>();
        private List<SaleCodeX> SaleCodeList = new List<SaleCodeX>();
        private List<ProductCodeX> ProductCodeList = new List<ProductCodeX>();

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

        public List<ReasonForReplacement> ReasonsForReplacement = new List<ReasonForReplacement>()
        {
            new ReasonForReplacement { Code = "A", Type = ReasonForReplacementType.Required, Description = "Part no longer performs intended function" },
            new ReasonForReplacement { Code = "B", Type = ReasonForReplacementType.Required, Description = "Part does not meet a design specification" },
            new ReasonForReplacement { Code = "C", Type = ReasonForReplacementType.Required, Description = "Part is missing" },
            new ReasonForReplacement { Code = "D", Type = ReasonForReplacementType.Required, Description = "Necessary component of service" },
            new ReasonForReplacement { Code = "1", Type = ReasonForReplacementType.Suggested, Description = "Part is close to end of useful life" },
            new ReasonForReplacement { Code = "2", Type = ReasonForReplacementType.Suggested, Description = "Customer need, convenience or request" },
            new ReasonForReplacement { Code = "3", Type = ReasonForReplacementType.Suggested, Description = "Comply with recommended maintenance" },
            new ReasonForReplacement { Code = "4", Type = ReasonForReplacementType.Suggested, Description = "Technician's recommendation" }
        };

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

        public class SaleTypeEnumModel
        {
            public SaleType Value { get; set; }
            public string DisplayText { get; set; }
        }

        public class LaborTypeEnumModel
        {
            public ItemLaborType Value { get; set; }
            public string DisplayText { get; set; }
        }

        public class DiscountTypeEnumModel
        {
            public ItemDiscountType Value { get; set; }
            public string DisplayText { get; set; }
        }

        public enum ReasonForReplacementType
        {
            Required,
            Suggested
        }

        public class ReasonForReplacement
        {
            public string Code { get; set; }
            public ReasonForReplacementType Type { get; set; }
            public string Description { get; set; }
            public string DisplayText
            {
                get
                {
                    return Code + " - " + Description;
                }
            }
        }
    }
}